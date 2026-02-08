import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    validateEmail,
    validateFullName,
    validatePassword,
} from '../utils/utils';
import { UsersService } from '@/client';

export const useRegister = () => {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const register = async (name: string, email: string, password: string) => {
        setIsLoading(true);
        setError(null);

        const validations = [
            validateFullName(name),
            validateEmail(email),
            validatePassword(password),
        ];

        try {
            const firstError = validations.find(Boolean);
            if (firstError) {
                throw new Error(firstError as string);
            }

            const response = await UsersService.registerUser({
                name,
                email,
                password,
            });

            if (!response.success) {
                throw new Error(response.message || 'Registration failed');
            }

            navigate('/login');
        } catch (err: any) {
            setError(
                err?.body?.message || err?.message || 'Something went wrong'
            );
        } finally {
            setIsLoading(false);
        }
    };

    return { register, isLoading, error };
};
