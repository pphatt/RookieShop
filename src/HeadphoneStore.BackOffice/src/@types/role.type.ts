import { PaginationType } from "@/@types/pagination.type"
import { FilterType } from "@/@types/filter.type"

export type TRole = {
  id: string
  name: string
  displayName: string
  roleStatus: string
  description: string
  createdDateTime: string
  updatedDateTime: null | string
}

export type ResponseListRoles = {
  items: TRole[]
} & PaginationType

export type RoleQueryParams = {} & FilterType

export type RoleQueryConfig = {
  [key in keyof RoleQueryParams]: string
}
