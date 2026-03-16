import type { AxiosInstance } from "axios";
import type { ProductType } from "../types/productType";
import { ProductSchema } from "../schemas/productSchema";

export const createProductService = (api: AxiosInstance) => ({
    getProducts: async () => {
        const res = await api.get<ProductType[]>("/api/Product/GetAll");
        const parsed = ProductSchema.safeParse(res.data);

        if (!parsed.success) {
            console.error("Invalid product data", parsed.error);
            return null;
        }

        return parsed.data;
    }
})