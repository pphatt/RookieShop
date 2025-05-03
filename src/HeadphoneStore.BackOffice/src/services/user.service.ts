import { ResponseData } from "@/@types/response.type"
import instanceAxios from "@/configs/axiosInstance"
import { USER_API } from "@/apis/admin/user.api"
import {
  UserQueryConfig,
  ResponseListUsers,
  TUserAdd,
  TUserDelete,
  TUserUpdate,
} from "@/@types/user.type"

export const GetAllUsersPagination = async (params: UserQueryConfig) =>
  await instanceAxios.get<ResponseData<ResponseListUsers>>(
    USER_API.PAGINATION,
    {
      params,
    }
  )

export const AddNewUser = async (data: TUserAdd) =>
  await instanceAxios.post(USER_API.CREATE, data, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  })

export const UpdateUser = async (data: TUserUpdate) =>
  await instanceAxios.put(`${USER_API.UPDATE}/${data.id}`, data, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  })

export const DeleteUser = async (data: TUserDelete) =>
  await instanceAxios.delete(`${USER_API.DELETE}/${data.id}`)
