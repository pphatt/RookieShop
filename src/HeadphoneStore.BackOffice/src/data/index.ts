export type EntityStatus = "active" | "inactive" | "suspended"

export type ProductStatus = "in-stock" | "out-of-stock" | "discontinued"

export const entityStatusTypes = new Map<EntityStatus, string>([
  ["active", "bg-teal-100/30 text-teal-900 dark:text-teal-200 border-teal-200"],
  [
    "inactive",
    "bg-destructive/10 dark:bg-destructive/50 text-destructive dark:text-primary border-destructive/10",
  ],
  ["suspended", "bg-neutral-300/40 border-neutral-300"],
])

export const productStatusTypes = new Map<ProductStatus, string>([
  [
    "in-stock",
    "bg-teal-100/30 text-teal-900 dark:text-teal-200 border-teal-200",
  ],
  [
    "out-of-stock",
    "bg-destructive/10 dark:bg-destructive/50 text-destructive dark:text-primary border-destructive/10",
  ],
  ["discontinued", "bg-neutral-300/40 border-neutral-300"],
])
