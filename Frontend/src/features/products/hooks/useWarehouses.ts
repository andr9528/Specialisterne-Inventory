import { useQuery } from "@tanstack/react-query";
import { useAxiosContext } from "../../../app/context/axiosContext";
import { createWarehouseService } from "../services/warehouseService";

const useWarehouses = () => {
    const { authAxios } = useAxiosContext();
    const warehouseService = createWarehouseService(authAxios);

    const { data: warehouses = [], isLoading } = useQuery({
        queryKey: ["warehouses"],
        queryFn: () => warehouseService.getWarehouses()
    })

    return {
        warehouses,
        isLoading
    }
}

export default useWarehouses;