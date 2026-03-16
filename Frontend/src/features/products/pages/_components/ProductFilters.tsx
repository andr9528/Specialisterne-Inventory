import { useState } from "react";
import FilterSelect from "../../../../shared/components/FilterSelect";


const ProductFilter = () => {
    const [categoryValue, setCategoryValue] = useState<string>("all");
    const [warehouseValue, setWarehouseValue] = useState<string>("all");

    return (
        <div className="bg-gray-50 p-3 w-full border border-gray-300 rounded-lg overflow-hidden grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-4">
            <div className="lg:col-span-2">
                <input type="search" />
            </div>
            <div className="">
                <FilterSelect selectValue={categoryValue} setSelectValue={setCategoryValue} items={["Elektronik", "Møbler", "Audio"]} selectDefaultItemText="Alle kategorier" />
            </div>
            <div className="">
                <FilterSelect selectValue={warehouseValue} setSelectValue={setWarehouseValue} items={["Lager A", "Lager B", "Lager C"]} selectDefaultItemText="Alle Lagre" />
            </div>
            <div className="">
                <FilterSelect selectValue={categoryValue} setSelectValue={setCategoryValue} items={["Elektronik", "Møbler", "Audio"]} selectDefaultItemText="testing" />
            </div>
        </div>
    )
}

export default ProductFilter;