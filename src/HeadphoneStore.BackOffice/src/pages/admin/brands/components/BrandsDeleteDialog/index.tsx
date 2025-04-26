"use client"

import { useState } from "react"
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { ConfirmDialog } from "@/components/form/confirm-dialog"
import { TriangleAlert } from "lucide-react"
import { TBrand } from "@/@types/brand.type"

interface Props {
  open: boolean
  onOpenChange: (open: boolean) => void
  currentRow: TBrand
}

export function BrandsDeleteDialog({ open, onOpenChange, currentRow }: Props) {
  const [value, setValue] = useState("")

  const handleDelete = () => {
    if (value.trim() !== currentRow.name) return

    onOpenChange(false)
  }

  return (
    <ConfirmDialog
      open={open}
      onOpenChange={onOpenChange}
      handleConfirm={handleDelete}
      disabled={value.trim() !== currentRow.name}
      title={
        <span className="text-destructive">
          <TriangleAlert
            className="stroke-destructive mr-1 inline-block"
            size={18}
          />
          Delete User
        </span>
      }
      desc={
        <div className="space-y-4">
          <p className="mb-2">
            Are you sure you want to delete{" "}
            <span className="font-bold">{currentRow.name}</span>?
            <br />
            This action will permanently remove the brand from the system. This
            cannot be undone.
          </p>

          <Label className="my-2">
            Brand:
            <Input
              value={value}
              onChange={(e) => setValue(e.target.value)}
              placeholder="Enter brand to confirm deletion."
            />
          </Label>

          <Alert variant="destructive">
            <AlertTitle>Warning!</AlertTitle>
            <AlertDescription>
              Please be carefull, this operation can not be rolled back.
            </AlertDescription>
          </Alert>
        </div>
      }
      confirmText="Delete"
      destructive
    />
  )
}
