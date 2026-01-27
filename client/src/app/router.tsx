import { Routes, Route, Navigate } from 'react-router-dom';
import { StationList } from '@/features/weather/components/StationList';
import MapLayout from '@/features/weather/components/MapLayout';
// import { LoginPage } from '@/features/auth/pages/LoginPage';

export const AppRouter = () => {
  return (
    <Routes>
      {/* Public Routes */}
      <Route path="/" element={<>Home</>} />
      <Route path="/stations" element={<StationList />} />
      <Route path="/map" element={<MapLayout/>} />
      
      {/* Auth Routes */}
      {/* <Route path="/login" element={<LoginPage />} /> */}

      {/* Protected Routes (Admin Only) */}
      {/* <Route path="/admin/countries" element={<CountryManager />} /> */}

      {/* Fallback */}
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
};