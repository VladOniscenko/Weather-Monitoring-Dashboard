import React, { useState } from 'react';
import { ThemeSwitcher } from '@/components/ui/ThemeSwitcher';
import { useAppTheme } from '@/context/ThemeContext';
import { Settings } from 'lucide-react';
import { useAuth } from '@/hooks/useAuth';
import { NavItem } from './NavItem';
import styles from './navbar.module.css';

export const Navbar: React.FC = () => {
    const [open, setOpen] = useState(false);
    const { theme, setTheme, THEMES } = useAppTheme();
    const { isAuthenticated } = useAuth();

    return (
        <div className={styles.navbar}>
            {!open && (
                <Settings
                    onClick={() => setOpen(true)}
                    className={styles.settingsBtn}
                />
            )}

            {open && (
                <nav className={`card ${styles.navbarContent}`}>
                    <a
                        className={styles.closeBtn}
                        aria-label="Close menu"
                        onClick={() => setOpen(false)}
                    >
                        close
                    </a>

                    <div className={styles.navList}>
                        {isAuthenticated ? (
                            <>
                                <NavItem placeholder="Home" url="/home" />
                                <NavItem placeholder="Map" url="/map" />
                                <NavItem placeholder="Profile" url="/profile" />
                                <NavItem placeholder="Logout" url="/logout" />
                            </>
                        ) : (
                            <>
                                <NavItem
                                    placeholder="Login / Sign-Up"
                                    url="/login"
                                />
                            </>
                        )}
                    </div>

                    <ThemeSwitcher
                        theme={theme}
                        themes={THEMES}
                        onChange={setTheme}
                    />
                </nav>
            )}
        </div>
    );
};
