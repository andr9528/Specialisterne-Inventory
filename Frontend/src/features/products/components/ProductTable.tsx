import type { UseMutationResult } from "@tanstack/react-query";
import { productColumns } from "../table/productColumns";
import type { ProductType } from "../types/productType";
import ActionButtons from "./ActionButtons";

import ProductRow from "./ProductRow";

type ProductTableType = {
    products: ProductType[];
    deleteProductMutation: UseMutationResult<void, Error, {
        id: number;
    }, unknown>;
}

const ProductTable = ({ products, deleteProductMutation }: ProductTableType) => {

    const columns = productColumns.map(column => {
        if (column.key === "action") {
            return {
                ...column,
                render: (row: ProductType) => (
                    <ActionButtons
                        row={row}
                        onDelete={(row) => deleteProductMutation.mutate({ id: isNaN(Number(row.sku)) ? 0 : Number(row.sku) })}
                        excludeEdit={false}
                    />
                )
            }
        }

        return column;
    });

    return (
        <table className="w-full border border-gray-300 rounded-lg table-fixed border-separate border-spacing-0 overflow-hidden">
            <thead>
                <tr>
                    {/* Emtpy header col for the chevron icon */}
                    <th className="border-b border-gray-200" style={{ width: "3%" }}></th>

                    {columns.map(col => {
                        const textFloat = col.textFloat ? `text-${col.textFloat}` : "text-left";
                        return (
                            <th key={col.key} className={`border-b border-gray-200 px-4 py-2 ${textFloat}`} style={{ width: col.width }}>
                                {col.label}
                            </th>
                        )
                    })}
                </tr>
            </thead>

            <tbody>
                {products.map((product, index, arr) => {
                    const showBorder: boolean = index !== arr.length - 1;

                    return (
                        <ProductRow product={product} productColumns={columns} showBorder={showBorder} key={product.sku} />
                    )
                })}
            </tbody>
        </table>
    )
}

export default ProductTable;