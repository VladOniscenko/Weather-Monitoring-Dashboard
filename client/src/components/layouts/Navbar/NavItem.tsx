import { useNavigate } from 'react-router';
import styles from './navbar.module.css';

interface NavItemProps {
    placeholder: string;
    url: string;
}

export const NavItem = ({ placeholder, url }: NavItemProps) => {
    const navigate = useNavigate();

    return (
        <button
            className={styles.navItem}
            onClick={() => navigate(url)}
            aria-label={`Navigate to ${placeholder}`}
        >
            {placeholder}
        </button>
    );
};
