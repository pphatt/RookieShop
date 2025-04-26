import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Link, useNavigate } from "react-router-dom"
import { useAppContext } from "@/hooks/user-app-context"
import { toTitleCase } from "@/utils"
import { useMutation } from "@tanstack/react-query"
import { logout } from "@/services/auth.services"
import { clearLS } from "@/utils/auth.utils"
import { toast } from "react-toastify"
import IconSpinner from "@/components/icon-spinner"

export function ProfileDropdown() {
  const { isAuthenticated, setProfile, setIsAuthenticated, profile } =
    useAppContext()

  let shortName =
    profile?.firstName && profile?.lastName
      ? profile?.firstName?.charAt(0) + profile?.lastName?.charAt(0)
      : profile?.email.charAt(0)

  let name =
    profile?.firstName && profile?.lastName
      ? toTitleCase(`${profile?.firstName} ${profile?.lastName}`)
      : profile?.email.split("@")[0]

  const navigate = useNavigate()

  const { mutate, isLoading } = useMutation({
    mutationFn: logout,
    onSuccess: () => {
      clearLS()
      setIsAuthenticated(false)
      setProfile(undefined)
      toast.success("Log out successfully !")
      navigate("/login")
    },
    onError: () => {
      toast.error("Error when logging out")
    },
  })

  const handleLogout = () => {
    mutate()
  }

  return (
    <DropdownMenu modal={false}>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" className="relative h-8 w-8 rounded-full">
          <Avatar className="h-8 w-8">
            <AvatarImage src="/avatars/01.png" alt="@shadcn" />
            <AvatarFallback>{shortName}</AvatarFallback>
          </Avatar>
        </Button>
      </DropdownMenuTrigger>

      <DropdownMenuContent className="w-56" align="end" forceMount>
        <DropdownMenuLabel className="font-normal">
          <div className="flex flex-col space-y-1">
            <p className="text-sm leading-none font-medium">{name}</p>
            <p className="text-muted-foreground text-xs leading-none">
              {profile?.email}
            </p>
          </div>
        </DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuGroup>
          <DropdownMenuItem asChild>
            <Link to="/settings">Profile</Link>
          </DropdownMenuItem>
        </DropdownMenuGroup>
        <DropdownMenuSeparator />
        <DropdownMenuItem onClick={() => handleLogout()}>
          {isLoading && <IconSpinner aria-hidden="true" />}
          Log out
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  )
}
