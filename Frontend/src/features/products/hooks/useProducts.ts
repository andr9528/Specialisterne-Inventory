import { useQuery } from "@tanstack/react-query";
import { useAxiosContext } from "../../../app/context/axiosContext";
import { createProductService } from "../services";

const useProducts = () => {
    const { authAxios } = useAxiosContext();
    const productService = createProductService(authAxios);

    const { data: products = [], isLoading } = useQuery({
        queryKey: ["products"],
        queryFn: () => productService.getProducts()
    })

    return {
        products,
        isLoading
    }
}

export default useProducts;