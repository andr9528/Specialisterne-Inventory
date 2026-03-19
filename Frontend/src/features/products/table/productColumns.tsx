import type { ProductType } from "../types/productType";

export type Column<T> = {
    key: keyof T | "action";
    label: string;
    textFloat?: "left" | "right";
    width?: string;
    render?: (row: T) => React.ReactNode;
}


export const productColumns: Column<ProductType>[] = [
    { key: "name", label: "Produkt", width: "20%" },
    { key: "sku", label: "Sku", width: "10%", },
    { key: "category", label: "Kategori", width: "10%", },
    { key: "totalStock", label: "Total Stock", width: "10%", },
    { key: "price", label: "Pris", width: "10%", },
    { key: "inventoryStatus", label: "Status", width: "20%", },
    { key: "action", label: "Handlinger", textFloat: "right", width: "10%" }
]