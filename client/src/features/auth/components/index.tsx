import { LoginForm } from './LoginForm';
import { RegisterForm } from './RegisterForm';
import { EarthIcon } from 'lucide-react';
import styles from './Auth.module.css';
import { Link } from 'react-router-dom';

interface AuthPageProps {
    isRegister?: boolean;
}

export const AuthPage = ({ isRegister = false }: AuthPageProps) => {
    return (
        <div className={styles.page}>
            <div className={styles.container}>
                <div className={styles.logoContainer}>
                    <EarthIcon className={styles.logo} size={100} />
                </div>

                {isRegister ? <RegisterForm /> : <LoginForm />}

                <div className="mt-4 text-center">
                    <Link
                        to={isRegister ? '/login' : '/register'}
                        className={styles.link}
                    >
                        {isRegister
                            ? 'Have an account? Login'
                            : 'Need an account? Register'}
                    </Link>
                </div>
            </div>
        </div>
    );
};
