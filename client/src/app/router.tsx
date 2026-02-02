import { Routes, Route, Navigate } from 'react-router-dom';
import Map from '@/features/weather/components/Map';
// import { LoginPage } from '@/features/auth/pages/LoginPage';

export const AppRouter = () => {
    return (
        <Routes>
            {/* Public Routes */}
            <Route path="/" element={<>Home</>} />
            <Route path="/map" element={<Map />} />

            {/* Auth Routes */}
            {/* <Route path="/login" element={<LoginPage />} /> */}

            {/* Protected Routes (Admin Only) */}
            {/* <Route path="/admin/countries" element={<CountryManager />} /> */}

            {/* Fallback */}
            <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
    );
};
