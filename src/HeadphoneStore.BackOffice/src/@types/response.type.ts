export type ResponseData<Data> = {
  isSuccess: boolean
  isFailure: boolean
  value?: Data | null
  error: {
    code?: string
    message?: string
  }
}
