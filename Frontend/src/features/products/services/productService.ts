import type { AxiosInstance } from "axios";
import type { AddProductType, ApiProductType, ProductFilterOptionsType, ProductType } from "../types/productType";
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
    addProduct: async (product: AddProductType) => {
        console.log({
            name: product.name,
            price: product.price,
            locations: product.warehouses.map(warehouse => ({
                locationId: warehouse.id,
                targetQuantity: warehouse.stock,
                quantity: warehouse.stock
            })),
            category: {
                name: product.category
            }
        })
        const res = await api.post<ApiProductType[]>("/Product/CreateProduct", {
            name: product.name,
            price: product.price,
            locations: product.warehouses.map(warehouse => ({
                locationId: warehouse.id,
                targetQuantity: warehouse.stock,
                quantity: warehouse.stock
            })),
            category: {
                name: product.category
            }
        }, {
            headers: { "Content-Type": "application/json" }
        });

        if (res.status !== 200) {
            throw `Add product failed: ${res.statusText}`
        }
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