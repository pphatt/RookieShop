import { Suspense } from "react"
import { Outlet, Route, Routes } from "react-router-dom"
import { AdminMainLayout } from "@/pages/root"

function App() {
  return (
    <Suspense>
      <Routes>
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
      </Routes>
    </Suspense>
  )
}

export default App
