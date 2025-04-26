import { Button } from "@/components/ui/button"
import { UserPlus } from "lucide-react"
import { useBrands } from "@/pages/admin/brands/context/brands-context"

export function BrandFormButton() {
  const { setOpen } = useBrands()

  return (
    <div className="flex gap-2">
      <Button className="space-x-1" onClick={() => setOpen("add")}>
        <span>Add Brand</span>

        <UserPlus size={18} />
      </Button>
    </div>
  )
}
