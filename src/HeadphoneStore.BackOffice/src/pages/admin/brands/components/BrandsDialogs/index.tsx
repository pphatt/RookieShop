import { BrandsActionDialog } from "@/pages/admin/brands/components/BrandsActionDialog"
import { useBrands } from "@/pages/admin/brands/context/brands-context"
import { BrandsDeleteDialog } from "@/pages/admin/brands/components/BrandsDeleteDialog"

export function BrandsDialogs() {
  const { open, setOpen, currentRow, setCurrentRow } = useBrands()

  return (
    <>
      <BrandsActionDialog
        key="brand-add"
        open={open === "add"}
        onOpenChange={() => setOpen("add")}
      />

      {currentRow && (
        <>
          <BrandsActionDialog
            key={`brand-edit-${currentRow.id}`}
            open={open === "edit"}
            onOpenChange={() => {
              setOpen("edit")
              setTimeout(() => {
                setCurrentRow(null)
              }, 500)
            }}
            currentRow={currentRow}
          />

          <BrandsDeleteDialog
            key={`brand-delete-${currentRow.id}`}
            open={open === "delete"}
            onOpenChange={() => {
              setOpen("delete")
              setTimeout(() => {
                setCurrentRow(null)
              }, 500)
            }}
            currentRow={currentRow}
          />
        </>
      )}
    </>
  )
}
