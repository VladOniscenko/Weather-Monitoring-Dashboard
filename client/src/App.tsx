import { BrowserRouter } from 'react-router-dom';
import { AppRouter } from './app/router';
// import { Navbar } from '@/components/layout/Navbar';

export const App = () => {
  return (
    <BrowserRouter>
      <div className="flex flex-col min-h-screen bg-slate-50">
        {/* <Navbar />  */}
        
        <main className="container mx-auto px-4 py-8">
          <AppRouter />
        </main>

        <footer className="py-4 text-center text-sm text-slate-400">
          Weather Dashboard 2026
        </footer>
      </div>
    </BrowserRouter>
  );
};

export default App;