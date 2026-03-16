import PageWrapper from "../../../shared/components/PageWrapper";
import ProductFilter from "./_components/ProductFilters";
import ProductTable from "./_components/ProductTable";

const ProductsPage = () => {

    return (
        <PageWrapper>
            <h2 className="my-5">Products</h2>
            <ProductFilter />
            <ProductTable />
        </PageWrapper>
    )
}

export default ProductsPage;