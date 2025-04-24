import { Suspense } from "react"
import { Outlet, Route, Routes } from "react-router-dom"
import { RouteComponent } from "@/pages/general/Root"
import AdminDashboard from "@/pages/general/Dashboard"

function App() {
  return (
    <RouteComponent>
      <AdminDashboard></AdminDashboard>
    </RouteComponent>
  )
}

export default App
