import { PaginationType } from "@/@types/pagination.type"
import { FilterType } from "@/@types/filter.type"

export type TCategory = {
  id: string
  name: string
  description: string
  parent?: TCategory
  status: string
}

export type TCategoryAdd = {
  name: string
  description: string
  parentCategoryId?: string
  status: string
}

export type TCategoryUpdate = {
  id: string
  name: string
  description: string
  parentCategoryId?: string
  status: string
}

export type TCategoryDelete = {
  id: string
}

export type ResponseListCategories = {
  items: TCategory[]
} & PaginationType

export type CategoryQueryParams = {} & FilterType

export type CategoryQueryConfig = {
  [key in keyof CategoryQueryParams]: string
}
