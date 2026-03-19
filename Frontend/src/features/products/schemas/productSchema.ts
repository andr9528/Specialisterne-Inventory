import z from "zod";

export const InventoryStatusSchema = z.enum(["IN_STOCK", "LOW_STOCK", "OUT_OF_STOCK"]);

export const ProductWarehouseSchema = z.object({
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
    warehouses: z.array(ProductWarehouseSchema)
});

export const ApiProductSchema = z.object({
  Id: z.number(),
  Name: z.string(),
  Price: z.number(),
  Status: InventoryStatusSchema,
  TotalQuantity: z.number(),
  Locations: z.array(
    z.object({
      Quantity: z.number(),
      Status: InventoryStatusSchema,
      Location: z.object({
        Name: z.string()
      })
    })
  ),
  Category: z.object({
    Name: z.string(),
  }),
});

export const ApiProductsSchema = z.array(ApiProductSchema);