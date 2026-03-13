import type z from "zod";
import type { InventoryStatusSchema, ProductSchema } from "../schemas/productSchema";

export const InventoryStatus: Record<z.infer<typeof InventoryStatusSchema>, string> = {
  IN_STOCK: "In Stock",
  LOW_STOCK: "Low Stock",
  OUT_OF_STOCK: "Out of Stock",
} as const;

export type ProductType = z.infer<typeof ProductSchema>;