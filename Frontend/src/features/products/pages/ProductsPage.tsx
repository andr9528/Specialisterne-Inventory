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
import Select from "../../../shared/components/ui/Select";

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
                                        placeholder="Fx Iphone X"
                                        value={formData.name}
                                        onChange={(value) => setFormData({ ...formData, name: value.target.value })}
                                    />
                                </div>

                                <div className="space-y-2 flex flex-col">
                                    <label htmlFor="sku">SKU *</label>
                                    <Input
                                        id="sku"
                                        type="text"
                                        placeholder="Fx TECH-001"
                                        value={formData.name}
                                        onChange={(value) => setFormData({ ...formData, sku: value.target.value })}
                                    />
                                </div>

                                <div className="grid grid-cols-2 gap-4">
                                    <div className="space-y-2 flex flex-col">
                                        <label htmlFor="category">Kategori</label>
                                        <Select
                                            selectValue={formData.category}
                                            setSelectValue={(value) => setFormData({ ...formData, category: value })}
                                            items={["Elektronik", "Møbler", "Audio"]}
                                        />
                                    </div>

                                    <div className="space-y-2 flex flex-col">
                                        <label htmlFor="price">Pris *</label>
                                        <Input
                                            id="price"
                                            type="text"
                                            placeholder="0"
                                            value={formData.price}
                                            onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                                                const value = Number(e.target.value);
                                                setFormData({ ...formData, price: isNaN(value) ? formData.price : value });
                                            }}
                                        />
                                    </div>
                                </div>

                                <div className="space-y-2 flex flex-row justify-between">
                                    <label htmlFor="price">Lagerplacering *</label>
                                    <Button variant="outline" icon={Plus}>{textKeys.ADD_WAREHOUSE}</Button> {/* TO-DO finish modal */}
                                </div>

                                {formData.warehouses.length === 0 && (
                                    <div className="p-4 bg-gray-50 rounded-md text-center text-sm text-gray-500">
                                        Ingen lagerplaceringer tilføjet. Klik "Tilføj Lager" for at starte.
                                    </div>
                                )}
                            </div>
                        </form>
                    </div>
                </div>
            </Modal>
        </PageWrapper>
    )
}

export default ProductsPage;