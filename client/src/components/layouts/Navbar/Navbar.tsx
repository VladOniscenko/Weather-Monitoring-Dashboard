import React, { useState } from 'react';
import { useNavigate } from 'react-router';
import { ThemeSwitcher } from '@/components/ui/ThemeSwitcher';
import { useAppTheme } from '@/context/ThemeContext';
import { Settings } from 'lucide-react';
import { useAuth } from '@/hooks/useAuth';
import { useProfile } from '@/features/profile/hooks/useProfile';
import styles from './navbar.module.css';
import { getInitials } from '@/helpers/user';

interface NavItemProps {
    placeholder: string;
    url: string;
}

const NavItem = ({ placeholder, url }: NavItemProps) => {
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

export const Navbar: React.FC = () => {
    const [open, setOpen] = useState(false);
    const { theme, setTheme, THEMES } = useAppTheme();
    const { isAuthenticated } = useAuth();
    const { user } = useProfile();

    return (
        <>
            {/* Floating Settings Button */}
            <div className={styles.navbarWrapper}>
                <Settings
                    onClick={() => setOpen(true)}
                    className={styles.settingsBtn}
                />
            </div>

            {open && (
                <>
                    {/* Backdrop */}
                    <div
                        className={styles.backdrop}
                        onClick={() => setOpen(false)}
                    />

                    {/* Floating Card */}
                    <div className={`${styles.navbarCard} card`}>
                        {/* Header */}
                        <div className={styles.navHeader}>
                            {/* Avatar with initials */}
                            <div
                                className={styles.avatar}
                                style={{
                                    width: 56,
                                    height: 56,
                                    borderRadius: '50%',
                                    background: 'var(--accent)',
                                    color: 'var(--accent-fg)',
                                }}
                            >
                                {getInitials(user?.name ?? '')}
                            </div>

                            <div className={styles.navTitleWrapper}>
                                <h3 className={styles.navTitle}>
                                    {user?.name ?? 'Welcome'}
                                </h3>
                                <p className={styles.navSubtitle}>Quick Menu</p>
                            </div>

                            <button
                                className={styles.closeBtn}
                                onClick={() => setOpen(false)}
                                aria-label="Close menu"
                            >
                                x
                            </button>
                        </div>

                        {/* Nav Items */}
                        <div className={styles.navList}>
                            {isAuthenticated ? (
                                <>
                                    <NavItem placeholder="Home" url="/home" />
                                    <NavItem placeholder="Map" url="/map" />
                                    <NavItem
                                        placeholder="Profile"
                                        url="/profile"
                                    />
                                    <NavItem
                                        placeholder="Logout"
                                        url="/logout"
                                    />
                                </>
                            ) : (
                                <NavItem
                                    placeholder="Login / Sign-Up"
                                    url="/login"
                                />
                            )}
                        </div>

                        {/* Theme Switcher */}
                        <ThemeSwitcher
                            theme={theme}
                            themes={THEMES}
                            onChange={setTheme}
                        />
                    </div>
                </>
            )}
        </>
    );
};
