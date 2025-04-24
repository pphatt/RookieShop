import * as React from "react"
import { Header } from "@/components/layout/header"
import { TopNav } from "@/components/layout/top-nav"
import { ThemeSwitch } from "@/components/layout/theme-switch"
import { ProfileDropdown } from "@/components/layout/profile-dropdown"
import { Main } from "@/components/layout/main"
import { UserFormButton } from "@/pages/general/Dashboard/components/UserFormButton"
import { UsersDialogs } from "@/pages/general/Dashboard/components/UsersDialogs"
import UsersProvider from "@/pages/general/Dashboard/context/users-context"
import { UsersTable } from "@/pages/general/Dashboard/table/users-table"
import { userListSchema } from "@/@types/user.type"
import { users } from "@/model/user.scheme"
import { columns } from "@/pages/general/Dashboard/table/columns"

export default function AdminDashboard() {
  const userList = userListSchema.parse(users)

  return (
    <UsersProvider>
      <Header>
        <TopNav />

        <div className="ml-auto flex items-center space-x-4">
          <ThemeSwitch />
          <ProfileDropdown />
        </div>
      </Header>

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
