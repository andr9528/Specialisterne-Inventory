import { useState } from "react";
import Modal from "../../../../shared/components/Modal";
import type { InventoryStatusType, WarehouseType } from "../../types/productType";
import { textKeys } from "../../../../shared/constants/textKeys";
import Input from "../../../../shared/components/ui/Input";
import Select from "../../../../shared/components/ui/Select";
import Button from "../../../../shared/components/ui/Button";
import { Plus } from "lucide-react";

type ProductModalType = {
    modalIsOpen: boolean;
    setModalIsOpen: (value: boolean) => void;
}

const ProductModal = ({ modalIsOpen, setModalIsOpen }: ProductModalType) => {
    const warehouses = ["Lager A", "Lager B", "Lager C"];

    const [formData, setFormData] = useState({
        name: "",
        sku: "",
        category: "Elektronik",
        price: 0,
        warehouses: [] as WarehouseType[],
        inventoryStatus: "IN_STOCK" as InventoryStatusType["IN_STOCK"],
    });

    const handleSubmit = () => {

    }

    const handleAddWarehouse = () => {
        setFormData({
            ...formData,
            warehouses: [...formData.warehouses, { name: warehouses[0], stock: 0, inventoryStatus: "OUT_OF_STOCK" }],
        });
    };
    return (
        <Modal isOpen={modalIsOpen} onClose={() => setModalIsOpen(false)}>
            <div>
                <h3>{textKeys.ADD_NEW_PRODUCT}</h3>

                <div>
                    <form onSubmit={(e) => e.preventDefault()}>
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
                                        id="category"
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
                                <Button variant="outline" onClick={handleAddWarehouse} icon={Plus}>{textKeys.ADD_WAREHOUSE}</Button> {/* TO-DO finish modal */}
                            </div>

                            {formData.warehouses.length === 0 && (
                                <div className="p-4 bg-gray-50 rounded-md text-center text-sm text-gray-500">
                                    Ingen lagerplaceringer tilføjet. Klik "Tilføj Lager" for at starte.
                                </div>
                            )}

                            <div className="space-y-3">
                                {formData.warehouses.map((warehouse, index) => {
                                    return (
                                        <div key={warehouse.name + index}>
                                            <label htmlFor="warehouse">Lagre</label>
                                            {/*TO-DO finish add project
                                             <Select
                                                selectValue={formData.warehouses[0].name}
                                                items={warehouses}
                                            /> */} 
                                        </div>
                                    )
                                })

                                }
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </Modal>
    )
}

export default ProductModal;