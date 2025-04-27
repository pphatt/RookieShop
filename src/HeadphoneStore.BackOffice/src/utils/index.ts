import { toast } from "react-toastify"

export const toTitleCase = (str: string) => {
  if (!str) return str

  return str
    .toLowerCase()
    .replace(/(^|\s)\w/g, (letter) => letter.toUpperCase())
}

export const handleError = (error: any) => {
  if (error?.response?.data?.title) {
    let errorMessage = error.response.data.title

    if (error?.response?.data?.description) {
      errorMessage += ` - ${error.response.data.description}`
    }

    toast.error(errorMessage)
  } else if (error?.response?.data?.Error?.Message) {
    toast.error(error.response.data.Error.Message)
  } else if (error?.response?.data?.message) {
    toast.error(error.response.data.message)
  } else {
    toast.error("Something went wrong")
  }
}
