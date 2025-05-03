import { Button } from "@/components/ui/button"
import { PlusCircle } from "lucide-react"
import { useUsers } from "@/pages/admin/users/context/users-context"

export function UserFormButton() {
  const { setOpen } = useUsers()

  return (
    <div className="flex gap-2">
      <Button className="space-x-1" onClick={() => setOpen("add")}>
        <span>Add User</span>

        <PlusCircle size={18} />
      </Button>
    </div>
  )
}
