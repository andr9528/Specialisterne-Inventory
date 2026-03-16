import PageWrapper from "../../../shared/components/PageWrapper";
import useProducts from "../hooks/useProducts";
import ProductFilter from "./_components/ProductFilters";
import ProductTable from "./_components/ProductTable";

const ProductsPage = () => {
    const { products } = useProducts();

    return (
        <PageWrapper>
            <div>
                <h2 className="mt-5">Products</h2>
                <p className="text-[16px]">{products.length} produkter fundet</p>
            </div>
            <ProductFilter />

            <ProductTable />
        </PageWrapper>
    )
}

export default ProductsPage;