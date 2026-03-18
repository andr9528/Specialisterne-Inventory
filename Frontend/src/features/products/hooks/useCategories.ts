import { useQuery } from "@tanstack/react-query";
import { useAxiosContext } from "../../../app/context/axiosContext";
import { createCategoryService } from "../services/categoryService";

const useCategories = () => {
    const { authAxios } = useAxiosContext();
    const categoryService = createCategoryService(authAxios);

    const { data: categories = [], isLoading } = useQuery({
        queryKey: ["categories"],
        queryFn: () => categoryService.getCategories()
    })

    return {
        categories,
        isLoading
    }
}

export default useCategories;