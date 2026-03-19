import type { ApiProductType, ProductType } from "../types/productType";

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