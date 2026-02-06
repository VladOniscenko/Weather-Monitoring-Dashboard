import { Routes, Route, Navigate } from 'react-router-dom';
import Map from '@/features/weather/components/Map';
import { AuthPage } from '@/features/auth/components';
import { ProtectedRoute } from '@/components/ProtectedRoute';
import { PublicOnlyRoute } from '@/components/PublicOnlyRoute';
import { Logout } from '@/features/auth/components/Logout';

export const AppRouter = () => {
    return (
        <Routes>
            <Route path="/" element={<>Home</>} />

            <Route element={<PublicOnlyRoute />}>
                <Route
                    path="/login"
                    element={<AuthPage isRegister={false} />}
                />
                <Route
                    path="/register"
                    element={<AuthPage isRegister={true} />}
                />
            </Route>

            <Route element={<ProtectedRoute />}>
                <Route path="/map" element={<Map />} />
                <Route path="/logout" element={<Logout />} />
            </Route>

            <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
    );
};
