import ReactDOM from "react-dom/client"
import "./index.css"
import App from "@/App"
import { BrowserRouter } from "react-router-dom"
import { ThemeProvider } from "@/context/theme-context"
import { ToastContainer } from "react-toastify"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { AppProvider } from "@/context/app-context"
import { AxiosInterceptor } from "@/configs/axiosInstance"
import { ReactQueryDevtools } from "@tanstack/react-query-devtools"

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
    },
  },
})

ReactDOM.createRoot(document.getElementById("root")!).render(
  <BrowserRouter>
    <ToastContainer
      closeOnClick
      pauseOnHover={false}
      pauseOnFocusLoss={false}
    />

    <QueryClientProvider client={queryClient}>
      <AppProvider>
        <AxiosInterceptor>
          <ThemeProvider>
            <App />
          </ThemeProvider>
        </AxiosInterceptor>
      </AppProvider>

      <ReactQueryDevtools initialIsOpen={false} />
    </QueryClientProvider>
  </BrowserRouter>
)
