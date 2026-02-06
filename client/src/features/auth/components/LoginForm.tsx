import InputField from './InputField';
import { useState } from 'react';
import styles from './Auth.module.css';
import { useLogin } from '@/features/auth/hooks/useLogin';
import ErrorMessage from './ErrorMessage';

export const LoginForm = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const { login, isLoading, error } = useLogin();

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        login(email, password);
    };

    return (
        <form onSubmit={handleSubmit}>
            <ErrorMessage error={error} />

            <InputField
                placeholder="Email"
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            />
            <InputField
                placeholder="Password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button
                disabled={isLoading}
                type="submit"
                className={styles.submit}
            >
                {isLoading ? 'Loading...' : 'Login'}
            </button>
        </form>
    );
};