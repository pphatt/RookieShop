"use client"

import * as React from "react"
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

// Import FilePond styles
import "filepond/dist/filepond.min.css"

import FilePondPluginFileValidateType from "filepond-plugin-file-validate-type"
// Import the Image EXIF Orientation and Image Preview plugins
// Note: These need to be installed separately
// `npm i filepond-plugin-image-preview filepond-plugin-image-exif-orientation --save`
import FilePondPluginImageExifOrientation from "filepond-plugin-image-exif-orientation"
import FilePondPluginImagePreview from "filepond-plugin-image-preview"

import "filepond-plugin-image-preview/dist/filepond-plugin-image-preview.css"

import {
  ProductStatus,
  TProduct,
  TProductAdd,
  TProductUpdate,
} from "@/@types/product.type"
import { AddNewProduct, UpdateProduct } from "@/services/product.service"
import { SelectDropdown } from "@/components/select-dropdown"
import { TCategory } from "@/@types/category.type"
import { TBrand } from "@/@types/brand.type"
import styles from "@/pages/admin/products/components/ProductsActionDialog/style.module.scss"
import { registerPlugin, type FilePondFile } from "filepond"
import { FilePond } from "react-filepond"
import { useMounted } from "@/hooks/use-mounted"

registerPlugin(
  FilePondPluginImageExifOrientation,
  FilePondPluginImagePreview,
  FilePondPluginFileValidateType
)

const schema = z.object({
  name: z.string().min(1, { message: "Name is required." }),
  slug: z.string(),
  description: z.string().min(1, { message: "Description is required." }),
  stock: z
    .number({ invalid_type_error: "Quantity must be a number." })
    .min(1, { message: "Quantity must be at least 1." }),
  sku: z.string(),
  status: z.string(),
  productStatus: z.enum(["InStock", "OutOfStock", "Discontinued"] as const),
  productPrice: z
    .number({ invalid_type_error: "Price must be a number." })
    .min(0, { message: "Price cannot be negative." }),
  categoryId: z.string().min(1, { message: "Category is required." }),
  brandId: z.string().min(1, { message: "Brand is required." }),
})

type ProductForm = z.infer<typeof schema>

interface ProductsActionDialogProps {
  allCategories: TCategory[]
  allBrands: TBrand[]
  currentRow?: TProduct
  refetch: () => any
  open: boolean
  onOpenChange: (open: boolean) => void
}

export function ProductsActionDialog({
  allCategories,
  allBrands,
  currentRow,
  refetch,
  open,
  onOpenChange,
}: ProductsActionDialogProps) {
  const isMounted = useMounted()
  const [images, setImages] = React.useState<FilePondFile[]>(
    // eslint-disable-next-line @typescript-eslint/ban-ts-comment
    // @ts-expect-error
    currentRow
      ? currentRow.media.map(({ imageUrl }) => ({
          source: imageUrl,
        }))
      : []
  )

  const isEdit = !!currentRow

  const defaultValues: ProductForm = {
    name: "",
    slug: "",
    description: "",
    stock: 1,
    sku: "",
    status: "Active",
    productStatus: "InStock",
    productPrice: 0,
    categoryId: "",
    brandId: "",
  }

  const mapProductStatusToString = (
    status: ProductStatus
  ): "InStock" | "OutOfStock" | "Discontinued" => {
    let statusMap = status.toString()

    switch (statusMap) {
      case "In stock":
        return "InStock"
      case "Out of stock":
        return "OutOfStock"
      default:
        return "Discontinued"
    }
  }

  const mapStringToProductStatus = (
    status: "InStock" | "OutOfStock" | "Discontinued"
  ): ProductStatus => {
    return ProductStatus[status] as unknown as ProductStatus
  }

  const form = useForm<ProductForm>({
    defaultValues: isEdit
      ? {
          name: currentRow.name,
          slug: currentRow.slug,
          description: currentRow.description,
          stock: currentRow.stock,
          sku: currentRow.sku,
          status: currentRow.status,
          productStatus: mapProductStatusToString(currentRow.productStatus),
          productPrice: currentRow.productPrice,
          categoryId: currentRow?.category.id,
          brandId: currentRow?.brand.id,
        }
      : defaultValues,
    resolver: zodResolver(schema),
  })

  const createProductMutation = useMutation({
    mutationFn: (body: FormData) => AddNewProduct(body),
  })

  const updateProductMutation = useMutation({
    mutationFn: (body: FormData) => UpdateProduct(body),
  })

  const onSubmit = (data: ProductForm) => {
    const productData = {
      ...data,
      status: data.status,
      productPrice: Number(data.productPrice),
      productStatus: data.productStatus,
    }

    const formData = new FormData()
    formData.append("name", productData.name)
    formData.append("description", productData.description)
    formData.append("stock", productData.stock.toString())
    formData.append("slug", productData.slug.toString())
    formData.append("status", productData.status.toString())
    formData.append("productStatus", productData.productStatus.toString())
    formData.append("productPrice", productData.productPrice.toString())
    formData.append("categoryId", productData.categoryId)
    formData.append("brandId", productData.brandId)

    if (isEdit) {
      formData.append("id", currentRow!.id)
      formData.append("sku", currentRow!.sku.toString())

      const oldImagesId = currentRow.media.map(
        ({ imageUrl }) => imageUrl.split("/")[10]
      )

      images.forEach((file, index) => {
        const isOldImage = oldImagesId.includes(file.filename)

        if (isOldImage) {
          const mediaItem = currentRow?.media.find(
            ({ imageUrl }) => imageUrl.split("/")[10] === file.filename
          )

          if (mediaItem?.id) {
            formData.append("oldImages", mediaItem.id)
            formData.append("listOrder", `old-${index + 1}`)
          }
        } else {
          formData.append("newImages", file.file)
          formData.append("listOrder", `new-${index + 1}`)
        }
      })

      updateProductMutation.mutate(formData, {
        onSuccess: () => {
          toast.success("Product updated successfully.")
          refetch()
          handleResetDialog()
        },
        onError: (error: any) => {
          handleError(error)
        },
      })

      return
    }

    images.forEach((file, index) => {
      formData.append(`images`, file.file)
    })

    createProductMutation.mutate(formData, {
      onSuccess: () => {
        toast.success("Product added successfully.")
        refetch()
        handleResetDialog()
      },
      onError: (error: any) => {
        handleError(error)
      },
    })
  }

  const handleResetDialog = () => {
    onOpenChange(false)
    form.reset(defaultValues)
    setImages([])
  }

  React.useEffect(() => {
    if (open && isEdit && currentRow?.media) {
      setImages(
        // eslint-disable-next-line @typescript-eslint/ban-ts-comment
        // @ts-expect-error
        currentRow.media.map(({ imageUrl }) => ({
          source: imageUrl,
        }))
      )
    }
  }, [open, isEdit, currentRow])

  return (
    <Dialog
      open={open}
      onOpenChange={(state) => {
        form.reset()
        onOpenChange(state)
        setImages([])
      }}
    >
      <DialogContent
        className="sm:max-w-lg"
        style={{ maxHeight: "600px", maxWidth: "1025px", overflowY: "scroll" }}
      >
        <DialogHeader className="text-left">
          <DialogTitle>
            {isEdit ? "Edit Product" : "Add New Product"}
          </DialogTitle>
          <DialogDescription>
            {isEdit ? "Update the product here." : "Create a new product here."}{" "}
            Click save when you’re done.
          </DialogDescription>
        </DialogHeader>

        <div className="w-full py-1">
          <Form {...form}>
            <form
              id="product-form"
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
                        placeholder="Product Name"
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
                <>
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
                            placeholder="Product Slug"
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
                    name="sku"
                    render={({ field }) => (
                      <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                        <FormLabel className="col-span-2 text-right">
                          Sku
                        </FormLabel>
                        <FormControl>
                          <Input
                            placeholder="Product Sku"
                            className="col-span-4"
                            autoComplete="off"
                            {...field}
                          />
                        </FormControl>
                        <FormMessage className="col-span-4 col-start-3" />
                      </FormItem>
                    )}
                  />
                </>
              )}

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
                        placeholder="Product description"
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
                name="stock"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      Stock
                    </FormLabel>
                    <FormControl>
                      <Input
                        type="number"
                        placeholder="Stock"
                        className="col-span-4"
                        autoComplete="off"
                        {...field}
                        onChange={(e) => field.onChange(Number(e.target.value))}
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
                name="productStatus"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      Product Status
                    </FormLabel>
                    <SelectDropdown
                      defaultValue={field.value}
                      onValueChange={field.onChange}
                      placeholder="Select status"
                      className="col-span-4"
                      items={[
                        { value: "InStock", label: "In Stock" },
                        { value: "OutOfStock", label: "Out of Stock" },
                        { value: "Discontinued", label: "Discontinued" },
                      ]}
                    />
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="productPrice"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      Price
                    </FormLabel>
                    <FormControl>
                      <Input
                        type="text"
                        placeholder="Price"
                        className="col-span-4"
                        autoComplete="off"
                        {...field}
                        onChange={(e) => field.onChange(Number(e.target.value))}
                      />
                    </FormControl>
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="categoryId"
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
                      items={allCategories.map(({ id, name }) => ({
                        value: id,
                        label: name,
                      }))}
                    />
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="brandId"
                render={({ field }) => (
                  <FormItem className="grid grid-cols-6 items-center space-y-0 gap-x-4 gap-y-1">
                    <FormLabel className="col-span-2 text-right">
                      Brand
                    </FormLabel>
                    <SelectDropdown
                      defaultValue={field.value}
                      onValueChange={field.onChange}
                      placeholder="Select a brand"
                      className="col-span-4"
                      items={allBrands.map(({ id, name }) => ({
                        value: id,
                        label: name,
                      }))}
                    />
                    <FormMessage className="col-span-4 col-start-3" />
                  </FormItem>
                )}
              />

              <div className={styles["form-item-wrapper"]}>
                <span className={styles["form-item-span"]}>Upload image</span>

                <div className={styles["form-item-image-upload-tips"]}>
                  You can upload up to 10 images at the same time (JPG, PNG,
                  JPEG).
                </div>

                <div className={styles["form-item-image-upload-wrapper"]}>
                  <FilePond
                    className={styles["upload-files"]}
                    // @ts-expect-error @ts-ignore
                    files={images}
                    onupdatefiles={setImages}
                    allowMultiple={true}
                    name="images"
                    allowReorder={true}
                    // set allowed file types with image/*
                    acceptedFileTypes={["image/jpeg", "image/jpg", "image/png"]}
                    maxFiles={10}
                    onreorderfiles={setImages}
                    labelIdle='Drag & Drop your image or <span class="filepond--label-action">Browse</span>'
                  />
                </div>
              </div>
            </form>
          </Form>
        </div>

        <DialogFooter>
          <Button
            type="submit"
            form="product-form"
            disabled={
              createProductMutation.isLoading || updateProductMutation.isLoading
            }
          >
            {(createProductMutation.isLoading ||
              updateProductMutation.isLoading) && <IconSpinner />}
            Save changes
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  )
}
