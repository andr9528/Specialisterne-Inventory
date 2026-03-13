import { useQuery } from "@tanstack/react-query";
import { useAxiosContext } from "../../../app/context/axiosContext";
import { createProductService } from "../services";
import type { ProductType } from "../types/productType";

const useProducts = () => {
    const { authAxios } = useAxiosContext();
    const productService = createProductService(authAxios);

    const { data } = useQuery({
        queryKey: ["products"],
        queryFn: () => productService.getProducts()
    })

    console.log(data);

    const products: ProductType[] = [{
        name: "test",
        category: "test",
        inventoryStatus: "IN_STOCK",
        price: 500,
        sku: "test500",
        totalStock: 45,
        warehouses: [{ name: "A", inventoryStatus: "IN_STOCK", stock: 30 }, { name: "B", inventoryStatus: "LOW_STOCK", stock: 10 }, { name: "B", inventoryStatus: "LOW_STOCK", stock: 10 }, { name: "B", inventoryStatus: "LOW_STOCK", stock: 10 }, { name: "B", inventoryStatus: "OUT_OF_STOCK", stock: 0 }]
    }, {
        name: "test",
        category: "test",
        inventoryStatus: "IN_STOCK",
        price: 500,
        sku: "test501",
        totalStock: 40,
        warehouses: [{ name: "A", inventoryStatus: "IN_STOCK", stock: 30 }, { name: "B", inventoryStatus: "LOW_STOCK", stock: 10 }]
    }];


    return {
        products
    }
}

export default useProducts;