import type { ApiWarehouseType, WarehouseType } from "../types/warehouseType";

export const mapWarehouse = (api: ApiWarehouseType): WarehouseType => api.name;