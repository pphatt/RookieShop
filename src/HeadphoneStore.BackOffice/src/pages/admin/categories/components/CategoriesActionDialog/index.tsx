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
import { Textarea } from "@/components/ui/textarea"
import { useMutation } from "@tanstack/react-query"
import { toast } from "react-toastify"
import IconSpinner from "@/components/icon-spinner"
import { handleError } from "@/utils"
import {
  TCategory,
  TCategoryAdd,
  TCategoryUpdate,
} from "@/@types/category.type"
import { AddNewCategory, UpdateCategory } from "@/services/category.service"
import { SelectDropdown } from "@/components/select-dropdown"
import * as React from "react"

const schema = z.object({
  name: z.string().min(1, { message: "Last Name is required." }),
  description: z.string().min(1, { message: "Username is required." }),
  parentCategoryId: z.string().optional(),
  status: z.string(),
})

type CategoryForm = z.infer<typeof schema>

interface CategoriesActionDialogProps {
  allFirstLevelCategories?: TCategory[]
  currentRow?: TCategory
  refetch: () => any
  open: boolean
  onOpenChange: (open: boolean) => void
}

type TDefaultValue = {
  name: string
  description: string
  parentCategoryId?: string
  status: string
}

export function CategoriesActionDialog({
  allFirstLevelCategories,
  currentRow,
  refetch,
  open,
  onOpenChange,
}: CategoriesActionDialogProps) {
  const isEdit = !!currentRow

  const defaultValues: TDefaultValue = {
    name: "",
    description: "",
    status: "Active",
  }

  const form = useForm({
    defaultValues: isEdit
      ? {
          name: currentRow.name,
          description: currentRow?.description,
          parentCategoryId: currentRow?.parent?.id,
          status: currentRow?.status,
        }
      : defaultValues,
    resolver: zodResolver(schema),
  })

  const onSubmit = (data: TDefaultValue) => {
    if (isEdit) {
      updateCategoryMutation.mutate(
        {
          id: currentRow?.id,
          name: data.name,
          description: data.description,
          parentCategoryId: data.parentCategoryId,
          status: data.status,
        },
        {
          onSuccess: () => {
            toast.success("Update category successfully.")
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

    createCategoryMutation.mutate(data, {
      onSuccess: () => {
        toast.success("Add new category successfully.")
        refetch()
        handleResetDialog()
      },
      onError: (error: any) => {
        handleError(error)
      },
    })
  }

  const createCategoryMutation = useMutation({
    mutationFn: (body: TCategoryAdd) => AddNewCategory(body),
  })

  const updateCategoryMutation = useMutation({
    mutationFn: (body: TCategoryUpdate) => UpdateCategory(body),
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
          <DialogTitle>
            {isEdit ? "Edit Category" : "Add New Category"}
          </DialogTitle>

          <DialogDescription>
            {isEdit
              ? "Update the category here. "
              : "Create new category here. "}
            Click save when you&apos;re done.
          </DialogDescription>
        </DialogHeader>

        <div className="-mr-4 w-full overflow-y-auto py-1">
          <Form {...form}>
            <form
              id="brand-form"
              onSubmit={form.handleSubmit(onSubmit)}
              className="space-y-4 p-0.5"
            >
              <FormField
                control={form.control}
                name="name"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      Name
                    </FormLabel>
                    <FormControl>
                      <Input
                        placeholder="Tai Nghe"
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

              <FormField
                control={form.control}
                name="description"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      Description
                    </FormLabel>
                    <FormControl>
                      <Textarea
                        placeholder="Tai Nghe description"
                        className="col-span-4"
                        autoComplete="off"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />

              {allFirstLevelCategories && (
                <FormField
                  control={form.control}
                  name="parentCategoryId"
                  render={({ field }) => (
                    <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                      <FormLabel className="col-span-2 text-right">
                        Category
                      </FormLabel>

                      <SelectDropdown
                        defaultValue={field.value}
                        onValueChange={field.onChange}
                        placeholder="Select a category"
                        className="col-span-4"
                        items={[
                          {
                            id: null!,
                            name: "No category",
                          },
                          ...allFirstLevelCategories,
                        ].map(({ id, name }) => ({
                          value: id,
                          label: name,
                        }))}
                      />

                      <FormMessage className="col-span-4 col-start-3" />
                    </FormItem>
                  )}
                />
              )}
            </form>
          </Form>
        </div>

        <DialogFooter>
          <Button
            type="submit"
            form="brand-form"
            disabled={
              createCategoryMutation.isLoading ||
              updateCategoryMutation.isLoading
            }
          >
            {(createCategoryMutation.isLoading ||
              updateCategoryMutation.isLoading) && <IconSpinner />}
            Save changes
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
