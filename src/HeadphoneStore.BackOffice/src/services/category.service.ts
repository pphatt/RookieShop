import { ResponseData } from "@/@types/response.type"
import instanceAxios from "@/configs/axiosInstance"
import { CATEGORY_API } from "@/apis/admin/category.api"
import {
  CategoryQueryConfig,
  ResponseListCategories,
  TCategory,
  TCategoryAdd,
  TCategoryDelete,
  TCategoryUpdate,
} from "@/@types/category.type"

export const GetAllCategoriesPagination = async (params: CategoryQueryConfig) =>
  await instanceAxios.get<ResponseData<ResponseListCategories>>(
    CATEGORY_API.PAGINATION,
    {
      params,
    }
  )

export const GetAllSubCategories = async () =>
  await instanceAxios.get<ResponseData<TCategory[]>>(CATEGORY_API.ALL_SUB)

export const GetAllFirstLevelCategoriesPagination = async () =>
  await instanceAxios.get<ResponseData<TCategory[]>>(
    CATEGORY_API.GET_ALL_FIRST_LEVEL
  )

export const AddNewCategory = async (data: TCategoryAdd) =>
  await instanceAxios.post(CATEGORY_API.CREATE, data, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  })

export const UpdateCategory = async (data: TCategoryUpdate) =>
  await instanceAxios.put(`${CATEGORY_API.UPDATE}/${data.id}`, data, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  })

export const DeleteCategory = async (data: TCategoryDelete) =>
  await instanceAxios.delete(`${CATEGORY_API.DELETE}/${data.id}`)
