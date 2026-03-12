import type { ProductType } from "../types/productType";

const useProducts = () => {
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