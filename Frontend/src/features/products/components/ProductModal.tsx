import { useState, type ChangeEvent } from "react";
import Modal from "../../../shared/components/Modal";
import type { WarehouseType } from "../types/productType";
import { textKeys } from "../../../app/constants/textKeys";
import Input from "../../../shared/components/ui/Input";
import Select from "../../../shared/components/ui/Select";
import Button from "../../../shared/components/ui/Button";
import { Plus } from "lucide-react";
import ActionButtons from "./ActionButtons";

type ProductModalType = {
    modalIsOpen: boolean;
    setModalIsOpen: (value: boolean) => void;
}

const ProductModal = ({ modalIsOpen, setModalIsOpen }: ProductModalType) => {
    const warehouses = ["Lager A", "Lager B", "Lager C"];

    const [formData, setFormData] = useState({
        name: "",
        category: "Elektronik",
        price: 0,
        warehouses: [] as Omit<WarehouseType, "inventoryStatus">[],
    });

    const handleSubmit = () => {

    }

    const handleOnChange = (e: ChangeEvent<HTMLInputElement>) => {
        setFormData(prev => ({ ...prev, [e.target.id]: e.target.value }))
    }

    const handleSelectChange = (value: string, name: string) => {
        setFormData(prev => ({ ...prev, [name]: value }))
    }

    const handleWarehouseChange = (index: number, field: string, value: string) => {
        setFormData((prev) => {
            const newWarehouses = [...prev.warehouses];
            newWarehouses[index] = { ...newWarehouses[index], [field]: value };
            return { ...prev, warehouses: newWarehouses };
        });
    }

    const handleRemoveWarehouse = (id: number) => {
        setFormData(prev => ({
            ...prev,
            warehouses: [...prev.warehouses.filter((_, index) => index !== id)],
        }));
    }

    const handleAddWarehouse = () => {
        setFormData(prev => ({
            ...prev,
            warehouses: [...prev.warehouses, { name: warehouses[0], stock: 0 }],
        }));
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
                                    onChange={handleOnChange}
                                />
                            </div>

                            <div className="grid grid-cols-2 gap-4">
                                <div className="space-y-2 flex flex-col">
                                    <label htmlFor="category">Kategori</label>
                                    <Select
                                        id="category"
                                        selectValue={formData.category}
                                        setSelectValue={handleSelectChange}
                                        items={["Elektronik", "Møbler", "Audio"]}
                                    />
                                </div>

                                <div className="space-y-2 flex flex-col">
                                    <label htmlFor="price">Pris *</label>
                                    <Input
                                        id="price"
                                        type="number"
                                        placeholder="0"
                                        value={formData.price}
                                        onChange={handleOnChange}
                                    />
                                </div>
                            </div>

                            <div className="space-y-2 flex flex-row justify-between">
                                <label htmlFor="price">Lagerplacering *</label>
                                <Button variant="outline" onClick={handleAddWarehouse} icon={Plus}>{textKeys.ADD_WAREHOUSE}</Button>
                            </div>

                            {formData.warehouses.length === 0 && (
                                <div className="p-4 bg-gray-50 rounded-md text-center text-sm text-gray-500">
                                    Ingen lagerplaceringer tilføjet. Klik "Tilføj Lager" for at starte.
                                </div>
                            )}

                            <div className="space-y-3">
                                {formData.warehouses.map((warehouse, index) => {
                                    return (
                                        <div className="grid grid-cols-7 gap-4" key={warehouse.name + index}>
                                            <div className="space-y-2 col-span-3 flex flex-col">
                                                <label htmlFor="warehouse">Lager</label>
                                                <Select
                                                    id="warehouses"
                                                    selectValue={warehouse.name}
                                                    setSelectValue={(value) => handleWarehouseChange(index, "name", value)}
                                                    items={warehouses}
                                                />
                                            </div>

                                            <div className="space-y-2 col-span-3 flex flex-col">
                                                <label htmlFor="stock">Antal</label>
                                                <Input
                                                    id="stock"
                                                    type="number"
                                                    placeholder="0"
                                                    value={formData.warehouses[index].stock}
                                                    onChange={(e) => handleWarehouseChange(index, "stock", e.target.value)}
                                                />
                                            </div>

                                            <div className="space-y-2 mt-auto mb-2">
                                                <ActionButtons row={index} onDelete={handleRemoveWarehouse} excludeEdit={true} />
                                            </div>
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