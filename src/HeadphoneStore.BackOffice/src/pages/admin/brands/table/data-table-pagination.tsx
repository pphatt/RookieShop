import * as React from "react"
import { Button } from "@/components/ui/button"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import {
  ChevronLeft,
  ChevronRight,
  ChevronsLeft,
  ChevronsRight,
} from "lucide-react"
import { createSearchParams, useNavigate } from "react-router-dom"
import { Table } from "@tanstack/react-table"
import { JSX } from "react"
import { PaginationType } from "@/@types/pagination.type"
import { FilterType } from "@/@types/filter.type"

const RANGE = 2

interface DataTablePaginationProps<TData> extends PaginationType {
  table: Table<TData>
  path?: string
  queryConfig?: FilterType
  className?: string
}

export function Navigate(props: JSX.IntrinsicElements["svg"]) {
  return (
    <svg xmlns="http://www.w3.org/2000/svg" width={24} height={24} {...props}>
      <g clipPath="url(#clip0_2011_2816)">
        <path
          d="M18.01 20.13L16.24 21.9L6.34001 12L16.24 2.09998L18.01 3.86998L9.88001 12L18.01 20.13Z"
          fill="currentColor"
        />
      </g>
      <defs>
        <clipPath id="clip0_2011_2816">
          <rect
            width="24"
            height="24"
            fill="currentColor"
            transform="matrix(-1 0 0 1 24.5 0)"
          />
        </clipPath>
      </defs>
    </svg>
  )
}

export function DataTablePagination<TData>({
  table,
  path,
  queryConfig,
  pageIndex,
  pageSize,
  totalPage,
  totalCount,
  hasPreviousPage,
  hasNextPage,
  className,
}: DataTablePaginationProps<TData>) {
  const navigate = useNavigate()
  const page = Number(queryConfig?.pageIndex) || 1
  let dotAfter = false
  let dotBefore = false

  React.useEffect(() => {
    window.scrollTo({ top: 0, behavior: "smooth" })
  }, [page])

  const handlePageIndexChange = (newPageIndex: number) => {
    navigate({
      pathname: path,
      search: createSearchParams({
        ...queryConfig,
        pageIndex: newPageIndex.toString(),
      }).toString(),
    })
  }

  const handlePageSizeChange = (newPageSize: string) => {
    navigate({
      pathname: path,
      search: createSearchParams({
        ...queryConfig,
        pageIndex: "1",
        pageSize: newPageSize,
      }).toString(),
    })
  }

  function renderDotAfter(index: number) {
    if (!dotAfter) {
      dotAfter = true
      return (
        <Button asChild key={index} variant="outline" className="h-8 w-8 p-0">
          <span>...</span>
        </Button>
      )
    }
  }

  function renderDotBefore(index: number) {
    if (!dotBefore) {
      dotBefore = true
      return (
        <Button asChild key={index} variant="outline" className="h-8 w-8 p-0">
          <span>...</span>
        </Button>
      )
    }
  }

  return (
    <div
      className="flex items-center justify-between overflow-clip px-2"
      style={{ overflowClipMargin: 1 }}
    >
      <div className="text-muted-foreground hidden flex-1 text-sm sm:block">
        {table.getFilteredSelectedRowModel().rows.length} of{" "}
        {table.getFilteredRowModel().rows.length} row(s) selected.
      </div>

      <div className="flex items-center sm:space-x-6 lg:space-x-8">
        <div className="flex items-center space-x-2">
          <p className="hidden text-sm font-medium sm:block">Rows per page</p>
          <Select
            value={`${pageSize}`}
            onValueChange={(value) => {
              handlePageSizeChange(value)
            }}
          >
            <SelectTrigger className="h-8 w-[70px]">
              <SelectValue placeholder={pageSize} />
            </SelectTrigger>

            <SelectContent side="top">
              {[1, 3, 5, 10].map((pageSize) => (
                <SelectItem key={pageSize} value={`${pageSize}`}>
                  {pageSize}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div className="flex w-[100px] items-center justify-center text-sm font-medium">
          Page {table.getState().pagination.pageIndex + 1} of{" "}
          {table.getPageCount()}
        </div>

        <div className="flex items-center space-x-2">
          <Button
            variant="outline"
            className="hidden h-8 w-8 p-0 lg:flex"
            onClick={() => handlePageIndexChange(1)}
          >
            <span className="sr-only">Go to first page</span>
            <ChevronsLeft className="h-4 w-4" />
          </Button>

          <Button
            variant="outline"
            className="h-8 w-8 p-0"
            onClick={() => handlePageIndexChange(page - 1)}
            disabled={!hasPreviousPage}
          >
            <span className="sr-only">Go to previous page</span>
            <ChevronLeft className="h-4 w-4" />
          </Button>

          {Array(totalPage)
            .fill(0)
            .map((_, index) => {
              const pageNumber = index + 1

              if (
                page <= RANGE * 2 + 1 &&
                pageNumber > page + RANGE &&
                pageNumber <= totalPage - RANGE
              ) {
                return renderDotAfter(index)
              } else if (page > RANGE * 2 + 1 && page < totalPage - RANGE * 2) {
                if (pageNumber < page - RANGE && pageNumber > RANGE) {
                  return renderDotBefore(index)
                } else if (
                  pageNumber > page + RANGE &&
                  pageNumber < totalPage - RANGE + 1
                ) {
                  return renderDotAfter(index)
                }
              } else if (
                page >= totalPage - RANGE * 2 &&
                pageNumber > RANGE &&
                pageNumber < page - RANGE
              ) {
                return renderDotBefore(index)
              }

              return (
                <Button
                  key={index}
                  variant="outline"
                  className="h-8 w-8 p-0"
                  isActive={Number(pageNumber) === page}
                  onClick={() => handlePageIndexChange(pageNumber)}
                >
                  {pageNumber}
                </Button>
              )
            })}

          <Button
            variant="outline"
            className="h-8 w-8 p-0"
            onClick={() => handlePageIndexChange(page + 1)}
            disabled={!hasNextPage}
          >
            <span className="sr-only">Go to next page</span>
            <ChevronRight className="h-4 w-4" />
          </Button>

          <Button
            variant="outline"
            className="hidden h-8 w-8 p-0 lg:flex"
            onClick={() => handlePageIndexChange(totalPage)}
          >
            <span className="sr-only">Go to last page</span>
            <ChevronsRight className="h-4 w-4" />
          </Button>
        </div>
      </div>
    </div>
  )
}
