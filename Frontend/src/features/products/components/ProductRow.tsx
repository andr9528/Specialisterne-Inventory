import { Check, ChevronDown, ChevronRight } from "lucide-react";
import type { Column } from "../table/productColumns";
import { InventoryStatus, type ProductType } from "../types/productType";
import { textKeys } from "../../../app/constants/textKeys";
import { useState, type ReactNode } from "react";

type ProdutcRowType = {
    product: ProductType;
    productColumns: Column<ProductType>[];
    showBorder: boolean;
}

const ProductRow = ({ product, productColumns, showBorder }: ProdutcRowType) => {

    const [isExpanded, setIsExpanded] = useState<boolean>(false);

    const borderVisiable = showBorder && !isExpanded;

    const handleExpandClick = () => {
        if (product.warehouses.length === 0) return;

        setIsExpanded(prev => !prev);
    }

    return (
        <>
            <tr className="bg-gray-50 hover:cursor-pointer hover:bg-gray-100" onClick={handleExpandClick}>
                {/* Adds a chevron icon */}
                <td className={`${borderVisiable ? "border-b border-gray-200" : ""}`}>
                    {product.warehouses.length > 0 && (
                        isExpanded ? (
                            <ChevronDown className="mx-auto" />
                        ) : (
                            <ChevronRight className="mx-auto" />
                        )
                    )}
                </td>

                {productColumns.map(col => {
                    const textBold: string = col.key === "name" ? "font-bold" : "";

                    if (col.key === "totalStock") {
                        return (
                            <td key={col.key} className={`px-4 py-2 ${borderVisiable ? "border-b border-gray-200" : ""}`}>
                                <div className="flex flex-col">
                                    <span className="font-bold">{product.totalStock} stk</span>
                                    <span className="text-sm">{product.warehouses.length + " " + textKeys.WAREHOUSES}</span>
                                </div>
                            </td>
                        )
                    }

                    if (col.key === "price") {
                        return (
                            <td key={col.key} className={`px-4 py-2 ${borderVisiable ? "border-b border-gray-200" : ""}`}>{product[col.key]} kr.</td>
                        )
                    }

                    return (
                        <td key={col.key} className={`px-4 py-2 ${borderVisiable ? "border-b border-gray-200" : ""} ${textBold}`}>
                            {col.render ? col.render(product) : product[col.key as keyof Omit<ProductType, "warehouses">]}
                        </td>
                    )
                })}
            </tr>

            {product.warehouses.length > 0 && (
                <tr>
                    <td colSpan={8} className="p-0">
                        <div className={`transition-all duration-500 overflow-hidden ${isExpanded ? "max-h-125 opacity-100" : "max-h-0 opacity-0"}`}>
                            <h4 className="mx-12 my-3">{textKeys.WAREHOUSES_DISTRIBUTION}</h4>
                            <div className="mx-12 flex flex-wrap gap-5 mt-2 mb-4">
                                {product.warehouses.map((warehouse, index) => {
                                    warehouse.inventoryStatus
                                    const statusColors: Record<keyof typeof InventoryStatus, string> = {
                                        IN_STOCK: "bg-green-50 border-green-200",
                                        LOW_STOCK: "bg-amber-50 border-amber-200",
                                        OUT_OF_STOCK: "bg-red-50 border-red-200"
                                    };

                                    const statusDotColors: Record<keyof typeof InventoryStatus, string> = {
                                        IN_STOCK: "bg-green-500",
                                        LOW_STOCK: "bg-amber-500",
                                        OUT_OF_STOCK: "bg-red-500"
                                    };

                                    const stockStatus: Record<keyof typeof InventoryStatus, ReactNode> = {
                                        IN_STOCK: <span className="flex gap-1"><Check className="h-4 w-4" />På lager</span>,
                                        LOW_STOCK: <span>⚠️ Lav beholdning</span>,
                                        OUT_OF_STOCK: <span>⚠️ Udsolgt</span>
                                    };


                                    return (
                                        <div key={warehouse.name + index} className={`flex flex-col gap-2 w-[30%] p-4 border-2 rounded-lg ${statusColors[warehouse.inventoryStatus]}`}>
                                            <div className="flex flex-row">
                                                <div className="flex flex-row gap-2">
                                                    <div className={`mt-2 w-2 h-2 rounded-full ${statusDotColors[warehouse.inventoryStatus]}`}></div>
                                                    <h5>{warehouse.name}</h5>
                                                </div>
                                                <span className="ml-auto text-sm">{((warehouse.stock / product.totalStock) * 100).toFixed()} %</span>
                                            </div>

                                            <div className="flex flex-row justify-between">
                                                <span>{textKeys.WAREHOUSES_AMOUNT}</span>
                                                <span className="text-lg text-black font-black">{warehouse.stock} stk</span>
                                            </div>
                                            <div className="h-2 bg-gray-200 rounded-full overflow-hidden">
                                                <div className={`h-full transition-all ${statusDotColors[warehouse.inventoryStatus]}`}
                                                    style={{ width: `${Math.min((warehouse.stock / product.totalStock) * 100, 100)}%` }}
                                                ></div>
                                            </div>

                                            <div className="text-xs text-gray-500 mt-2">
                                                {stockStatus[warehouse.inventoryStatus]}
                                            </div>
                                        </div>
                                    )
                                })}
                            </div>
                        </div>
                    </td>
                </tr>
            )}
        </>
    )
}

export default ProductRow;