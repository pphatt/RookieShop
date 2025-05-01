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
import { TBrand, TBrandAdd, TBrandUpdate } from "@/@types/brand.type"
import { Textarea } from "@/components/ui/textarea"
import { useMutation } from "@tanstack/react-query"
import { AddNewBrand, UpdateBrand } from "@/services/brand.service"
import { toast } from "react-toastify"
import IconSpinner from "@/components/icon-spinner"
import { handleError } from "@/utils"
import { SelectDropdown } from "@/components/select-dropdown"
import * as React from "react"

const schema = z.object({
  name: z.string().min(1, { message: "Last Name is required." }),
  slug: z.string(),
  description: z.string().min(1, { message: "Username is required." }),
  status: z.string(),
})

type BrandForm = z.infer<typeof schema>

interface BrandsActionDialogProps {
  currentRow?: TBrand
  refetch: () => any
  open: boolean
  onOpenChange: (open: boolean) => void
}

type TDefaultValue = {
  name: string
  slug: string
  description: string
  status: string
}

export function BrandsActionDialog({
  currentRow,
  refetch,
  open,
  onOpenChange,
}: BrandsActionDialogProps) {
  const isEdit = !!currentRow

  const defaultValues: TDefaultValue = {
    name: "",
    slug: "",
    description: "",
    status: "Active",
  }

  const form = useForm({
    defaultValues: isEdit ? currentRow : defaultValues,
    resolver: zodResolver(schema),
  })

  const onSubmit = (data: TDefaultValue) => {
    if (isEdit) {
      updateBrandMutation.mutate(
        {
          id: currentRow?.id,
          name: data.name,
          slug: data.slug,
          description: data.description,
          status: data.status,
        },
        {
          onSuccess: () => {
            toast.success("Update brand successfully.")
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

    createBrandMutation.mutate(data, {
      onSuccess: () => {
        toast.success("Add new brand successfully.")
        refetch()
        handleResetDialog()
      },
      onError: (error: any) => {
        handleError(error)
      },
    })
  }

  const createBrandMutation = useMutation({
    mutationFn: (body: TBrandAdd) => AddNewBrand(body),
  })

  const updateBrandMutation = useMutation({
    mutationFn: (body: TBrandUpdate) => UpdateBrand(body),
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
          <DialogTitle>{isEdit ? "Edit Brand" : "Add New Brand"}</DialogTitle>

          <DialogDescription>
            {isEdit ? "Update the brand here. " : "Create new brand here. "}
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
                        placeholder="Sony"
                        className="col-span-4"
                        autoComplete="off"
                        {...field}
                      />
                    </FormControl>
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />

              {isEdit && (
                <FormField
                  control={form.control}
                  name="slug"
                  render={({ field }) => (
                    <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                      <FormLabel className="col-span-2 text-right">
                        Slug
                      </FormLabel>
                      <FormControl>
                        <Input
                          placeholder="sony"
                          className="col-span-4"
                          autoComplete="off"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage className="col-span-4 col-start-3" />
                    </FormItem>
                  )}
                />
              )}

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
                        placeholder="Sony description"
                        className="col-span-4"
                        autoComplete="off"
                        {...field}
                      />
                    </FormControl>
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
            form="brand-form"
            disabled={
              createBrandMutation.isLoading || updateBrandMutation.isLoading
            }
          >
            {(createBrandMutation.isLoading ||
              updateBrandMutation.isLoading) && <IconSpinner />}
            Save changes
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
