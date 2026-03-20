import { Edit, Trash2 } from "lucide-react";
import Button from "../../../shared/components/ui/Button";

type ActionButtonsType<T> = {
  excludeEdit?: boolean;
  row: T;
  onEdit?: (row: T) => void;
  onDelete?: (row: T) => void;
}

const ActionButtons = <T,>({ row, onDelete, onEdit, excludeEdit = false }: ActionButtonsType<T>) => {

  return (
    <div className="flex flex-row justify-center gap-2">
      {!excludeEdit && (
        <Button
          variant="ghost"
          className="text-primary"
          onClick={(e) => {
            e.stopPropagation();
            onEdit?.(row)
          }}><Edit className="size-4" /></Button>
      )}

      <Button
        variant="ghost"
        className="text-destructive"
        onClick={(e) => {
          e.stopPropagation();
          onDelete?.(row)
        }}><Trash2 className="size-4 text-red-600" /></Button>
    </div>
  )
};

export default ActionButtons;