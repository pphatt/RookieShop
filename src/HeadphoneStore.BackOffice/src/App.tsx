import { lazy, Suspense } from "react"
import { Outlet, Route, Routes } from "react-router-dom"
import { AdminMainLayout } from "@/pages/root"
import AdminGuard from "@/guards/AdminGuard"
import UnauthenticatedGuard from "@/guards/UnauthenticatedGuard"
import BrandDashboard from "@/pages/admin/brands"
import CategoryDashboard from "@/pages/admin/categories"
import ProductDashboard from "@/pages/admin/products"
import UserDashboard from "@/pages/admin/users"

const Login = lazy(() => import("@/pages/auth/Login"))

function App() {
  return (
    <Suspense>
      <Routes>
        <Route element={<UnauthenticatedGuard />}>
          <Route path="/login" element={<Login />}></Route>
        </Route>

        <Route element={<AdminGuard />}>
          <Route
            path={"/"}
            element={
              <AdminMainLayout>
                <>Dashboard</>
              </AdminMainLayout>
            }
          ></Route>

          <Route
            path={"/customers"}
            element={
              <AdminMainLayout>
                <UserDashboard />
              </AdminMainLayout>
            }
          ></Route>

          <Route
            path={"/products"}
            element={
              <AdminMainLayout>
                <ProductDashboard />
              </AdminMainLayout>
            }
          ></Route>

          <Route
            path={"/categories"}
            element={
              <AdminMainLayout>
                <CategoryDashboard />
              </AdminMainLayout>
            }
          ></Route>

          <Route
            path={"/brands"}
            element={
              <AdminMainLayout>
                <BrandDashboard />
              </AdminMainLayout>
            }
          ></Route>
        </Route>
      </Routes>
    </Suspense>
  )
}

export default App
