import z from "zod";

export const CategorySchema = z.string();

export const ApiCategorySchema = z.object({ name: CategorySchema });

export const ApiCategoriesSchema = z.array(ApiCategorySchema);