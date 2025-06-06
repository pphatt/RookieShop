import { Button } from "@/components/ui/button"
import { PlusCircle } from "lucide-react"
import { useBrands } from "@/pages/admin/brands/context/brands-context"

export function BrandFormButton() {
  const { setOpen } = useBrands()

  return (
    <div className="flex gap-2">
      <Button className="space-x-1" onClick={() => setOpen("add")}>
        <span>Add Brand</span>

        <PlusCircle size={18} />
      </Button>
    </div>
  )
}
