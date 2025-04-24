import * as React from "react"
import { Main } from "@/components/layout/main"
import { UserFormButton } from "@/pages/admin/users/components/UserFormButton"
import { UsersDialogs } from "@/pages/admin/users/components/UsersDialogs"
import UsersProvider from "@/pages/admin/users/context/users-context"
import { UsersTable } from "@/pages/admin/users/table/users-table"
import { userListSchema } from "@/@types/user.type"
import { users } from "@/model/user.scheme"
import { columns } from "@/pages/admin/users/table/columns"

export default function AdminDashboard() {
  const userList = userListSchema.parse(users)

  return (
    <UsersProvider>
      <Main>
        <div className="mb-2 flex flex-wrap items-center justify-between space-y-2">
          <div>
            <h2 className="text-2xl font-bold tracking-tight">User List</h2>

            <p className="text-muted-foreground">
              Manage your users and their roles here.
            </p>
          </div>

          <UserFormButton />
        </div>

        <div className="-mx-4 flex-1 overflow-auto px-4 py-1 lg:flex-row lg:space-y-0 lg:space-x-12">
          <UsersTable data={userList} columns={columns} />
        </div>
      </Main>

      <UsersDialogs />
    </UsersProvider>
  )
}
