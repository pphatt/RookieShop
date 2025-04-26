import * as React from "react"
import { cn } from "@/lib/utils"
import styles from "@/layouts/loading-screen/style.module.scss"
import IconSpinner from "@/components/icon-spinner"

interface LoadingScreenProps extends React.HTMLAttributes<HTMLElement> {
  fixed?: boolean
  ref?: React.Ref<HTMLElement>
}

export const LoadingScreen = ({ fixed, ...props }: LoadingScreenProps) => {
  return (
    <main
      className={cn(
        "peer-[.header-fixed]/header:mt-16",
        "px-4 py-6",
        fixed && "fixed-main flex grow flex-col overflow-hidden",
        styles["wrapper"]
      )}
      {...props}
    >
      <IconSpinner className={styles["icon"]} />
    </main>
  )
}

LoadingScreen.displayName = "LoadingScreen"
