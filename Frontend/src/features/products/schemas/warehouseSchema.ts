import z from "zod";

export const WarehouseSchema = z.object({
    id: z.number(),
    name: z.string()
});

export const ApiWarehouseSchema = z.object(WarehouseSchema);

export const ApiWarehousesSchema = z.array(WarehouseSchema);