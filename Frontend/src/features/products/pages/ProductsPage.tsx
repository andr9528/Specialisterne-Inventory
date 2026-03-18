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

const ProductsPage = () => {
    const { products, isLoading } = useProducts();
    const [modalIsOpen, setModalIsOpen] = useState<boolean>(false);

    return (
        <PageWrapper>
            <div className="flex justify-between">
                <div className="flex flex-col">
                    <h2 className="mt-5">Produkter</h2>
                    <p className="text-[16px]">{products.length} produkter fundet</p>
                </div>
                <div className="mt-auto">
                    <Button variant="primary" onClick={() => setModalIsOpen(true)} icon={Plus}>{textKeys.ADD_PRODUCT}</Button>
                </div>
            </div>
            
            <ProductFilter />

            {isLoading ? (
                <Loader />
            ) : (
                <ProductTable />
            )}

            <ProductModal modalIsOpen={modalIsOpen} setModalIsOpen={setModalIsOpen} />
        </PageWrapper>
    )
}

export default ProductsPage;