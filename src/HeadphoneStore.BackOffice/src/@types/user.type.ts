import { PaginationType } from "@/@types/pagination.type"
import { FilterType } from "@/@types/filter.type"

export type TRole = {
  id: string
  displayName: string
}

export type TUser = {
  id: string
  firstName: string
  lastName: string
  email: string
  phoneNumber: string
  roles: TRole
  status: string
}

export type TUserAdd = {
  email: string
  firstName: string
  lastName: string
  phoneNumber: string
  roleId: string
  status: string
}

export type TUserUpdate = {
  id: string
  email: string
  firstName: string
  lastName: string
  phoneNumber: string
  roleId: string
  status: string
}

export type TUserDelete = {
  id: string
}

export type ResponseListUsers = {
  items: TUser[]
} & PaginationType

export type UserQueryParams = {} & FilterType

export type UserQueryConfig = {
  [key in keyof UserQueryParams]: string
}
