import ReactDOM from "react-dom/client"
import "./index.css"
import App from "@/App"
import { BrowserRouter } from "react-router-dom"
import { ThemeProvider } from "@/context/theme-context"

ReactDOM.createRoot(document.getElementById("root")!).render(
  <BrowserRouter>
    <ThemeProvider>
      <App />
    </ThemeProvider>
  </BrowserRouter>
)
