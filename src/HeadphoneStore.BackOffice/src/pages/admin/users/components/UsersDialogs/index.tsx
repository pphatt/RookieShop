import { UsersActionDialog } from "@/pages/admin/users/components/UsersActionDialog"
import { useUsers } from "@/pages/admin/users/context/users-context"
import { UsersDeleteDialog } from "@/pages/admin/users/components/UsersDeleteDialog"
import { TRole } from "@/@types/user.type"

interface UsersDialogsProps {
  roles: TRole[]
  refetch: () => any
}

export function UsersDialogs({ roles, refetch }: UsersDialogsProps) {
  const { open, setOpen, currentRow, setCurrentRow } = useUsers()

  return (
    <>
      <UsersActionDialog
        key="user-add"
        open={open === "add"}
        onOpenChange={() => setOpen("add")}
        allRoles={roles}
        refetch={refetch}
      />

      {currentRow && (
        <>
          <UsersActionDialog
            key={`user-edit-${currentRow.id}`}
            open={open === "edit"}
            onOpenChange={() => {
              setOpen("edit")
            }}
            currentRow={currentRow}
            allRoles={roles}
            refetch={() => {
              refetch()
              setCurrentRow(null)
            }}
          />

          <UsersDeleteDialog
            key={`user-delete-${currentRow.id}`}
            open={open === "delete"}
            onOpenChange={() => {
              setOpen("delete")
            }}
            currentRow={currentRow}
            refetch={() => {
              refetch()
              setCurrentRow(null)
            }}
          />
        </>
      )}
    </>
  )
}
