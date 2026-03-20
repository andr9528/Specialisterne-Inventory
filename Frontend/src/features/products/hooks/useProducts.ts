import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useAxiosContext } from "../../../app/context/axiosContext";
import { createProductService } from "../services";
import type { AddProductType, ProductFilterOptionsType } from "../types/productType";
import { mapFilter } from "../mappers/productMapper";

const useProducts = (filters?: ProductFilterOptionsType) => {
    const { authAxios } = useAxiosContext();
    const productService = createProductService(authAxios);

    const queryClient = useQueryClient();

    const { data: products = [], isLoading } = useQuery({
        queryKey: ["products", filters],
        queryFn: () => filters ? productService.getProductsByfilter(mapFilter(filters)) : productService.getProducts()
    })

    const addProductMutation = useMutation({
        mutationFn: (product: AddProductType) => productService.addProduct(product),
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