import { PaginationType } from "@/@types/pagination.type"
import { FilterType } from "@/@types/filter.type"
import { TCategory } from "@/@types/category.type"
import { TBrand } from "@/@types/brand.type"

export enum ProductStatus {
  InStock = 1,
  OutOfStock = 2,
  Discontinued = 3,
}

export type TProductImage = {
  id: string
  imageUrl: string
  publicId: string
  path: string
  name: string
}

export type TProductImageRequest = {
  id: string
  order: number
}

export type TProductImageUpload = {
  image: File
  order: number
}

export type TProduct = {
  id: string
  name: string
  description: string
  quantity: number
  sku: string
  productStatus: ProductStatus
  status: string
  productPrice: number
  averageRating: number
  totalView: number
  category: TCategory
  brand: TBrand
  media: TProductImage[]
}

export type TProductAdd = {
  name: string
  description: string
  quantity: number
  productStatus: ProductStatus
  status: string
  productPrice: number
  categoryId: string
  brandId: string
}

export type TProductUpdate = {
  id: string
  name: string
  description: string
  quantity: number
  productStatus: ProductStatus
  status: string
  productPrice: number
  categoryId: string
  brandId: string
}

export type TProductDelete = {
  id: string
}

export type ResponseListProducts = {
  items: TProduct[]
} & PaginationType

export type ProductQueryParams = {} & FilterType

export type ProductQueryConfig = {
  [key in keyof ProductQueryParams]: string
}
