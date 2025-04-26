import { useSearchParams } from "react-router-dom"

const useParams = () => {
  const [params] = useSearchParams()

  const paramVariables = Object.fromEntries([...params])

  return paramVariables
}

export default useParams
