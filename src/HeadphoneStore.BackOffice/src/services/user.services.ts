import instanceAxios from "@/configs/axiosInstance"
import { ResponseData } from "@/@types/response.type"
import { TRole } from "@/@types/role.type"
import { UserAPI } from "@/apis/user.api"

export interface GetMeResponse {
  userId: string
  firstName: string
  lastName: string
  email: string
  avatar: string
  phoneNumber: string
  bio?: string
  userStatus: string
  createdDateTime: string
  updatedDateTime: string
  roles: TRole
}

export const WhoAmI = async () =>
  await instanceAxios.get<ResponseData<GetMeResponse>>(UserAPI.WHOAMI)
