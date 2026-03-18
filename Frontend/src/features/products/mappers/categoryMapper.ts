import type { ApiCategoryType, CategoryType } from "../types/categoryType";

export const mapCategory = (api: ApiCategoryType): CategoryType => api.name;