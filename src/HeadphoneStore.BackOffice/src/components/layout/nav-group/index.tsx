import { ReactNode } from "react"
import { ChevronRight } from "lucide-react"
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "@/components/ui/collapsible"
import {
  SidebarGroup,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarMenuSub,
  SidebarMenuSubButton,
  SidebarMenuSubItem,
  useSidebar,
} from "@/components/ui/sidebar"
import { Badge } from "@/components/ui/badge"
import {
  NavCollapsible,
  NavItem,
  NavLink,
  type NavGroup,
} from "@/data/sidebar-data"
import { Link, useLocation } from "react-router-dom"
import { Separator } from "@/components/ui/separator"
import * as React from "react"
import { cn } from "@/lib/utils"

export function NavGroup({ title, items }: NavGroup) {
  const { open } = useSidebar()

  return (
    <SidebarGroup className={cn(!open && "pt-0 pb-1")}>
      <Separator
        orientation="horizontal"
        className={cn(
          "h-6",
          "opacity-0 transition-opacity ease-in-out",
          open && "duration-300",
          !open && "mb-1 opacity-100 duration-300 delay-100"
        )}
      />

      <SidebarGroupLabel>{title}</SidebarGroupLabel>

      <SidebarMenu>
        {items.map((item) => {
          const key = `${item.title}-${item.url}`

          return (
            <SidebarMenuLink key={key} item={item as NavLink} href={item.url} />
          )
        })}
      </SidebarMenu>
    </SidebarGroup>
  )
}

const NavBadge = ({ children }: { children: ReactNode }) => (
  <Badge className="rounded-full px-1 py-0 text-xs">{children}</Badge>
)

const SidebarMenuLink = ({ item, href }: { item: NavLink; href: string }) => {
  const { setOpenMobile } = useSidebar()

  return (
    <SidebarMenuItem>
      <SidebarMenuButton
        asChild
        isActive={checkIsActive(href, item)}
        tooltip={item.title}
      >
        <Link to={item.url} onClick={() => setOpenMobile(false)}>
          {item.icon && <item.icon />}
          <span>{item.title}</span>
          {item.badge && <NavBadge>{item.badge}</NavBadge>}
        </Link>
      </SidebarMenuButton>
    </SidebarMenuItem>
  )
}

function checkIsActive(href: string, item: NavItem, mainNav = false) {
  const location = useLocation().pathname

  return (
    href === location || // /endpint?search=param
    href.split("?")[0] === location || // endpoint
    (mainNav &&
      href.split("/")[1] !== "" &&
      href.split("/")[1] === location?.split("/")[1])
  )
}
