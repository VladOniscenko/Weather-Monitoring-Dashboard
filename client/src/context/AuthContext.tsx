import { type UserDto, UsersService } from '@/client';
import { createContext, useState, useEffect, type ReactNode } from 'react';

interface AuthContextType {
    user: UserDto | null;
    token: string | null;
    isAuthenticated: boolean;
    isLoading: boolean;
    login: (token: string, userData: UserDto) => void;
    logout: () => void;
}

export const AuthContext = createContext<AuthContextType | undefined>(
    undefined
);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<UserDto | null>(null);
    const [token, setToken] = useState<string | null>(
        localStorage.getItem('token')
    );
    const [isLoading, setIsLoading] = useState<boolean>(true);

    useEffect(() => {
        const initAuth = async () => {
            const storedToken = localStorage.getItem('token');
            if (storedToken) {
                try {
                    // retrieve current user data
                    const currentUser = await UsersService.getCurrentUser();
                    if (!currentUser || !currentUser.data) {
                        throw new Error(currentUser.message || 'Login failed');
                    }

                    setToken(storedToken);
                    setUser(currentUser.data);
                } catch (error) {
                    console.error('Token invalid', error);
                    logout();
                }
            }
            setIsLoading(false);
        };

        initAuth();
    }, []);

    const login = (newToken: string, userData: UserDto) => {
        setToken(newToken);
        setUser(userData);
        localStorage.setItem('token', newToken);
    };

    const logout = () => {
        setToken(null);
        setUser(null);
        localStorage.removeItem('token');
    };

    // Derived state
    const isAuthenticated = !!token && !!user;

    return (
        <AuthContext.Provider
            value={{ user, token, isAuthenticated, isLoading, login, logout }}
        >
            {children}
        </AuthContext.Provider>
    );
};
