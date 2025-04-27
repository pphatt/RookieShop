import { CategoriesActionDialog } from "@/pages/admin/categories/components/CategoriesActionDialog"
import { useCategories } from "@/pages/admin/categories/context/categories-context"
import { CategoriesDeleteDialog } from "@/pages/admin/categories/components/CategoriesDeleteDialog"
import { TCategory } from "@/@types/category.type"

interface BrandsDialogsProps {
  allFirstLevelCategories?: TCategory[]
  refetch: () => any
}

export function CategoriesDialogs({
  allFirstLevelCategories,
  refetch,
}: BrandsDialogsProps) {
  const { open, setOpen, currentRow, setCurrentRow } = useCategories()

  return (
    <>
      <CategoriesActionDialog
        key="category-add"
        open={open === "add"}
        onOpenChange={() => setOpen("add")}
        refetch={refetch}
        allFirstLevelCategories={allFirstLevelCategories}
      />

      {currentRow && (
        <>
          <CategoriesActionDialog
            key={`category-edit-${currentRow.id}`}
            open={open === "edit"}
            onOpenChange={() => {
              setOpen("edit")
            }}
            currentRow={currentRow}
            refetch={() => {
              refetch()
              setCurrentRow(null)
            }}
            allFirstLevelCategories={allFirstLevelCategories}
          />

          <CategoriesDeleteDialog
            key={`category-delete-${currentRow.id}`}
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
