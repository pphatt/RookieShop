import { HTMLAttributes, useState } from "react"
import { z } from "zod"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { PasswordInput } from "@/components/form/password-input"
import { Link } from "react-router-dom"
import { useAppContext } from "@/hooks/user-app-context"
import { useMutation } from "@tanstack/react-query"
import { TLogin } from "@/@types/auth.type"
import { login } from "@/services/auth.services"
import { toast } from "react-toastify"
import { jwtDecode } from "jwt-decode"
import {
  saveAccessTokenToLS,
  saveRefreshTokenToLS,
  saveUserToLS,
} from "@/utils/auth.utils"
import IconSpinner from "@/components/icon-spinner"
import { handleError } from "@/utils"

type UserAuthFormProps = HTMLAttributes<HTMLFormElement>

const formSchema = z.object({
  email: z
    .string()
    .min(1, { message: "Please enter your email" })
    .email({ message: "Invalid email address" }),
  password: z
    .string()
    .min(1, {
      message: "Please enter your password",
    })
    .min(7, {
      message: "Password must be at least 7 characters long",
    }),
})

export function UserAuthForm({ className, ...props }: UserAuthFormProps) {
  const form = useForm<z.infer<typeof formSchema>>({
    mode: "onBlur",
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  })

  const { setIsAuthenticated, setProfile } = useAppContext()

  const { isLoading, mutate } = useMutation({
    mutationFn: (body: TLogin) => login(body),
    onError: (err: any) => {
      handleError(err)
    },
  })

  const onSubmit = (data: TLogin) => {
    mutate(data, {
      onSuccess(info) {
        const accessToken = data && (info?.data?.value?.accessToken as string)
        const refreshToken = data && (info?.data?.value?.refreshToken as string)
        const exp = 10000 // temporary

        const dataDetail = jwtDecode(accessToken as string)

        const { email, fullname, id, permissions, phoneNumber, roles } =
          dataDetail as any

        const roleData = JSON.parse(roles)

        if (!roleData.includes("admin"))
          throw {
            response: {
              data: {
                title: "This account is not allowed to sign in",
                description: "Only admin accounts can access this dashboard.",
              },
            },
          }

        saveAccessTokenToLS(accessToken)
        saveRefreshTokenToLS(refreshToken as string, exp)
        saveUserToLS({ email, userId: id, phoneNumber, roles: roleData })
        setProfile({ email, userId: id, phoneNumber, roles: roleData })
        setIsAuthenticated(true)
        toast.success("Login successfully!")
      },
    })
  }

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className={cn("grid gap-3", className)}
        {...props}
      >
        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Email</FormLabel>
              <FormControl>
                <Input placeholder="name@example.com" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="password"
          render={({ field }) => (
            <FormItem className="relative">
              <FormLabel>Password</FormLabel>
              <FormControl>
                <PasswordInput placeholder="********" {...field} />
              </FormControl>
              <FormMessage />
              <Link
                to="/forgot-password"
                className="text-muted-foreground absolute -top-0.5 right-0 text-sm font-medium hover:opacity-75"
              >
                Forgot password?
              </Link>
            </FormItem>
          )}
        />

        <Button className="mt-2" disabled={isLoading}>
          {isLoading && <IconSpinner aria-hidden="true" />}
          Login
        </Button>
      </form>
    </Form>
  )
}
