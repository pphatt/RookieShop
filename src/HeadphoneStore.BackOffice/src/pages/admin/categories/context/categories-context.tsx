import React, { useState } from "react"
import useDialogState from "@/hooks/use-dialog-state"
import { TCategory } from "@/@types/category.type"

type CategoriesDialogType = "add" | "edit" | "delete"

interface CategoriesContextType {
  open: CategoriesDialogType | null
  setOpen: (str: CategoriesDialogType | null) => void
  currentRow: TCategory | null
  setCurrentRow: React.Dispatch<React.SetStateAction<TCategory | null>>
}

const CategoriesContext = React.createContext<CategoriesContextType | null>(
  null
)

interface Props {
  children: React.ReactNode
}

export default function CategoriesProvider({ children }: Props) {
  const [open, setOpen] = useDialogState<CategoriesDialogType>(null)
  const [currentRow, setCurrentRow] = useState<TCategory | null>(null)

  return (
    <CategoriesContext value={{ open, setOpen, currentRow, setCurrentRow }}>
      {children}
    </CategoriesContext>
  )
}

// eslint-disable-next-line react-refresh/only-export-components
export const useCategories = () => {
  const categoriesContext = React.useContext(CategoriesContext)

  if (!categoriesContext) {
    throw new Error("useCategories has to be used within <CategoriesContext>")
  }

  return categoriesContext
}
