import * as React from "react"
import { Main } from "@/components/layout/main"
import useParams from "@/hooks/use-params"
import { isUndefined, omitBy } from "lodash"
import {
  ProductQueryConfig,
  ProductQueryParams,
  ResponseListProducts,
  TProduct,
} from "@/@types/product.type"
import { useQuery, useQueryClient } from "@tanstack/react-query"
import { GetAllProductsPagination } from "@/services/product.service"
import { ProductFormButton } from "@/pages/admin/products/components/ProductFormButton"
import ProductsProvider from "@/pages/admin/products/context/products-context"
import { ProductsDialogs } from "@/pages/admin/products/components/ProductsDialogs"
import { useState } from "react"
import {
  ColumnDef,
  ColumnFiltersState,
  flexRender,
  getCoreRowModel,
  getFacetedRowModel,
  getFacetedUniqueValues,
  getFilteredRowModel,
  getPaginationRowModel,
  getSortedRowModel,
  RowData,
  SortingState,
  useReactTable,
  VisibilityState,
} from "@tanstack/react-table"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { DataTablePagination } from "@/pages/admin/products/table/data-table-pagination"
import { Checkbox } from "@/components/ui/checkbox"
import { cn } from "@/lib/utils"
import styles from "@/pages/admin/products/styles/column.module.scss"
import { DataTableColumnHeader } from "@/pages/admin/products/table/data-table-column-header"
import LongText from "@/components/long-text"
import { DataTableRowActions } from "@/pages/admin/products/table/data-table-row-actions"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { X } from "lucide-react"
import IconSpinner from "@/components/icon-spinner"
import { LoadingScreen } from "@/layouts/loading-screen"
import SearchInput from "@/components/search"
import { all } from "axios"
import { GetAllSubCategories } from "@/services/category.service"
import { GetAllBrands } from "@/services/brand.service"
import { TBrand } from "@/@types/brand.type"
import { TCategory } from "@/@types/category.type"

declare module "@tanstack/react-table" {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  interface ColumnMeta<TData extends RowData, TValue> {
    className: string
  }
}

export default function ProductDashboard() {
  const queryClient = useQueryClient()

  const queryParams: ProductQueryParams = useParams()
  const queryConfig: ProductQueryConfig = omitBy(
    {
      searchTerm: queryParams.searchTerm,
      pageIndex: queryParams.pageIndex || "1",
      pageSize: queryParams.pageSize || "10",
    },
    isUndefined
  )

  const { data, isFetching } = useQuery({
    queryKey: ["products", queryConfig],
    queryFn: () => GetAllProductsPagination(queryConfig),
    select: (res) => res.data.value,
  })

  const { data: categories, isFetching: isFetchingCategories } = useQuery({
    queryKey: ["all-sub-categories"],
    queryFn: () => GetAllSubCategories(),
    select: (res) => res.data.value,
  })

  const { data: brands, isFetching: isFetchingBrands } = useQuery({
    queryKey: ["all-brands"],
    queryFn: () => GetAllBrands(),
    select: (res) => res.data.value,
  })

  const refetch = async () => {
    await queryClient.invalidateQueries(["products", queryConfig])
  }

  const [rowSelection, setRowSelection] = useState({})
  const [columnVisibility, setColumnVisibility] = useState<VisibilityState>({})
  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([])
  const [sorting, setSorting] = useState<SortingState>([])

  const columns: ColumnDef<TProduct>[] = [
    {
      id: "select",
      header: ({ table }) => (
        <Checkbox
          checked={
            table.getIsAllPageRowsSelected() ||
            (table.getIsSomePageRowsSelected() && "indeterminate")
          }
          onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
          aria-label="Select all"
          className="translate-y-[2px]"
        />
      ),
      cell: ({ row }) => (
        <Checkbox
          checked={row.getIsSelected()}
          onCheckedChange={(value) => row.toggleSelected(!!value)}
          aria-label="Select row"
          className="translate-y-[2px]"
        />
      ),
      meta: {
        className: cn(
          styles["checkbox"],
          "sticky md:table-cell left-0 z-10 rounded-tl",
          "text-foreground h-10 px-2 text-left align-middle font-medium whitespace-nowrap [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]"
        ),
      },
      enableSorting: false,
      enableHiding: false,
    },
    {
      id: "id",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Id" />
      ),
      cell: ({ row }) => {
        const { id } = row.original
        return <LongText>{id}</LongText>
      },
    },
    {
      id: "name",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Name" />
      ),
      cell: ({ row }) => {
        const { name } = row.original
        return <LongText>{name}</LongText>
      },
    },
    {
      id: "quantity",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Quantity" />
      ),
      cell: ({ row }) => {
        const { quantity } = row.original
        return <LongText>{quantity}</LongText>
      },
    },
    {
      id: "sku",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Sku" />
      ),
      cell: ({ row }) => {
        const { sku } = row.original
        return <LongText>{sku}</LongText>
      },
    },
    {
      id: "price",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Price" />
      ),
      cell: ({ row }) => {
        const { productPrice } = row.original
        return <LongText>{productPrice}</LongText>
      },
    },
    {
      id: "status",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Status" />
      ),
      cell: ({ row }) => {
        const { productStatus } = row.original
        return <LongText>{productStatus}</LongText>
      },
    },
    {
      accessorKey: "description",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Description" />
      ),
      cell: ({ row }) => <LongText>{row.getValue("description")}</LongText>,
      enableSorting: false,
      enableHiding: false,
    },
    {
      id: "actions",
      cell: DataTableRowActions,
      meta: {
        className: styles["action-column"],
      },
    },
  ]

  const table = useReactTable({
    data: data && data?.items ? data?.items : [],
    columns,
    state: {
      sorting,
      columnVisibility,
      rowSelection,
      columnFilters,
    },
    onRowSelectionChange: setRowSelection,
    onSortingChange: setSorting,
    onColumnFiltersChange: setColumnFilters,
    onColumnVisibilityChange: setColumnVisibility,
    getCoreRowModel: getCoreRowModel(),
    getFilteredRowModel: getFilteredRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    getSortedRowModel: getSortedRowModel(),
    getFacetedRowModel: getFacetedRowModel(),
    getFacetedUniqueValues: getFacetedUniqueValues(),
  })

  const isFiltered = !!queryConfig.searchTerm

  return (
    <ProductsProvider>
      <Main>
        <div className="mb-2 flex flex-wrap items-center justify-between space-y-2">
          <div>
            <h2 className="text-2xl font-bold tracking-tight">Product List</h2>

            <p className="text-muted-foreground">
              Manage store's products here.
            </p>
          </div>

          <ProductFormButton />
        </div>

        <div className="-mx-4 flex-1 overflow-auto px-4 py-1 lg:flex-row lg:space-y-0 lg:space-x-12">
          <div className="space-y-4">
            {/* Table search + filter */}
            <SearchInput
              queryConfig={queryConfig}
              path="/products"
              placeholder="Search in products..."
              isFiltered={isFiltered}
            />

            <div className="rounded-md border">
              {isFetching && (
                <LoadingScreen>
                  <IconSpinner />
                </LoadingScreen>
              )}

              {!isFetching && (
                <Table>
                  <TableHeader>
                    {table.getHeaderGroups().map((headerGroup) => (
                      <TableRow key={headerGroup.id} className="group/row">
                        {headerGroup.headers.map((header) => {
                          return (
                            <TableHead
                              key={header.id}
                              colSpan={header.colSpan}
                              className={
                                header.column.columnDef.meta?.className ?? ""
                              }
                            >
                              {header.isPlaceholder
                                ? null
                                : flexRender(
                                    header.column.columnDef.header,
                                    header.getContext()
                                  )}
                            </TableHead>
                          )
                        })}
                      </TableRow>
                    ))}
                  </TableHeader>

                  <TableBody>
                    {table.getRowModel().rows?.length ? (
                      table.getRowModel().rows.map((row) => (
                        <TableRow
                          key={row.id}
                          data-state={row.getIsSelected() && "selected"}
                          className="group/row"
                        >
                          {row.getVisibleCells().map((cell) => (
                            <TableCell
                              key={cell.id}
                              className={
                                cell.column.columnDef.meta?.className ?? ""
                              }
                            >
                              {flexRender(
                                cell.column.columnDef.cell,
                                cell.getContext()
                              )}
                            </TableCell>
                          ))}
                        </TableRow>
                      ))
                    ) : (
                      <TableRow>
                        <TableCell
                          colSpan={columns.length}
                          className="h-24 text-center"
                        >
                          No results.
                        </TableCell>
                      </TableRow>
                    )}
                  </TableBody>
                </Table>
              )}
            </div>

            {!isFetching && (
              <DataTablePagination
                table={table}
                queryConfig={queryConfig}
                className="mt-7"
                pageSize={data!.pageSize}
                pageIndex={data!.pageIndex}
                totalPage={data!.totalPage}
                totalCount={data!.totalCount}
                hasPreviousPage={data!.hasPreviousPage}
                hasNextPage={data!.hasNextPage}
                firstRowOnPage={data!.firstRowOnPage}
                lastRowOnPage={data!.lastRowOnPage}
              />
            )}
          </div>
        </div>
      </Main>

      {(!isFetchingCategories || !isFetchingBrands) && (
        <ProductsDialogs
          categories={
            categories ?? ([{ id: "", name: "No category" }] as TCategory[])
          }
          brands={brands ?? ([{ id: "", name: "No brand" }] as TBrand[])}
          refetch={refetch}
        />
      )}
    </ProductsProvider>
  )
}
