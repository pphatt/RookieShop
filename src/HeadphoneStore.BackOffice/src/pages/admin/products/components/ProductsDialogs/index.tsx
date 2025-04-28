import { ProductsActionDialog } from "@/pages/admin/products/components/ProductsActionDialog"
import { useProducts } from "@/pages/admin/products/context/products-context"
import { ProductsDeleteDialog } from "@/pages/admin/products/components/ProductsDeleteDialog"
import { TCategory } from "@/@types/category.type"
import { TBrand } from "@/@types/brand.type"

interface ProductsDialogsProps {
  categories: TCategory[]
  brands: TBrand[]
  refetch: () => any
}

export function ProductsDialogs({
  categories,
  brands,
  refetch,
}: ProductsDialogsProps) {
  const { open, setOpen, currentRow, setCurrentRow } = useProducts()

  return (
    <>
      <ProductsActionDialog
        key="product-add"
        open={open === "add"}
        onOpenChange={() => setOpen("add")}
        allBrands={brands}
        allCategories={categories}
        refetch={refetch}
      />

      {currentRow && (
        <>
          <ProductsActionDialog
            key={`product-edit-${currentRow.id}`}
            open={open === "edit"}
            onOpenChange={() => {
              setOpen("edit")
            }}
            currentRow={currentRow}
            allBrands={brands}
            allCategories={categories}
            refetch={() => {
              refetch()
              setCurrentRow(null)
            }}
          />

          <ProductsDeleteDialog
            key={`product-delete-${currentRow.id}`}
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
