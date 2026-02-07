import { BrowserRouter } from 'react-router-dom';
import { AppRouter } from '@/app/router';
import { Navbar } from '@/components/layouts/Navbar/Navbar';

import { ThemeProvider, useAppTheme } from '@/context/ThemeContext';
import { AuthProvider } from './context/AuthContext';

const AppContent = () => {
    const { theme } = useAppTheme();

    return (
        <div className="theme-container" data-theme={theme}>
            <BrowserRouter>
                <Navbar />
                <AppRouter />
            </BrowserRouter>
        </div>
    );
};

const App = () => (
    <AuthProvider>
        <ThemeProvider>
            <AppContent />
        </ThemeProvider>
    </AuthProvider>
);

export default App;
