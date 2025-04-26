import * as React from "react"
import { Loader2 } from "lucide-react"
import styles from "@/components/icon-spinner/style.module.scss"
import { cn } from "@/lib/utils"

interface IconSpinnerProps extends React.HTMLAttributes<SVGElement> {}

export default function IconSpinner({ className, ...props }: IconSpinnerProps) {
  return <Loader2 className={cn(className, styles["spinner"])} {...props} />
}
