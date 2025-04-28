import React, { useState } from "react"
import useDialogState from "@/hooks/use-dialog-state"
import { TProduct } from "@/@types/product.type"

type ProductsDialogType = "add" | "edit" | "delete"

interface ProductsContextType {
  open: ProductsDialogType | null
  setOpen: (str: ProductsDialogType | null) => void
  currentRow: TProduct | null
  setCurrentRow: React.Dispatch<React.SetStateAction<TProduct | null>>
}

const ProductsContext = React.createContext<ProductsContextType | null>(null)

interface Props {
  children: React.ReactNode
}

export default function ProductsProvider({ children }: Props) {
  const [open, setOpen] = useDialogState<ProductsDialogType>(null)
  const [currentRow, setCurrentRow] = useState<TProduct | null>(null)

  return (
    <ProductsContext value={{ open, setOpen, currentRow, setCurrentRow }}>
      {children}
    </ProductsContext>
  )
}

// eslint-disable-next-line react-refresh/only-export-components
export const useProducts = () => {
  const productsContext = React.useContext(ProductsContext)

  if (!productsContext) {
    throw new Error("useProducts has to be used within <ProductsContext>")
  }

  return productsContext
}
