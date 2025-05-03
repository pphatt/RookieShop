"use client"

import { z } from "zod"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { TRole, TUser, TUserAdd, TUserUpdate } from "@/@types/user.type"
import { Textarea } from "@/components/ui/textarea"
import { useMutation } from "@tanstack/react-query"
import { AddNewUser, UpdateUser } from "@/services/user.service"
import { toast } from "react-toastify"
import IconSpinner from "@/components/icon-spinner"
import { handleError } from "@/utils"
import { SelectDropdown } from "@/components/select-dropdown"
import * as React from "react"

const schema = z.object({
  email: z.string().email(),
  firstName: z.string().min(1, { message: "First Name is required." }),
  lastName: z.string().min(1, { message: "Last Name is required." }),
  phoneNumber: z.string(),
  roleId: z.string(),
  status: z.string(),
})

type UserForm = z.infer<typeof schema>

interface UsersActionDialogProps {
  allRoles: TRole[]
  currentRow?: TUser
  refetch: () => any
  open: boolean
  onOpenChange: (open: boolean) => void
}

type TDefaultValue = {
  email: string
  firstName: string
  lastName: string
  phoneNumber: string
  roleId: string
  status: string
}

export function UsersActionDialog({
  allRoles,
  currentRow,
  refetch,
  open,
  onOpenChange,
}: UsersActionDialogProps) {
  const isEdit = !!currentRow

  const defaultValues: TDefaultValue = {
    email: "",
    firstName: "",
    lastName: "",
    phoneNumber: "",
    roleId: "",
    status: "Active",
  }

  const form = useForm({
    defaultValues: isEdit
      ? {
          email: currentRow.email,
          firstName: currentRow?.firstName,
          lastName: currentRow?.lastName,
          phoneNumber: currentRow?.phoneNumber,
          roleId: currentRow?.roles.id,
          status: currentRow.status,
        }
      : defaultValues,
    resolver: zodResolver(schema),
  })

  const onSubmit = (data: TDefaultValue) => {
    if (isEdit) {
      updateUserMutation.mutate(
        {
          id: currentRow?.id,
          email: data.email,
          firstName: data.firstName,
          lastName: data.lastName,
          phoneNumber: data.phoneNumber,
          roleId: data.roleId,
          status: data.status,
        },
        {
          onSuccess: () => {
            toast.success("Update user successfully.")
            refetch()
            handleResetDialog()
          },
          onError: (error: any) => {
            handleError(error)
          },
        }
      )

      return
    }

    createUserMutation.mutate(data, {
      onSuccess: () => {
        toast.success("Add new user successfully.")
        refetch()
        handleResetDialog()
      },
      onError: (error: any) => {
        handleError(error)
      },
    })
  }

  const createUserMutation = useMutation({
    mutationFn: (body: TUserAdd) => AddNewUser(body),
  })

  const updateUserMutation = useMutation({
    mutationFn: (body: TUserUpdate) => UpdateUser(body),
  })

  const handleResetDialog = () => {
    onOpenChange(false)
    form.reset(defaultValues)
  }

  return (
    <Dialog
      open={open}
      onOpenChange={(state) => {
        form.reset()
        onOpenChange(state)
      }}
    >
      <DialogContent className="sm:max-w-lg">
        <DialogHeader className="text-left">
          <DialogTitle>{isEdit ? "Edit User" : "Add New User"}</DialogTitle>

          <DialogDescription>
            {isEdit ? "Update the user here. " : "Create new user here. "}
            Click save when you&apos;re done.
          </DialogDescription>
        </DialogHeader>

        <div className="-mr-4 w-full overflow-y-auto py-1">
          <Form {...form}>
            <form
              id="user-form"
              onSubmit={form.handleSubmit(onSubmit)}
              className="space-y-4 p-0.5"
            >
              <FormField
                control={form.control}
                name="email"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      Email
                    </FormLabel>
                    <FormControl>
                      <Input
                        placeholder="Email"
                        className="col-span-4"
                        autoComplete="off"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="firstName"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      First Name
                    </FormLabel>
                    <FormControl>
                      <Input
                        placeholder="First Name"
                        className="col-span-4"
                        autoComplete="off"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="lastName"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      Last Name
                    </FormLabel>
                    <FormControl>
                      <Input
                        placeholder="Last Name"
                        className="col-span-4"
                        autoComplete="off"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="phoneNumber"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      Phone Number
                    </FormLabel>
                    <FormControl>
                      <Input
                        placeholder="Phone Number"
                        className="col-span-4"
                        autoComplete="off"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="roleId"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      Role
                    </FormLabel>
                    <SelectDropdown
                      defaultValue={field.value}
                      onValueChange={field.onChange}
                      placeholder="Select a role"
                      className="col-span-4"
                      items={allRoles.map(({ id, displayName }) => ({
                        value: id,
                        label: displayName,
                      }))}
                    />
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="status"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      Status
                    </FormLabel>
                    <SelectDropdown
                      defaultValue={field.value}
                      onValueChange={field.onChange}
                      placeholder="Select status"
                      className="col-span-4"
                      items={[
                        { value: "Active", label: "Active" },
                        { value: "Inactive", label: "Inactive" },
                      ]}
                    />
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />
            </form>
          </Form>
        </div>

        <DialogFooter>
          <Button
            type="submit"
            form="user-form"
            disabled={
              createUserMutation.isLoading || updateUserMutation.isLoading
            }
          >
            {(createUserMutation.isLoading || updateUserMutation.isLoading) && (
              <IconSpinner />
            )}
            Save changes
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
