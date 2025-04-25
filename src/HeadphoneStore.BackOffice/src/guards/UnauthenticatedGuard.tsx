import { useAppContext } from "@/hooks/user-app-context"
import { Navigate, Outlet } from "react-router-dom"

export default function UnauthenticatedGuard() {
  const { isAuthenticated } = useAppContext()

  return !isAuthenticated ? <Outlet></Outlet> : <Navigate to="/"></Navigate>
}
