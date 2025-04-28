import { Button } from "@/components/ui/button"
import { PlusCircle } from "lucide-react"
import { useProducts } from "@/pages/admin/products/context/products-context"

export function ProductFormButton() {
  const { setOpen } = useProducts()

  return (
    <div className="flex gap-2">
      <Button className="space-x-1" onClick={() => setOpen("add")}>
        <span>Add Product</span>

        <PlusCircle size={18} />
      </Button>
    </div>
  )
}
