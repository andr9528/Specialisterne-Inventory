import z from "zod";

export const InventoryStatusSchema = z.enum(["IN_STOCK", "LOW_STOCK", "OUT_OF_STOCK"]);

export const WarehouseSchema = z.object({
    name: z.string(),
    stock: z.number(),
    inventoryStatus: InventoryStatusSchema
})

export const ProductSchema = z.object({
    name: z.string(),
    sku: z.string(),
    category: z.string(),
    totalStock: z.number(),
    price: z.number(),
    inventoryStatus: InventoryStatusSchema,
    warehouses: z.array(WarehouseSchema)
});