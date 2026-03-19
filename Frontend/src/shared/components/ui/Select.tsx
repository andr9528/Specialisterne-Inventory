import type { SortItemType } from "../../types/SelectTypes";
import { Select as RadixSelect, SelectContent, SelectIcon, SelectItem, SelectTriger, SelectValue, SelectViewport } from "./RadixSelect";
import { ChevronDownIcon } from "@radix-ui/react-icons";
import { DEFAULT_ITEM_VALUE } from "../../../app/constants/filterDefaultValue";

type SelectType = {
    placeholder?: string;
    defaultItem?: string;
    defaultItemValue?: string;
    items: string[] | SortItemType;
    selectValue: string;
    setSelectValue: (value: string, id: string) => void;
    id: string;
}

const Select = ({ id, items, placeholder, defaultItem, defaultItemValue = DEFAULT_ITEM_VALUE, selectValue, setSelectValue }: SelectType) => {

    const isObjectItem = (item: any): item is SortItemType[number] => {
        return item && typeof item === "object" && "value" in item && "text" in item;
    }

    return (
        <RadixSelect value={selectValue} onValueChange={(value) => setSelectValue(value, id)} name={id}>
            <SelectTriger id={id} className="min-h-8 rounded-lg border border-gray-300 outline-none focus-visible:ring-[1px] focus-visible:border-ring focus-visible:ring-ring py-1 px-2 flex flex-row justify-between w-full items-center bg-input-background font-bold text-sm ">
                <SelectValue placeholder={placeholder} />
                <SelectIcon className="SelectIcon">
                    <ChevronDownIcon />
                </SelectIcon>
            </SelectTriger>

            <SelectContent
                position="popper"
                side="bottom"
                sideOffset={4}
                className="bg-gray-50 border border-gray-300 w-(--radix-select-trigger-width) rounded shadow text-gray-900"
            >
                <SelectViewport>

                    {defaultItem && <SelectItem value={defaultItemValue}>{defaultItem}</SelectItem>}
                    {items.map(item => {
                        if (isObjectItem(item)) {
                            return <SelectItem value={item.value} key={item.value}>{item.text}</SelectItem>
                        } else {
                            return <SelectItem value={item} key={item}>{item}</SelectItem>
                        }
                        
                    })}
                </SelectViewport>
            </SelectContent>
        </RadixSelect>
    )
}

export default Select;