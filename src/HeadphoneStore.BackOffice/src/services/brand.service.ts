import { ResponseData } from "@/@types/response.type"
import instanceAxios from "@/configs/axiosInstance"
import { BRAND_API } from "@/apis/admin/brand.api"
import {
  BrandQueryConfig,
  ResponseListBrands,
  TBrandAdd,
} from "@/@types/brand.type"

export const GetBrandsPagination = async (params: BrandQueryConfig) =>
  await instanceAxios.get<ResponseData<ResponseListBrands>>(
    BRAND_API.PAGINATION,
    {
      params,
    }
  )

export const AddNewBrand = async (data: TBrandAdd) =>
  await instanceAxios.post(BRAND_API.CREATE, data, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  })
