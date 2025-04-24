import {
  Headphones,
  LayoutDashboard,
  LayoutList,
  LayoutPanelLeft,
  Users,
} from "lucide-react"

export interface User {
  name: string
  email: string
  avatar: string
}

export interface BaseNavItem {
  title: string
  badge?: string
  icon?: React.ElementType
}

export type NavLink = BaseNavItem & {
  url: string
  items?: never
}

export type NavCollapsible = BaseNavItem & {
  items: (BaseNavItem & { url: string })[]
  url?: never
}

export type NavItem = NavCollapsible | NavLink

export interface NavGroup {
  title: string
  items: NavItem[]
}

export interface SidebarData {
  user: User
  navGroups: NavGroup[]
}

export const sidebarData: SidebarData = {
  user: {
    name: "satnaing",
    email: "satnaingdev@gmail.com",
    avatar: "/avatars/shadcn.jpg",
  },
  navGroups: [
    {
      title: "General",
      items: [
        {
          title: "Dashboard",
          url: "/",
          icon: LayoutDashboard,
        },
        {
          title: "Users",
          url: "/users",
          icon: Users,
        },
        {
          title: "Product",
          url: "/products",
          icon: Headphones,
        },
        {
          title: "Category",
          url: "/categories",
          icon: LayoutList,
        },
        {
          title: "Brand",
          url: "/brands",
          icon: LayoutPanelLeft,
        },
      ],
    },
  ],
}
