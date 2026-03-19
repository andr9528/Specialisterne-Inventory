import type { AxiosInstance } from "axios";
import { ApiWarehousesSchema } from "../schemas/warehouseSchema";
import type { ApiWarehouseType, WarehouseType } from "../types/warehouseType";
import { mapWarehouse } from "../mappers/warehouseMapper";

export const createWarehouseService = (api: AxiosInstance) => ({
    getWarehouses: async (): Promise<WarehouseType[]> => {
        const res = await api.get<ApiWarehouseType>("/Location/GetAll");

        const parsed = ApiWarehousesSchema.safeParse(res.data);

        if (!parsed.success) {
            throw `Invalid product data: ${parsed.error}`;
        }

        return parsed.data.map(c => mapWarehouse(c));
    }
})