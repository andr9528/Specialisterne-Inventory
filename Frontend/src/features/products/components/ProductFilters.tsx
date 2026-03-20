import { type ChangeEvent, type Dispatch, type SetStateAction } from "react";
import type { ObjectItemType } from "../../../shared/types/SelectTypes";
import { Search } from "lucide-react";
import Button from "../../../shared/components/ui/Button";
import { textKeys } from "../../../app/constants/textKeys";
import Input from "../../../shared/components/ui/Input";
import Select from "../../../shared/components/ui/Select";
import useCategories from "../hooks/useCategories";
import useWarehouses from "../hooks/useWarehouses";
import type { ProductFilterOptionsType } from "../types/productType";
import { DEFAULT_ITEM_VALUE } from "../../../app/constants/filterDefaultValue";

type ProductFilterType = {
    filterOptions: Required<ProductFilterOptionsType>,
    setFilterOptions: Dispatch<SetStateAction<Required<ProductFilterOptionsType>>>;
}

const ProductFilter = ({ filterOptions, setFilterOptions }: ProductFilterType) => {
    const { categories } = useCategories();
    const { warehouses } = useWarehouses();

    const handleOnChange = (e: ChangeEvent<HTMLInputElement>) => {
        setFilterOptions(prev => ({ ...prev, [e.target.id]: e.target.value }))
    }

    const handleSelectChange = (value: string, name: string) => {
        setFilterOptions(prev => ({ ...prev, [name]: value }))
    }

    const handleSetStatusValue = (statusValue: string) => {
        setFilterOptions(prev => ({ ...prev, status: statusValue }))
    }

    const sortItems: ObjectItemType = [
        { value: "name", text: "Navn (A-Å)" },
        { value: "price-asc", text: "Pris (lav-høj)" },
        { value: "price-desc", text: "Pris (høj-lav)" },
        { value: "quantity-asc", text: "Antal (lav-høj)" },
        { value: "quantity-desc", text: "Antal (høj-lav)" },
    ]

    const filterStatusStyle = (statusFilterValue: string, buttonValue: string): string => {
        if (statusFilterValue !== buttonValue) return "";

        switch (statusFilterValue) {
            case DEFAULT_ITEM_VALUE:
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
                            id="searchQuery"
                            type="search"
                            placeholder="Søg efter produkt..."
                            value={filterOptions.searchQuery}
                            onChange={(e) => handleOnChange(e)}
                        />
                    </div>
                </div>

                <Select id="category" selectValue={filterOptions.category} setSelectValue={handleSelectChange} items={categories} defaultItem="Alle kategorier" defaultItemValue={DEFAULT_ITEM_VALUE} />

                <Select id="warehouse" selectValue={filterOptions.warehouse} setSelectValue={handleSelectChange} items={warehouses.map(warehouse => warehouse.name)} defaultItem="Alle Lagre" defaultItemValue={DEFAULT_ITEM_VALUE} />

                <Select id="sort" selectValue={filterOptions.sort} setSelectValue={handleSelectChange} items={sortItems} />
            </div>

            <div className="flex flex-wrap mt-7 gap-2">
                <Button variant="tag" className={filterStatusStyle(filterOptions.status, DEFAULT_ITEM_VALUE)} onClick={() => handleSetStatusValue(DEFAULT_ITEM_VALUE)}>{textKeys.ALL}</Button>
                <Button variant="tag" className={filterStatusStyle(filterOptions.status, "IN_STOCK")} onClick={() => handleSetStatusValue("IN_STOCK")}>{textKeys.IN_STOCK}</Button>
                <Button variant="tag" className={filterStatusStyle(filterOptions.status, "LOW_STOCK")} onClick={() => handleSetStatusValue("LOW_STOCK")}>{textKeys.LOW_STOCK}</Button>
                <Button variant="tag" className={filterStatusStyle(filterOptions.status, "OUT_OF_STOCK")} onClick={() => handleSetStatusValue("OUT_OF_STOCK")}>{textKeys.OUT_OF_STOCK}</Button>
            </div>
        </div>
    )
}

export default ProductFilter;