import { User } from "@/@types/auth.type"

export const LocalStorageEventTarget = new EventTarget()

// Save To LS

export const saveAccessTokenToLS = (accessToken: string) => {
  try {
    localStorage.setItem("access_token", accessToken)
  } catch (err) {
    console.log("Error when saving access token to local storage", err)
  }
}

export const saveRefreshTokenToLS = (
  refreshToken: string,
  refreshTokenExpiryTime: number
) => {
  try {
    const data = {
      refreshToken,
      refreshTokenExpiryTime,
    }

    localStorage.setItem("refresh_token", JSON.stringify(data))
  } catch (err) {
    console.log("Error when saving refresh token to LS", err)
  }
}

export const saveUserToLS = (user: User) => {
  try {
    localStorage.setItem("user", JSON.stringify(user))
  } catch (err) {
    console.log("Error when saving user information to LS", err)
  }
}

export const savePermissions = (permissions: string[]) => {
  try {
    const permissionsJSON = JSON.stringify(permissions)
    localStorage.setItem("permissions", permissionsJSON)
  } catch (error) {
    console.error("Error saving permissions to local storage", error)
  }
}

// End Save to LS

// Clear LS
export const clearUserFromLS = () => {
  try {
    localStorage.removeItem("user")
  } catch (err) {
    console.log("Error when remove User information in LS", err)
  }
}

export const clearRefreshToken = () => {
  try {
    localStorage.removeItem("refresh_token")
  } catch (err) {
    console.log("Error when clear refresh token in LS", err)
  }
}

export const clearAccessTokenFromLS = () => {
  try {
    localStorage.removeItem("access_token")
  } catch (err) {
    console.log("Error when clearing access token to local storage", err)
  }
}

export const clearPermissions = () => {
  try {
    localStorage.removeItem("permissions")
  } catch (error) {
    console.error("Error deleting permissions from local storage", error)
  }
}

export const clearLS = () => {
  clearRefreshToken()
  clearAccessTokenFromLS()
  clearUserFromLS()
  clearPermissions()
}

// End

export const getPermissions = () => {
  try {
    const permissionsJSON = localStorage.getItem("permissions")
    return permissionsJSON ? JSON.parse(permissionsJSON) : null
  } catch (error) {
    console.error("Error retrieving permissions from local storage", error)
    return null
  }
}

export const getAccessTokenFromLS = () => {
  try {
    return localStorage.getItem("access_token") || ""
  } catch (error) {
    console.log("error when get access_token from LS", error)
  }
}

export const getRefreshToken = () => {
  try {
    const refreshTokenJSON = localStorage.getItem("refresh_token")
    return refreshTokenJSON ? JSON.parse(refreshTokenJSON) : {}
  } catch (err) {
    console.log("error when get refresh token from LS", err)
  }
}

export const getUserFromLS = () => {
  try {
    const user = localStorage.getItem("user")
    return user ? JSON.parse(user) : {}
  } catch (err) {
    console.log("Error when get user information in LS", err)
  }
}
