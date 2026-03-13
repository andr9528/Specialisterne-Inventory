import PageWrapper from "../../../shared/components/PageWrapper";
import ProductTable from "./_components/ProductTable";

const ProductsPage = () => {

    return (
        <PageWrapper>
            <h2 className="my-5">Products</h2>
            <ProductTable />
        </PageWrapper>
    )
}

export default ProductsPage;