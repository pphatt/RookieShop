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
import { TBrand, TBrandAdd } from "@/@types/brand.type"
import { Textarea } from "@/components/ui/textarea"
import { useMutation } from "@tanstack/react-query"
import { AddNewBrand } from "@/services/brand.service"
import { toast } from "react-toastify"
import IconSpinner from "@/components/icon-spinner"

const schema = z.object({
  name: z.string().min(1, { message: "Last Name is required." }),
  description: z.string().min(1, { message: "Username is required." }),
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
  description: string
}

export function BrandsActionDialog({
  currentRow,
  refetch,
  open,
  onOpenChange,
}: BrandsActionDialogProps) {
  const isEdit = !!currentRow

  // const form = useForm<BrandForm>({
  //   resolver: zodResolver(schema),
  // })

  // const onSubmit = (values: BrandForm) => {
  //   form.reset()
  //   onOpenChange(false)
  // }

  const defaultValues: TDefaultValue = {
    name: "",
    description: "",
  }

  const form = useForm({
    mode: "onChange",
    defaultValues,
    resolver: zodResolver(schema),
  })

  const onSubmit = (data: TDefaultValue) => {
    createBrandMutation.mutate(data, {
      onSuccess: () => {
        toast.success("Add new brand successfully.")
        refetch()
        handleResetDialog()
      },
      onError: (error: any) => {
        if (error?.response?.data?.title) {
          let errorMessage = error.response.data.title

          if (error?.response?.data?.description) {
            errorMessage += ` - ${error.response.data.description}`
          }

          toast.error(errorMessage)
        } else if (error?.response?.data?.Error?.Message) {
          toast.error(error.response.data.Error.Message)
        } else if (error?.response?.data?.message) {
          toast.error(error.response.data.message)
        } else {
          toast.error("Something went wrong")
        }
      },
    })
  }

  const createBrandMutation = useMutation({
    mutationFn: (body: TBrandAdd) => AddNewBrand(body),
  })

  const handleResetDialog = () => {
    onOpenChange(false)
    form.reset(defaultValues)
  }

  return (
    <Dialog
      open={open}
      onOpenChange={(state) => {
        form.reset(defaultValues)
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
            disabled={createBrandMutation.isLoading}
          >
            {createBrandMutation.isLoading && <IconSpinner />}
            Save changes
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
