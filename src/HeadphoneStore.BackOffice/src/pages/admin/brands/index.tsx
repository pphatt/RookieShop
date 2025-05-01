import * as React from "react"
import { Main } from "@/components/layout/main"
import useParams from "@/hooks/use-params"
import { isUndefined, omitBy } from "lodash"
import {
  BrandQueryConfig,
  BrandQueryParams,
  ResponseListBrands,
  TBrand,
} from "@/@types/brand.type"
import { useQuery } from "@tanstack/react-query"
import { GetAllBrandsPagination } from "@/services/brand.service"
import { BrandFormButton } from "@/pages/admin/brands/components/BrandFormButton"
import BrandsProvider from "@/pages/admin/brands/context/brands-context"
import { BrandsDialogs } from "@/pages/admin/brands/components/BrandsDialogs"
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
import { DataTablePagination } from "@/pages/admin/brands/table/data-table-pagination"
import { Checkbox } from "@/components/ui/checkbox"
import { cn } from "@/lib/utils"
import styles from "@/pages/admin/brands/styles/column.module.scss"
import { DataTableColumnHeader } from "@/pages/admin/brands/table/data-table-column-header"
import LongText from "@/components/long-text"
import { DataTableRowActions } from "@/pages/admin/brands/table/data-table-row-actions"
import IconSpinner from "@/components/icon-spinner"
import { LoadingScreen } from "@/layouts/loading-screen"
import SearchInput from "@/components/search"
import { entityStatusTypes, EntityStatus } from "@/data"
import { Badge } from "@/components/ui/badge"
import { DataTableViewOptions } from "@/pages/admin/brands/table/data-table-view-options"

declare module "@tanstack/react-table" {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  interface ColumnMeta<TData extends RowData, TValue> {
    className: string
  }
}

export default function BrandDashboard() {
  const queryParams: BrandQueryParams = useParams()
  const queryConfig: BrandQueryConfig = omitBy(
    {
      searchTerm: queryParams.searchTerm,
      pageIndex: queryParams.pageIndex || "1",
      pageSize: queryParams.pageSize || "10",
    },
    isUndefined
  )

  const { data, isFetching, refetch } = useQuery({
    queryKey: ["brands", queryConfig],
    queryFn: () => GetAllBrandsPagination(queryConfig),
    select: (res) => res.data.value,
  })

  const [rowSelection, setRowSelection] = useState({})
  const [columnVisibility, setColumnVisibility] = useState<VisibilityState>({})
  const [columnFilters, setColumnFilters] = useState<ColumnFiltersState>([])
  const [sorting, setSorting] = useState<SortingState>([])

  const columns: ColumnDef<TBrand>[] = [
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
      enableHiding: false,
    },
    {
      accessorKey: "id",
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
      enableHiding: true,
    },
    {
      accessorKey: "name",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Name" />
      ),
      cell: ({ row }) => {
        const { name } = row.original
        return <LongText>{name}</LongText>
      },
      enableHiding: true,
    },
    {
      accessorKey: "slug",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Slug" />
      ),
      cell: ({ row }) => {
        const { slug } = row.original
        return <LongText>{slug}</LongText>
      },
      enableHiding: true,
    },
    {
      accessorKey: "status",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Status" />
      ),
      cell: ({ row }) => {
        const { status } = row.original
        const badgeColor = entityStatusTypes.get(
          status.toLowerCase() as EntityStatus
        )

        return (
          <div className="flex space-x-2">
            <Badge variant="outline" className={cn("capitalize", badgeColor)}>
              {status}
            </Badge>
          </div>
        )
      },
      enableHiding: true,
    },
    {
      accessorKey: "description",
      header: ({ column }) => (
        <DataTableColumnHeader column={column} title="Description" />
      ),
      cell: ({ row }) => <LongText>{row.getValue("description")}</LongText>,
      enableSorting: false,
      enableHiding: true,
    },
    {
      id: "actions",
      cell: DataTableRowActions,
      meta: {
        className: styles["action-column"],
      },
      enableHiding: false,
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
    <BrandsProvider>
      <Main>
        <div className="mb-2 flex flex-wrap items-center justify-between space-y-2">
          <div>
            <h2 className="text-2xl font-bold tracking-tight">Brand List</h2>

            <p className="text-muted-foreground">Manage store's brands here.</p>
          </div>

          <BrandFormButton />
        </div>

        <div className="-mx-4 flex-1 overflow-auto px-4 py-1 lg:flex-row lg:space-y-0 lg:space-x-12">
          <div className="space-y-4">
            <div className="flex items-center justify-between">
              {/* Table search + filter */}
              <SearchInput
                queryConfig={queryConfig}
                path="/brands"
                placeholder="Search in brands..."
                isFiltered={isFiltered}
              />

              <DataTableViewOptions table={table} />
            </div>

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

      <BrandsDialogs refetch={refetch} />
    </BrandsProvider>
  )
}
