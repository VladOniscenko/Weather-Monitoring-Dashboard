import UserDetails from '@/features/profile/components/UserDetails';
import UserStations from '@/features/profile/components/UserStations';
import StationCreateForm from './StationCreateForm';

export const Profile = () => {
    return (
        <div className='p-4'>
            <UserDetails/>
            <StationCreateForm/>
            <UserStations/>
        </div>
    );
};
