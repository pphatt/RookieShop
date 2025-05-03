"use client"

import { useState } from "react"
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { ConfirmDialog } from "@/components/form/confirm-dialog"
import { TriangleAlert } from "lucide-react"
import { toast } from "react-toastify"
import { handleError } from "@/utils"
import { useMutation } from "@tanstack/react-query"
import { TUser, TUserDelete } from "@/@types/user.type"
import { DeleteUser } from "@/services/user.service"

interface Props {
  open: boolean
  onOpenChange: (open: boolean) => void
  currentRow: TUser
  refetch: () => void
}

export function UsersDeleteDialog({
  open,
  onOpenChange,
  currentRow,
  refetch,
}: Props) {
  const [value, setValue] = useState("")

  const handleDelete = () => {
    if (value.trim() !== currentRow.firstName + currentRow.lastName) return

    onOpenChange(false)

    deleteUserMutation.mutate(
      {
        id: currentRow.id,
      },
      {
        onSuccess: () => {
          toast.success("Delete user successfully.")
          refetch()
        },
        onError: (error: any) => {
          handleError(error)
        },
      }
    )
  }

  const deleteUserMutation = useMutation({
    mutationFn: (body: TUserDelete) => DeleteUser(body),
  })

  return (
    <ConfirmDialog
      open={open}
      onOpenChange={onOpenChange}
      handleConfirm={handleDelete}
      disabled={value.trim() !== currentRow.firstName + currentRow.lastName}
      isLoading={deleteUserMutation.isLoading}
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
            <span className="font-bold">
              {currentRow.firstName + currentRow.lastName}
            </span>
            ?
            <br />
            This action will permanently remove the user from the system. This
            cannot be undone.
          </p>

          <Label className="my-2">
            User:
            <Input
              value={value}
              onChange={(e) => setValue(e.target.value)}
              placeholder="Enter user to confirm deletion."
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
