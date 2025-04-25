import { ResponseLogin, TLogin, TRegister, User } from "@/@types/auth.type"
import { ResponseData } from "@/@types/response.type"
import { AUTH_API } from "@/apis/auth/auth.api"
import instanceAxios from "@/configs/axiosInstance"
import axios from "axios"

export const login = async (data: TLogin) =>
  await axios.post<ResponseData<ResponseLogin>>(AUTH_API.LOGIN, data)

export const register = async (data: TRegister) =>
  await axios.post<ResponseData<null>>(AUTH_API.REGISTER, data)

export const logout = async () =>
  await instanceAxios.post<ResponseData<null>>(AUTH_API.LOGOUT)
