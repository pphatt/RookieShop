import * as React from "react"
import { Main } from "@/components/layout/main"
import useParams from "@/hooks/use-params"
import { isUndefined, omitBy } from "lodash"
import {
  CategoryQueryConfig,
  CategoryQueryParams,
  ResponseListCategories,
  TCategory,
} from "@/@types/category.type"
import { useQuery, useQueryClient } from "@tanstack/react-query"
import {
  GetAllCategoriesPagination,
  GetAllFirstLevelCategoriesPagination,
} from "@/services/category.service"
import { CategoryFormButton } from "@/pages/admin/categories/components/CategoryFormButton"
import CategoriesProvider from "@/pages/admin/categories/context/categories-context"
import { CategoriesDialogs } from "@/pages/admin/categories/components/CategoriesDialogs"
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
import { DataTablePagination } from "@/pages/admin/categories/table/data-table-pagination"
import { Checkbox } from "@/components/ui/checkbox"
import { cn } from "@/lib/utils"
import styles from "@/pages/admin/categories/styles/column.module.scss"
import { DataTableColumnHeader } from "@/pages/admin/categories/table/data-table-column-header"
import LongText from "@/components/long-text"
import { DataTableRowActions } from "@/pages/admin/categories/table/data-table-row-actions"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { X } from "lucide-react"
import IconSpinner from "@/components/icon-spinner"
import { LoadingScreen } from "@/layouts/loading-screen"
import SearchInput from "@/components/search"
import { all } from "axios"

declare module "@tanstack/react-table" {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  interface ColumnMeta<TData extends RowData, TValue> {
    className: string
  }
}

export default function CategoryDashboard() {
  const queryClient = useQueryClient()

  const queryParams: CategoryQueryParams = useParams()
  const queryConfig: CategoryQueryConfig = omitBy(
    {
      searchTerm: queryParams.searchTerm,
      pageIndex: queryParams.pageIndex || "1",
      pageSize: queryParams.pageSize || "10",
    },
    isUndefined
  )

  const { data, isFetching } = useQuery({
    queryKey: ["categories", queryConfig],
    queryFn: () => GetAllCategoriesPagination(queryConfig),
    select: (res) => res.data.value,
  })

  const { data: allFirstLevelCategories, isFetching: isFetchingFirstLevel } =
    useQuery({
      queryKey: ["first-level-categories"],
      queryFn: () => GetAllFirstLevelCategoriesPagination(),
      select: (res) => res.data.value,
    })

  const refetch = async () => {
    await queryClient.invalidateQueries(["categories", queryConfig])
    await queryClient.invalidateQueries(["first-level-categories"])
  }

  const [rowSelection, setRowSelection] = useState({})
  const [columnVisibility, setColumnVisibility] = useState<VisibilityState>({})
  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([])
  const [sorting, setSorting] = useState<SortingState>([])

  const columns: ColumnDef<TCategory>[] = [
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
        return <LongText className={styles["id-column"]}>{id}</LongText>
      },
      meta: {
        className: styles["id-column"],
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
      id: "parent",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Parent" />
      ),
      cell: ({ row }) => {
        const { parent } = row.original
        return <LongText>{parent?.name ?? "-"}</LongText>
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
    <CategoriesProvider>
      <Main>
        <div className="mb-2 flex flex-wrap items-center justify-between space-y-2">
          <div>
            <h2 className="text-2xl font-bold tracking-tight">Category List</h2>

            <p className="text-muted-foreground">
              Manage store's categories here.
            </p>
          </div>

          <CategoryFormButton />
        </div>

        <div className="-mx-4 flex-1 overflow-auto px-4 py-1 lg:flex-row lg:space-y-0 lg:space-x-12">
          <div className="space-y-4">
            {/* Table search + filter */}
            <SearchInput
              queryConfig={queryConfig}
              path="/brands"
              placeholder="Search in categories..."
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

      {!isFetchingFirstLevel && (
        <CategoriesDialogs
          allFirstLevelCategories={allFirstLevelCategories!}
          refetch={refetch}
        />
      )}
    </CategoriesProvider>
  )
}
