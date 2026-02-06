import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { OpenAPI, UsersService } from '@/client';
import { useAuth } from '@/hooks/useAuth';

export const useLogin = () => {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const { login: contextLogin } = useAuth();

    const login = async (email: string, password: string) => {
        setIsLoading(true);
        setError(null);

        try {
            if (!email || !password) {
                throw new Error('Make sure all required fields are filled');
            }
            
            // try to login and retrieve session token
            const response = await UsersService.loginUser({ email, password });
            if (!response.success || !response.data?.token) {
                throw new Error(response.message || 'Login failed');
            }

            OpenAPI.TOKEN = response.data.token; // add token to the calls
            
            // retrieve current user data
            const currentUser = await UsersService.getCurrentUser();
            if(!currentUser || !currentUser.data){
                throw new Error(response.message || 'Login failed');
            }

            // set auth context
            contextLogin(response.data.token, currentUser.data);

            navigate('/map');
        } catch (err: any) {
            setError(
                err?.body?.message ||
                    err?.message ||
                    'Invalid email or password'
            );
        } finally {
            setIsLoading(false);
        }
    };

    return { login, isLoading, error };
};
