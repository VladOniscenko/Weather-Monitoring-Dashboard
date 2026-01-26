import { Routes, Route, Navigate } from 'react-router-dom';
import { StationList } from '@/features/weather/components/StationList';
// import { LoginPage } from '@/features/auth/pages/LoginPage';

export const AppRouter = () => {
  return (
    <Routes>
      {/* Public Routes */}
      <Route path="/" element={<>Home</>} />
      <Route path="/stations" element={<StationList />} />
      
      {/* Auth Routes */}
      {/* <Route path="/login" element={<LoginPage />} /> */}

      {/* Protected Routes (Admin Only) */}
      {/* <Route path="/admin/countries" element={<CountryManager />} /> */}

      {/* Fallback */}
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
};