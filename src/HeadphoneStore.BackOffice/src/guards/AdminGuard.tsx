import { useAppContext } from "@/hooks/user-app-context"
import React from "react"
import { Navigate, Outlet, useNavigation } from "react-router-dom"
import { ROLES } from "@/constants/role"

export default function AdminGuard() {
  const { isAuthenticated, profile } = useAppContext()
  const isAdmin =
    isAuthenticated && profile?.roles && profile?.roles?.includes(ROLES.ADMIN)

  return isAdmin ? <Outlet></Outlet> : <Navigate to="/login"></Navigate>
}
