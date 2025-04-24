import { SidebarProvider } from "@/components/ui/sidebar"
import { AppSidebar } from "@/components/layout/side-bar"
import { Outlet } from "react-router-dom"
import { cn } from "@/lib/utils"
import { TopNav } from "@/components/layout/top-nav"
import { ThemeSwitch } from "@/components/layout/theme-switch"
import { ProfileDropdown } from "@/components/layout/profile-dropdown"
import { Header } from "@/components/layout/header"
import * as React from "react"

interface RouteComponentProps {
  children: React.ReactNode
}

export function AdminMainLayout({ children }: RouteComponentProps) {
  return (
    <SidebarProvider>
      <AppSidebar />

      <div
        id="content"
        className={cn(
          "ml-auto w-full max-w-full",
          "peer-data-[state=collapsed]:w-[calc(100%-var(--sidebar-width-icon)-1rem)]",
          "peer-data-[state=expanded]:w-[calc(100%-var(--sidebar-width))]",
          "sm:transition-[width] sm:duration-200 sm:ease-linear",
          "flex h-svh flex-col",
          "group-data-[scroll-locked=1]/body:h-full",
          "has-[main.fixed-main]:group-data-[scroll-locked=1]/body:h-svh"
        )}
      >
        <Header>
          <TopNav />

          <div className="ml-auto flex items-center space-x-4">
            <ThemeSwitch />
            <ProfileDropdown />
          </div>
        </Header>

        {children}
      </div>
    </SidebarProvider>
  )
}
