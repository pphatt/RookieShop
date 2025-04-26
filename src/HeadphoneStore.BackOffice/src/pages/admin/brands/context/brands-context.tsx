import React, { useState } from "react"
import useDialogState from "@/hooks/use-dialog-state"
import { TBrand } from "@/@types/brand.type"

type BrandsDialogType = "add" | "edit" | "delete"

interface BrandsContextType {
  open: BrandsDialogType | null
  setOpen: (str: BrandsDialogType | null) => void
  currentRow: TBrand | null
  setCurrentRow: React.Dispatch<React.SetStateAction<TBrand | null>>
}

const BrandsContext = React.createContext<BrandsContextType | null>(null)

interface Props {
  children: React.ReactNode
}

export default function BrandsProvider({ children }: Props) {
  const [open, setOpen] = useDialogState<BrandsDialogType>(null)
  const [currentRow, setCurrentRow] = useState<TBrand | null>(null)

  return (
    <BrandsContext value={{ open, setOpen, currentRow, setCurrentRow }}>
      {children}
    </BrandsContext>
  )
}

// eslint-disable-next-line react-refresh/only-export-components
export const useBrands = () => {
  const brandsContext = React.useContext(BrandsContext)

  if (!brandsContext) {
    throw new Error("useBrands has to be used within <BrandsContext>")
  }

  return brandsContext
}
