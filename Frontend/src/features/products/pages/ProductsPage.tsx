import { Plus } from "lucide-react";
import Button from "../../../shared/components/ui/Button";
import PageWrapper from "../../../shared/components/PageWrapper";
import { textKeys } from "../../../shared/constants/textKeys";
import useProducts from "../hooks/useProducts";
import ProductFilter from "./_components/ProductFilters";
import ProductTable from "./_components/ProductTable";
import Modal from "../../../shared/components/Modal";
import { useState } from "react";
import type { InventoryStatusType, WarehouseType } from "../types/productType";
import Input from "../../../shared/components/ui/Input";

const ProductsPage = () => {
    const { products } = useProducts();

    const [modalIsOpen, setModalIsOpen] = useState<boolean>(false);

    const [formData, setFormData] = useState({
        name: "",
        sku: "",
        category: "",
        price: 0,
        warehouses: [] as WarehouseType[],
        inventoryStatus: "IN_STOCK" as InventoryStatusType["IN_STOCK"],
    });

    const handleSubmit = () => {

    }

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

            <Modal isOpen={modalIsOpen} onClose={() => setModalIsOpen(false)}>
                <div>
                    <h3>{textKeys.ADD_NEW_PRODUCT}</h3>

                    <div>
                        <form onSubmit={handleSubmit}>
                            <div className="space-y-4 py-4">
                                <div className="space-y-2 flex flex-col">
                                    <label htmlFor="name">Produktnavn *</label>
                                    <Input
                                        id="name"
                                        type="text"
                                        placeholder="test"
                                        value={formData.name}
                                        onChange={(value) => setFormData({ ...formData, name: value.target.value })}
                                    />
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </Modal>
        </PageWrapper>
    )
}

export default ProductsPage;