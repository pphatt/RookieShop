import { ResponseData } from "@/@types/response.type"
import instanceAxios from "@/configs/axiosInstance"
import { ROLE_API } from "@/apis/admin/role.api"
import { TRole } from "@/@types/role.type"

export const GetAllRoles = async () =>
  await instanceAxios.get<ResponseData<TRole[]>>(ROLE_API.ALL)
