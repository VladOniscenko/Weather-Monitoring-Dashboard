import { useAuth } from '@/hooks/useAuth';

export const useProfile = () => {
    const { user, isLoading: userIsLoading } = useAuth();
    const isLoading = userIsLoading;

    return {
        user,
        isLoading,
    };
};
