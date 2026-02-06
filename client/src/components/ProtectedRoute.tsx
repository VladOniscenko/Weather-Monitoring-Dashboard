// src/components/ProtectedRoute.tsx
import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '@/hooks/useAuth';

export const ProtectedRoute = () => {
    const { isAuthenticated, isLoading } = useAuth();

    if (isLoading) {
        return <div>Loading...</div>; 
    }

    // If not logged in, redirect to login
    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    // If logged in, render child routes
    return <Outlet />;
};