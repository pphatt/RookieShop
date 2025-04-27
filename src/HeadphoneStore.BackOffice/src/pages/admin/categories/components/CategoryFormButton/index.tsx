import { Button } from "@/components/ui/button"
import { PlusCircle } from "lucide-react"
import { useCategories } from "@/pages/admin/categories/context/categories-context"

export function CategoryFormButton() {
  const { setOpen } = useCategories()

  return (
    <div className="flex gap-2">
      <Button className="space-x-1" onClick={() => setOpen("add")}>
        <span>Add Category</span>

        <PlusCircle size={18} />
      </Button>
    </div>
  )
}
