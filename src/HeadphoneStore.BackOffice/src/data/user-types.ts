import { ShieldUser, Users } from "lucide-react"

export const userTypes = [
  {
    label: "Admin",
    value: "admin",
    icon: ShieldUser,
  },
  {
    label: "Customer",
    value: "customer",
    icon: Users,
  },
] as const
