import type z from "zod";
import type { ApiWarehouseSchema, WarehouseSchema } from "../schemas/warehouseSchema";

export type WarehouseType = z.infer<typeof WarehouseSchema>;

export type ApiWarehouseType = z.infer<typeof ApiWarehouseSchema>;