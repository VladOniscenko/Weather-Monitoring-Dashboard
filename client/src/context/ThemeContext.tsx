import React, { createContext, useContext } from 'react';
import { useTheme, type Theme } from '@/hooks/useTheme';

interface ThemeContextType {
    theme: Theme;
    setTheme: (theme: Theme) => void;
    THEMES: Theme[];
}

const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

export const ThemeProvider: React.FC<{ children: React.ReactNode }> = ({
    children,
}) => {
    const themeData = useTheme();
    return (
        <ThemeContext.Provider value={themeData}>
            {children}
        </ThemeContext.Provider>
    );
};

export const useAppTheme = () => {
    const context = useContext(ThemeContext);
    if (!context)
        throw new Error('useAppTheme must be used within ThemeProvider');
    return context;
};
