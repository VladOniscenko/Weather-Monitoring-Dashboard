import { BrowserRouter } from "react-router-dom";
import { AppRouter } from "@/app/router";
import { Navbar } from "@/components/layouts/Navbar";
import { useTheme } from "@/hooks/useTheme";

export const App = () => {
  const { theme } = useTheme();

  return (
    <BrowserRouter>
      <div
        className="theme-container flex flex-col min-h-screen transition-colors duration-500"
        data-theme={theme}
      >
        <Navbar />

        <main className="container mx-auto px-4 py-8">
          <AppRouter />
        </main>

        <footer className="py-4 text-center text-sm opacity-50">
          Weather Dashboard 2026
        </footer>
      </div>
    </BrowserRouter>
  );
};

export default App;