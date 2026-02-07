import { useAuth } from '@/hooks/useAuth';
import { useWeatherStations } from '@/features/weather/hooks/useWeatherStations';

export const useProfile = () => {
    const { user, isLoading: userIsLoading } = useAuth();

    // Only call the hook if user exists
    const stationsResult = user
        ? useWeatherStations({ userId: user.id })
        : { data: [], loading: false, error: null };

    const {
        data: stations,
        loading: stationsLoading,
        error: stationsError,
    } = stationsResult;

    const isLoading = userIsLoading || stationsLoading;

    return {
        user,
        stations,
        isLoading,
        stationsError,
    };
};
