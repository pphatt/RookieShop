import * as React from "react"
import { Loader2 } from "lucide-react"
import styles from "@/components/icon-spinner/style.module.scss"

export default function IconSpinner() {
  return <Loader2 className={styles["spinner"]} />
}
