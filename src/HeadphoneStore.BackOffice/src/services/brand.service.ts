import { ResponseData } from "@/@types/response.type"
import instanceAxios from "@/configs/axiosInstance"
import { BRAND_API } from "@/apis/admin/brand.api"
import { BrandQueryConfig, ResponseListBrands } from "@/@types/brand.type"

export const GetBrandsPagination = async (params: BrandQueryConfig) =>
  await instanceAxios.get<ResponseData<ResponseListBrands>>(
    BRAND_API.PAGINATION,
    {
      params,
    }
  )
