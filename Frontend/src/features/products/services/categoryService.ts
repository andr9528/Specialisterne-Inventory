import type { AxiosInstance } from "axios";
import type { ApiCategoryType, CategoryType } from "../types/categoryType";
import { ApiCategoriesSchema } from "../schemas/categorySchema";
import { mapCategory } from "../mappers/categoryMapper";

export const createCategoryService = (api: AxiosInstance) => ({
    getCategories: async (): Promise<CategoryType[]> => {
        const res = await api.get<ApiCategoryType>("/Category/GetAll");

        const parsed = ApiCategoriesSchema.safeParse(res.data);

        if (!parsed.success) {
            throw `Invalid product data: ${parsed.error}`;
        }

        return parsed.data.map(c => mapCategory(c));
    }
})