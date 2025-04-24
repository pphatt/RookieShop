import { cn } from "@/lib/utils"
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/components/ui/breadcrumb"

interface TopNavProps extends React.HTMLAttributes<HTMLElement> {
  links?: {
    title: string
    href: string
    isActive: boolean
    disabled?: boolean
  }[]
}

export function TopNav({ className, links, ...props }: TopNavProps) {
  return (
    <>
      <Breadcrumb
        className={cn(
          "hidden items-center space-x-4 md:flex lg:space-x-6",
          className
        )}
        {...props}
      >
        <BreadcrumbList>
          <BreadcrumbItem>
            <BreadcrumbLink className={"text-sm font-medium"} href="/">
              Home
            </BreadcrumbLink>
          </BreadcrumbItem>

          <BreadcrumbSeparator style={{ marginTop: "2px" }} />

          <BreadcrumbItem>
            <BreadcrumbPage className={"text-sm font-medium"}>
              Dashboard
            </BreadcrumbPage>
          </BreadcrumbItem>
        </BreadcrumbList>
      </Breadcrumb>
    </>
  )
}
