import InputField from './InputField';
import { useState } from 'react';
import styles from './Auth.module.css';
import ErrorMessage from './ErrorMessage';
import { useRegister } from '../hooks/useRegister';

export const RegisterForm = () => {
    const { register, isLoading, error } = useRegister();

    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        register(name, email, password);
    };

    return (
        <form onSubmit={handleSubmit}>
            <ErrorMessage error={error} />

            <InputField
                placeholder="Full Name"
                type="text"
                value={name}
                onChange={(e) => setName(e.target.value)}
            />
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
                {isLoading ? 'Loading...' : 'Register'}
            </button>
        </form>
    );
};
