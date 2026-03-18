import type { ApiProductType, ProductType } from "../types/productType";

export const mapProduct = (api: ApiProductType): ProductType => ({
    name: api.name,
    sku: api.id.toString(),
    category: api.category.name,
    totalStock: api.totalQuantity,
    price: api.price,
    inventoryStatus: api.status,
    warehouses: api.locations.map(location => ({
        name: location.name ?? "Name missing", //TO-DO Fix optional locaiton name
        inventoryStatus: location.status,
        stock: location.quantity
    }))
});