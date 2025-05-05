import {
  Headphones,
  LayoutDashboard,
  LayoutList,
  LayoutPanelLeft,
  Tag,
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
  url: string
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
    name: "abc",
    email: "abc123@gmail.com",
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
          title: "Product",
          url: "/products",
          icon: Headphones,
        },
        {
          title: "Category",
          url: "/categories",
          icon: Tag,
        },
        {
          title: "Brand",
          url: "/brands",
          icon: LayoutPanelLeft,
        },
      ],
    },
    {
      title: "Customer",
      items: [
        {
          title: "Customer",
          url: "/customers",
          icon: Users,
        },
      ],
    },
  ],
}
