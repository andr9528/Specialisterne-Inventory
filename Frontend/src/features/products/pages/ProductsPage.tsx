import { Plus } from "lucide-react";
import Button from "../../../shared/components/ui/Button";
import PageWrapper from "../../../shared/components/PageWrapper";
import { textKeys } from "../../../shared/constants/textKeys";
import useProducts from "../hooks/useProducts";
import ProductFilter from "./_components/ProductFilters";
import ProductTable from "./_components/ProductTable";
import ProductModal from "./_components/ProductModal";
import { useState } from "react";

const ProductsPage = () => {
    const { products } = useProducts();
    const [modalIsOpen, setModalIsOpen] = useState<boolean>(false);

    return (
        <PageWrapper>
            <div className="flex justify-between">
                <div className="flex flex-col">
                    <h2 className="mt-5">Products</h2>
                    <p className="text-[16px]">{products.length} produkter fundet</p>
                </div>
                <div className="mt-auto">
                    <Button variant="primary" onClick={() => setModalIsOpen(true)} icon={Plus}>{textKeys.ADD_PRODUCT}</Button>
                </div>
            </div>
            <ProductFilter />

            <ProductTable />

            <ProductModal modalIsOpen={modalIsOpen} setModalIsOpen={setModalIsOpen} />
        </PageWrapper>
    )
}

export default ProductsPage;