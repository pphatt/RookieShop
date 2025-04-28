import { ResponseData } from "@/@types/response.type"
import instanceAxios from "@/configs/axiosInstance"
import { PRODUCT_API } from "@/apis/admin/product.api"
import {
  ProductQueryConfig,
  ResponseListProducts,
  TProductAdd,
  TProductDelete,
  TProductUpdate,
} from "@/@types/product.type"

export const GetAllProductsPagination = async (params: ProductQueryConfig) =>
  await instanceAxios.get<ResponseData<ResponseListProducts>>(
    PRODUCT_API.PAGINATION,
    {
      params,
    }
  )

export const AddNewProduct = async (data: FormData) =>
  await instanceAxios.post(PRODUCT_API.CREATE, data, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  })

export const UpdateProduct = async (data: FormData) =>
  await instanceAxios.put(`${PRODUCT_API.UPDATE}/${data.get("id")}`, data, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  })

export const DeleteProduct = async (data: TProductDelete) =>
  await instanceAxios.delete(`${PRODUCT_API.DELETE}/${data.id}`)
