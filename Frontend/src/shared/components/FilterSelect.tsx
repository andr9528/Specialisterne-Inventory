import { Select, SelectContent, SelectIcon, SelectItem, SelectTriger, SelectValue, SelectViewport } from "./RadixSelect";
import { ChevronDownIcon } from "@radix-ui/react-icons";

type FilterSelectType = {
    selectPlaceholder?: string;
    selectDefaultItemText: string;
    items: string[] | {
        value: string;
        text: string;
    }[];
    selectValue: string;
    setSelectValue: (value: string) => void;
}

const FilterSelect = ({ items, selectPlaceholder, selectDefaultItemText, selectValue, setSelectValue }: FilterSelectType) => {

    const isObjectItem = (item: any): item is { value: string; text: string } => {
        return item && typeof item === "object" && "value" in item && "text" in item;
    }

    return (
        <Select value={selectValue} onValueChange={setSelectValue}>
            <SelectTriger className="rounded-lg border border-gray-300 outline-none focus-visible:ring-[1px] focus-visible:border-ring focus-visible:ring-ring p-2 flex flex-row justify-between w-full items-center bg-input-background font-bold text-sm ">
                <SelectValue placeholder={selectPlaceholder} />
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
                    <SelectItem value="all">{selectDefaultItemText}</SelectItem>
                    {items.map(item => {
                        if (isObjectItem(item)) {
                            return <SelectItem value={item.value} key={item.value}>{item.text}</SelectItem>
                        } else {
                            return <SelectItem value={item} key={item}>{item}</SelectItem>
                        }

                    })}
                </SelectViewport>
            </SelectContent>
        </Select>
    )
}

export default FilterSelect;
/* 
border-input data-[placeholder]:text-muted-foreground [&_svg:not([class*='text-'])]:text-muted-foreground focus-visible:border-ring focus-visible:ring-ring/50 aria-invalid:ring-destructive/20 dark:aria-invalid:ring-destructive/40 aria-invalid:border-destructive dark:bg-input/30 dark:hover:bg-input/50 flex w-full items-center justify-between gap-2 rounded-md border bg-input-background px-3 py-2 text-sm whitespace-nowrap transition-[color,box-shadow] outline-none focus-visible:ring-[3px] disabled:cursor-not-allowed disabled:opacity-50 data-[size=default]:h-9 data-[size=sm]:h-8 *:data-[slot=select-value]:line-clamp-1 *:data-[slot=select-value]:flex *:data-[slot=select-value]:items-center *:data-[slot=select-value]:gap-2 [&_svg]:pointer-events-none [&_svg]:shrink-0 [&_svg:not([class*='size-'])]:size-4 */