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

export const ApiProductSchema = z.object({
  id: z.number(),
  name: z.string(),
  price: z.number(),
  status: InventoryStatusSchema,
  totalQuantity: z.number(),
  locations: z.array(
    z.object({
      quantity: z.number(),
      status: InventoryStatusSchema,
      name: z.string().optional() //TO-DO fix optional
    })
  ),
  category: z.object({
    name: z.string(),
  }),
});

export const ApiProductsSchema = z.array(ApiProductSchema);