import type z from "zod";
import type { ApiProductSchema, InventoryStatusSchema, ProductSchema, ProductWarehouseSchema } from "../schemas/productSchema";

export const InventoryStatus: Record<z.infer<typeof InventoryStatusSchema>, string> = {
  IN_STOCK: "In Stock",
  LOW_STOCK: "Low Stock",
  OUT_OF_STOCK: "Out of Stock",
} as const;

export type InventoryStatusType = typeof InventoryStatus;

export type ProductType = z.infer<typeof ProductSchema>;

export type ProductWarehouseType = z.infer<typeof ProductWarehouseSchema>;

export type ApiProductType = z.infer<typeof ApiProductSchema>;

export type ProductFilterOptionsType = {
  searchQuery?: string;
  category?: string;
  warehouse?: string;
  sort?: string;
  status?: string;
}