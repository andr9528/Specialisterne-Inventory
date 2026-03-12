
export const InventoryStatus = {
  IN_STOCK: "In Stock",
  LOW_STOCK: "Low Stock",
  OUT_OF_STOCK: "Out of Stock",
} as const;

export type WarehouseType = {
    name: string;
    stock: number;
    inventoryStatus: keyof typeof InventoryStatus;
}

export type ProductType = {
    name: string;
    sku: string;
    category: string;
    totalStock: number;
    price: number;
    inventoryStatus: keyof typeof InventoryStatus;
    warehouses: WarehouseType[];
}