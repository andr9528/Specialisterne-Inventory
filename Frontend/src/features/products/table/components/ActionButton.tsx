import { Edit, Trash2 } from "lucide-react";
import type { ProductType } from "../../types/productType";
import Button from "../../../../shared/components/ui/Button";


const ActionButtons = (product: ProductType) => {

  return (
    <div className="flex flex-row justify-center gap-5">
      <Button variant="ghost" className="text-primary"><Edit className="size-4" /></Button>
      <Button variant="ghost" className="text-destructive"><Trash2 className="size-4 text-red-600" /></Button>
    </div>

  )
};

export default ActionButtons;