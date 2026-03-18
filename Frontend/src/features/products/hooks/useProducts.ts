import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useAxiosContext } from "../../../app/context/axiosContext";
import { createProductService } from "../services";

const useProducts = () => {
    const { authAxios } = useAxiosContext();
    const productService = createProductService(authAxios);

    const queryClient = useQueryClient();

    const { data: products = [], isLoading, error } = useQuery({
        queryKey: ["products"],
        queryFn: () => productService.getProducts()
    })
    console.log(error)

    const addProductMutation = useMutation({
        mutationFn: () => productService.addProduct(),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ["products"] }),
        onError: (err) => console.error(err)
    })

    const editProductMutation = useMutation({
        mutationFn: () => productService.editProduct(),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ["products"] }),
        onError: (err) => console.error(err)
    })

    const deleteProductMutation = useMutation({
        mutationFn: ({ id }: { id: number }) => productService.deleteProduct(id),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ["products"] }),
        onError: (err) => console.error(err)
    })

    return {
        products,
        isLoading,
        addProductMutation,
        editProductMutation,
        deleteProductMutation
    }
}

export default useProducts;