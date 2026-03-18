import type z from "zod";
import type { ApiCategorySchema, CategorySchema } from "../schemas/categorySchema";

export type CategoryType = z.infer<typeof CategorySchema>;

export type ApiCategoryType = z.infer<typeof ApiCategorySchema>;