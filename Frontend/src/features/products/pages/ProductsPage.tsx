import { Plus } from "lucide-react";
import Button from "../../../shared/components/ui/Button";
import PageWrapper from "../../../shared/components/PageWrapper";
import { textKeys } from "../../../app/constants/textKeys";
import useProducts from "../hooks/useProducts";
import ProductFilter from "../components/ProductFilters";
import ProductTable from "../components/ProductTable";
import ProductModal from "../components/ProductModal";
import { useState } from "react";
import Loader from "../../../shared/components/Loader";
import type { ProductFilterOptionsType } from "../types/productType";
import { DEFAULT_ITEM_VALUE } from "../../../app/constants/filterDefaultValue";

const ProductsPage = () => {
    const [modalIsOpen, setModalIsOpen] = useState<boolean>(false);

    const [filterOptions, setFilterOptions] = useState<Required<ProductFilterOptionsType>>({
        searchQuery: "",
        category: DEFAULT_ITEM_VALUE,
        warehouse: DEFAULT_ITEM_VALUE,
        sort: "name",
        status: DEFAULT_ITEM_VALUE
    });

    const { products, isLoading, deleteProductMutation } = useProducts(filterOptions);

    return (
        <PageWrapper>
            <div className="flex justify-between ">
                <div className="flex flex-col">
                    <h2 className="mt-5 text-[20px] md:text-2xl! transition-all duration-500">Produkter</h2>
                    <p className="text-sm md:text-[16px] transition-all duration-500">{products.length} produkter fundet</p>
                </div>
                <div className="mt-auto">
                    <Button variant="primary" onClick={() => setModalIsOpen(true)} icon={Plus} customIconStyle="size-4 mt-1 mr-3">{textKeys.ADD_PRODUCT}</Button>
                </div>
            </div>

            <ProductFilter filterOptions={filterOptions} setFilterOptions={setFilterOptions} />

            {isLoading ? (
                <Loader />
            ) : (
                <ProductTable products={products} deleteProductMutation={deleteProductMutation} />
            )}

            <ProductModal modalIsOpen={modalIsOpen} setModalIsOpen={setModalIsOpen} />
        </PageWrapper>
    )
}

export default ProductsPage;