import { Routes, Route, Navigate } from 'react-router-dom';
import Map from '@/features/weather/components/Map';
import { AuthPage } from '@/features/auth/components';

export const AppRouter = () => {
    return (
        <Routes>
            {/* Public Routes */}
            <Route path="/" element={<>Home</>} />
            <Route path="/map" element={<Map />} />
            <Route path="/login" element={<AuthPage isRegister={false} />} />
            <Route path="/register" element={<AuthPage isRegister={true} />} />

            {/* Auth Routes */}
            {/* <Route path="/login" element={<LoginPage />} /> */}

            {/* Protected Routes (Admin Only) */}
            {/* <Route path="/admin/countries" element={<CountryManager />} /> */}

            {/* Fallback */}
            <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
    );
};
