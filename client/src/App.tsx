import { BrowserRouter } from 'react-router-dom';
import { AppRouter } from '@/app/router';
import { Navbar } from '@/components/layouts/Navbar/Navbar';

import { ThemeProvider, useAppTheme } from '@/context/ThemeContext';

const AppContent = () => {
    const { theme } = useAppTheme();

    return (
        <div className="theme-container min-h-screen" data-theme={theme}>
            <BrowserRouter>
                <Navbar />
                <AppRouter />
            </BrowserRouter>
        </div>
    );
};

const App = () => (
    <ThemeProvider>
        <AppContent />
    </ThemeProvider>
);

export default App;
