export type User = {
  email: string
  userId: string
  phoneNumber: string
  firstName?: string
  lastName?: string
  avatar?: string
  userStatus?: string
  roles: string[]
}

export type Role = {
  name: string
  permissions: string[]
}

export type TLogin = {
  email: string
  password: string
}

export type TRegister = {
  email: string
  password: string
  confirmPassword: string
}

export type ResponseLogin = {
  accessToken: string
  refreshToken: string
  refreshTokenExpiryTime: string
}
