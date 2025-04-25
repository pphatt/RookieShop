import React, { useState } from "react"
import { useQuery } from "@tanstack/react-query"
import {
  clearLS,
  getAccessTokenFromLS,
  getUserFromLS,
  saveUserToLS,
} from "@/utils/auth.utils"

import { User } from "@/@types/auth.type"
import { WhoAmI } from "@/services/user.services"
import { Role } from "@/@types/role.type"
import { toast } from "react-toastify"

type TInitialState = {
  isAuthenticated: boolean
  setIsAuthenticated: React.Dispatch<React.SetStateAction<boolean>>
  profile: undefined | User
  setProfile: React.Dispatch<React.SetStateAction<User | undefined>>
  isLoading: boolean
}

const initialAppContext: TInitialState = {
  isAuthenticated: Boolean(getAccessTokenFromLS()),
  setIsAuthenticated: () => {},
  profile: getUserFromLS() || undefined,
  setProfile: () => {},
  isLoading: false,
}

export const AppContext = React.createContext<TInitialState>(initialAppContext)

export const AppProvider = ({ children }: { children: React.ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(
    initialAppContext.isAuthenticated
  )

  const [profile, setProfile] = useState(initialAppContext.profile)

  const accessToken = getAccessTokenFromLS()
  const [enrolledListCourse, setEnrolledListCourse] = useState<string[]>([])

  const { data, isLoading: fetchWhoAmILoading } = useQuery({
    queryKey: ["whoami", isAuthenticated],
    queryFn: WhoAmI,
    select: (data) => data?.data?.value,
    enabled: isAuthenticated && Boolean(accessToken),
    onSuccess: (data) => {
      if (data) {
        const {
          email,
          firstName,
          lastName,
          avatar,
          roles,
          userId,
          userStatus,
          phoneNumber,
        } = data

        saveUserToLS({
          email,
          firstName,
          lastName,
          avatar,
          userStatus,
          userId,
          phoneNumber,
          roles: roles.map((item: Role) => item.name),
        })

        setProfile({
          email,
          firstName,
          lastName,
          avatar,
          userStatus,
          userId,
          phoneNumber,
          roles: roles.map((item: Role) => item.name),
        })
      }
    },
    onError: (err) => {
      setIsAuthenticated(false)
      setProfile(undefined)
      clearLS()
      toast.error("Error when fetch whoami")
    },
  })

  // App Context Global Loading
  const isLoading = isAuthenticated && fetchWhoAmILoading

  return (
    <AppContext.Provider
      value={{
        profile,
        setProfile,
        isAuthenticated,
        setIsAuthenticated,
        isLoading,
      }}
    >
      {children}
    </AppContext.Provider>
  )
}
