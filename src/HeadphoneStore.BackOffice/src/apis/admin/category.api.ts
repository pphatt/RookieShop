import { URL } from ".."

export const BASE_URL = `${URL}/Category`

export const CATEGORY_API = {
  CREATE: `${BASE_URL}/create`,
  UPDATE: `${BASE_URL}`,
  DELETE: `${BASE_URL}`,
  PAGINATION: `${BASE_URL}/pagination`,
  ALL_SUB: `${BASE_URL}/all-sub`,
  GET_ALL_FIRST_LEVEL: `${BASE_URL}/all-first-level`,
}
