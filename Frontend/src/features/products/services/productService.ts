import type { AxiosInstance } from "axios";
import type { ApiProductType, ProductFilterOptionsType, ProductType } from "../types/productType";
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
    },
    getProductsByfilter: async (filter: ProductFilterOptionsType): Promise<ProductType[]> => {
        const res = await api.post<ApiProductType[]>("/Product/GetAllByComplexQuery", {
            includeLocations: true,
            ...(filter.searchQuery && { productNameContains: filter.searchQuery }),
            ...(filter.category && { categoryNameContains: filter.category }),
            ...(filter.warehouse && { locationNameContains: filter.warehouse }),
            ...(filter.status && { hasInventoryStatus: filter.status }),
        }, {
            headers: { "Content-Type": "application/json" }
        });

        const parsed = ApiProductsSchema.safeParse(res.data);

        if (!parsed.success) {
            throw `Invalid product data: ${parsed.error}`;
        }

        return parsed.data.map(p => mapProduct(p));
    },
    addProduct: async () => {

    },
    editProduct: async () => {

    },
    deleteProduct: async (id: number) => {
        const res = await api.delete(`/Product/DeleteById/id?id=${id}`);

        if (res.status !== 200) {
            throw `Product deletion failed with error: ${res.statusText}`;
        }
    }
})