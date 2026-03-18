import type { AxiosInstance } from "axios";
import type { ApiProductType, ProductType } from "../types/productType";
import { ApiProductsSchema } from "../schemas/productSchema";
import { mapProduct } from "../mappers/productMapper";

export const createProductService = (api: AxiosInstance) => ({
    getProducts: async (): Promise<ProductType[]> => {
        const res = await api.post<ApiProductType[]>("/Product/GetAllByComplexQuery", {
            includeLocations: true
        }, {
            headers: { "Content-Type": "application/json" }
        });

        const parsed = ApiProductsSchema.safeParse(res.data);

        if (!parsed.success) {
            throw `Invalid product data: ${parsed.error}`;
        }

        return parsed.data.map(p => mapProduct(p));
    }
})