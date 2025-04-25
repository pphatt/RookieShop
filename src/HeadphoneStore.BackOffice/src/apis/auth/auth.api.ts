import { URL } from ".."

export const BASE_URL = `${URL}/Authentication`

export const AUTH_API = {
  LOGIN: `${BASE_URL}/login`,
  REGISTER: `${BASE_URL}/register`,

  LOGOUT: `${BASE_URL}/logout`,
  REFRESH_TOKEN: `${BASE_URL}/refreshToken`,
}
