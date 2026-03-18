import z from "zod";

export const WarehouseSchema = z.string();

export const ApiWarehouseSchema = z.object({ name: WarehouseSchema });

export const ApiWarehousesSchema = z.array(ApiWarehouseSchema);