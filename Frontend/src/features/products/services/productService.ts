import type { AxiosInstance } from "axios";
import type { ProductType } from "../types/productType";

export const createProductService = (api: AxiosInstance) => ({
    getProducts: async () => {
        const res = await api.get<ProductType[]>("/api/Product/GetAll");
        return res.data;
    }
})