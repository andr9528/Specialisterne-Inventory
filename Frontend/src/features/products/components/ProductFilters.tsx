import { useState } from "react";
import type { SortItemType } from "../../../shared/types/SelectTypes";
import { Search } from "lucide-react";
import Button from "../../../shared/components/ui/Button";
import { textKeys } from "../../../app/constants/textKeys";
import Input from "../../../shared/components/ui/Input";
import Select from "../../../shared/components/ui/Select";
import useCategories from "../hooks/useCategories";
import useWarehouses from "../hooks/useWarehouses";


const ProductFilter = () => {
    const { categories } = useCategories();
    const { warehouses } = useWarehouses();

    const defaultItemValue = "ALL";

    const [searchQuery, setSearchQuery] = useState<string>("");
    const [categoryValue, setCategoryValue] = useState<string>(defaultItemValue);
    const [warehouseValue, setWarehouseValue] = useState<string>(defaultItemValue);
    const [sortValue, setSortValue] = useState<string>("name");
    const [statusFilter, setStatusFilter] = useState<string>(defaultItemValue);

    const sortItems: SortItemType = [
        { value: "name", text: "Navn (A-Å)" },
        { value: "price-asc", text: "Pris (lav-høj)" },
        { value: "price-desc", text: "Pris (høj-lav)" },
        { value: "quantity-asc", text: "Antal (lav-høj)" },
        { value: "quantity-desc", text: "Antal (høj-lav)" },
    ]

    const filterStatusStyle = (statusFilterValue: string, buttonValue: string): string => {
        if (statusFilterValue !== buttonValue) return "";

        switch (statusFilterValue) {
            case defaultItemValue:
                return "bg-blue-500! text-white!";
            case "IN_STOCK":
                return "bg-green-500! text-white!";
            case "LOW_STOCK":
                return "bg-amber-500! text-white!";
            case "OUT_OF_STOCK":
                return "bg-red-500! text-white!";
            default:
                return "";
        }
    }

    return (
        <div className="my-5 bg-gray-50 p-3 w-full border border-gray-300 rounded-lg overflow-hidden">
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-4">
                <div className="lg:col-span-2">
                    <div className="relative">
                        <Search className="absolute left-3 top-1/2 -translate-y-1/2 size-4 text-gray-400" />
                        <Input
                            type="search"
                            placeholder="Søg efter produkt eller SKU..."
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                        />
                    </div>
                </div>

                <Select id="categories" selectValue={categoryValue} setSelectValue={setCategoryValue} items={categories} defaultItem="Alle kategorier" defaultItemValue={defaultItemValue} />

                <Select id="warehouses" selectValue={warehouseValue} setSelectValue={setWarehouseValue} items={warehouses} defaultItem="Alle Lagre" defaultItemValue={defaultItemValue} />

                <Select id="sort" selectValue={sortValue} setSelectValue={setSortValue} items={sortItems} />
            </div>

            <div className="flex flex-wrap mt-7 gap-2">
                <Button variant="tag" className={filterStatusStyle(statusFilter, defaultItemValue)} onClick={() => setStatusFilter(defaultItemValue)}>{textKeys.ALL}</Button>
                <Button variant="tag" className={filterStatusStyle(statusFilter, "IN_STOCK")} onClick={() => setStatusFilter("IN_STOCK")}>{textKeys.IN_STOCK}</Button>
                <Button variant="tag" className={filterStatusStyle(statusFilter, "LOW_STOCK")} onClick={() => setStatusFilter("LOW_STOCK")}>{textKeys.LOW_STOCK}</Button>
                <Button variant="tag" className={filterStatusStyle(statusFilter, "OUT_OF_STOCK")} onClick={() => setStatusFilter("OUT_OF_STOCK")}>{textKeys.OUT_OF_STOCK}</Button>
            </div>
        </div>
    )
}

export default ProductFilter;