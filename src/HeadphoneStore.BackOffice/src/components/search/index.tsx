import { Input } from "@/components/ui/input"
import { debounce, omit } from "lodash"
import { Search, X } from "lucide-react"
import React from "react"
import { createSearchParams, useNavigate } from "react-router-dom"
import { Button } from "@/components/ui/button"
import { FilterType } from "@/@types/filter.type"

type IProps = {
  queryConfig: FilterType
  path: string
  placeholder: string
  isFiltered: boolean
}

export default function SearchInput({
  queryConfig,
  path,
  placeholder,
  isFiltered,
}: IProps) {
  const [search, setSearch] = React.useState<string>(
    queryConfig.searchTerm ?? ""
  )
  const navigate = useNavigate()

  const onSearchChange = React.useCallback(
    debounce((searchValue: string) => {
      if (searchValue) {
        navigate({
          pathname: path,
          search: createSearchParams({
            ...queryConfig,
            pageIndex: "1",
            searchTerm: searchValue,
          }).toString(),
        })
      } else {
        const updatedQuery = omit(queryConfig, ["searchTerm"])

        navigate({
          pathname: path,
          search: createSearchParams({
            ...updatedQuery,
          }).toString(),
        })
      }
    }, 300),
    [navigate, path, queryConfig]
  )

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearch(event.target.value)
    onSearchChange(event.target.value)
  }

  const resetFiltered = () => {
    setSearch("")

    const updatedQuery = omit(queryConfig, ["searchTerm"])

    navigate({
      pathname: path,
      search: createSearchParams({
        pageIndex: "1",
        pageSize: "10",
        ...updatedQuery,
      }).toString(),
    })
  }

  return (
    <div className="flex items-center justify-between">
      <div className="flex flex-1 flex-col-reverse items-start gap-y-2 sm:flex-row sm:items-center sm:space-x-2">
        <Input
          placeholder={placeholder}
          defaultValue={queryConfig.searchTerm}
          value={search}
          onChange={handleInputChange}
          className="h-8 w-[150px] lg:w-[250px]"
        />

        {isFiltered && (
          <Button
            variant="ghost"
            onClick={resetFiltered}
            className="h-8 px-2 lg:px-3"
          >
            Reset
            <X className="ml-2 h-4 w-4" />
          </Button>
        )}
      </div>
    </div>
  )
}
