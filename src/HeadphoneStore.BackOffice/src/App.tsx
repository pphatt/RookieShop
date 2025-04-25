import { lazy, Suspense } from "react"
import { Outlet, Route, Routes } from "react-router-dom"
import { AdminMainLayout } from "@/pages/root"
import AdminGuard from "@/guards/AdminGuard"
import UnauthenticatedGuard from "@/guards/UnauthenticatedGuard"

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
            path={"/users"}
            element={
              <AdminMainLayout>
                <>User dashboard</>
              </AdminMainLayout>
            }
          ></Route>

          <Route
            path={"/products"}
            element={
              <AdminMainLayout>
                <>Products dashboard</>
              </AdminMainLayout>
            }
          ></Route>

          <Route
            path={"/categories"}
            element={
              <AdminMainLayout>
                <>Categories dashboard</>
              </AdminMainLayout>
            }
          ></Route>

          <Route
            path={"/brands"}
            element={
              <AdminMainLayout>
                <>Brands dashboard</>
              </AdminMainLayout>
            }
          ></Route>
        </Route>
      </Routes>
    </Suspense>
  )
}

export default App
