export type EntityStatus = "active" | "inactive" | "suspended"

export const callTypes = new Map<EntityStatus, string>([
  ["active", "bg-teal-100/30 text-teal-900 dark:text-teal-200 border-teal-200"],
  [
    "inactive",
    "bg-destructive/10 dark:bg-destructive/50 text-destructive dark:text-primary border-destructive/10",
  ],
  ["suspended", "bg-neutral-300/40 border-neutral-300"],
])
