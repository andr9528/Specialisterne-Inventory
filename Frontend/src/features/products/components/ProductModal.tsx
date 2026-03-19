import { useEffect, useState, type ChangeEvent } from "react";
import Modal from "../../../shared/components/Modal";
import type { AddProductType, ProductWarehouseType } from "../types/productType";
import { textKeys } from "../../../app/constants/textKeys";
import Input from "../../../shared/components/ui/Input";
import Select from "../../../shared/components/ui/Select";
import Button from "../../../shared/components/ui/Button";
import { Plus } from "lucide-react";
import ActionButtons from "./ActionButtons";
import useCategories from "../hooks/useCategories";
import useWarehouses from "../hooks/useWarehouses";
import useProducts from "../hooks/useProducts";

type ProductModalType = {
    modalIsOpen: boolean;
    setModalIsOpen: (value: boolean) => void;
}



const ProductModal = ({ modalIsOpen, setModalIsOpen }: ProductModalType) => {
    const { warehouses } = useWarehouses();
    const { categories } = useCategories();
    const { addProductMutation } = useProducts();

    const initialFormData: AddProductType = {
        name: "",
        category: categories[0],
        price: 0,
        warehouses: [],
    }

    const [formData, setFormData] = useState(initialFormData);

    useEffect(() => {
        if (categories.length === 0) return;

        // Only set it if it's not already set
        setFormData(prev => {
            if (prev.category) return prev;

            return { ...prev, category: categories[0] }
        })
    }, [categories]);

    const totalQuantity = formData.warehouses.reduce((prev, current) => prev + Number(current.stock), 0);

    const handleSubmit = () => {
        addProductMutation.mutate(formData);

        setFormData(initialFormData);
        setModalIsOpen(false);
    }

    const handleCancel = () => {
        setFormData(initialFormData);
        setModalIsOpen(false);
    }

    const handleOnChange = (e: ChangeEvent<HTMLInputElement>) => {
        setFormData(prev => ({ ...prev, [e.target.id]: e.target.value }))
    }

    const handleSelectChange = (value: string, name: string) => {
        setFormData(prev => ({ ...prev, [name]: value }))
    }

    const handleWarehouseChange = (index: number, field: string, value: string, id?: number) => {
        setFormData((prev) => {
            const newWarehouses = [...prev.warehouses];
            newWarehouses[index] = { ...newWarehouses[index], [field]: value, ...(id && { id }), };
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
            warehouses: [...prev.warehouses, { id: warehouses[0].id, name: warehouses[0].name, stock: 0 }],
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
                                        items={categories}
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
                                <Button variant="outline-slim" onClick={handleAddWarehouse} icon={Plus} customIconStyle="size-4 mt-1 mr-3">{textKeys.ADD_WAREHOUSE}</Button>
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
                                                    setSelectValue={(value, _, itemId) => handleWarehouseChange(index, "name", value, itemId)}
                                                    items={warehouses.map(warehouse => ({ id: warehouse.id, text: warehouse.name, value: warehouse.name }))}
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
                                })}

                                <div className="p-3 bg-blue-50 rounded-md">
                                    <div className="flex justify-between items-center">
                                        <span className="text-sm font-medium text-blue-900">Total {textKeys.WAREHOUSES_AMOUNT}:</span>
                                        <span className="text-lg font-semibold text-blue-900">{totalQuantity} stk</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div className="flex flex-row justify-end gap-3 mt-auto">
                            <Button variant="outline" onClick={handleCancel}>{textKeys.CANCEL}</Button>
                            <Button variant="primary" onClick={handleSubmit}>{textKeys.ADD_PRODUCT}</Button>
                        </div>
                    </form>
                </div>
            </div>
        </Modal>
    )
}

export default ProductModal;