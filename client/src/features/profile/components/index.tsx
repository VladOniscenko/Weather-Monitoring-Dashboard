import UserDetails from '@/features/profile/components/UserDetails';
import UserStations from '@/features/profile/components/UserStations';
import StationCreateForm from './StationCreateForm';
import { useState } from 'react';

export const Profile = () => {
    const [refreshKey, setRefreshKey] = useState(0);
    function handleStationCreated() {
        setRefreshKey((prev) => prev + 1);
    }
    return (
        <div className="p-4">
            <UserDetails />
            <StationCreateForm onCreated={handleStationCreated} />
            <UserStations refreshKey={refreshKey} />
        </div>
    );
};
