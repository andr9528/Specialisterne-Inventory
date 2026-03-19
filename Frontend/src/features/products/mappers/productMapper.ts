import { DEFAULT_ITEM_VALUE } from "../../../app/constants/filterDefaultValue";
import type { ApiProductType, ProductFilterOptionsType, ProductType } from "../types/productType";

export const mapProduct = (api: ApiProductType): ProductType => ({
    name: api.Name,
    sku: api.Id.toString(),
    category: api.Category.Name,
    totalStock: api.TotalQuantity,
    price: api.Price,
    inventoryStatus: api.Status,
    warehouses: api.Locations.map(location => ({
        name: location.Location.Name ?? "Name missing", //TO-DO Fix optional locaiton name
        inventoryStatus: location.Status,
        stock: location.Quantity
    }))
});

export const mapFilter = (filterOptions: ProductFilterOptionsType): ProductFilterOptionsType => {
    const result: ProductFilterOptionsType = {};
    
    if (filterOptions.status && filterOptions.status !== DEFAULT_ITEM_VALUE) {
        result.status = filterOptions.status;
    }

    if (filterOptions.category && filterOptions.category !== DEFAULT_ITEM_VALUE) {
        result.category = filterOptions.category;
    }

    if (filterOptions.warehouse && filterOptions.warehouse !== DEFAULT_ITEM_VALUE) {
        result.warehouse = filterOptions.warehouse;
    }

    // Add any other fields that should always be included
    // For example, searchQuery or other optional fields
    if (filterOptions.searchQuery) {
        result.searchQuery = filterOptions.searchQuery;
    }

    if (filterOptions.sort) {
        result.sort = filterOptions.sort;
    }
    console.log("result", result)

    return result;
};