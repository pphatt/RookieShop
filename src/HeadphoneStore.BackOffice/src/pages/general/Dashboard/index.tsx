import * as React from "react"
import { Header } from "@/components/layout/header"
import { TopNav } from "@/components/layout/top-nav"
import { ThemeSwitch } from "@/components/layout/theme-switch"
import { ProfileDropdown } from "@/components/layout/profile-dropdown"

export default function AdminDashboard() {
  return (
    <>
      <Header>
        <TopNav />

        <div className="ml-auto flex items-center space-x-4">
          <ThemeSwitch />
          <ProfileDropdown />
        </div>
      </Header>
    </>
  )
}
