import React, { useState } from 'react';
import { ThemeSwitcher } from "@/components/ui/ThemeSwitcher";
import { useAppTheme } from "@/context/ThemeContext";
import styles from "./navbar.module.css";

export const Navbar: React.FC = () => {
    const [open, setOpen] = useState(false);
    const { theme, setTheme, THEMES } = useAppTheme();

    return (
        <div className={styles.navbar}>
            {!open && (
                <div className={styles.hamburger}>
                    <button onClick={() => setOpen(true)}>Menu</button>
                </div>
            )}

            {open && (
                <nav className={`card ${styles.navbarContent}`}>
                    <div className={styles.header}>
                        <a
                            aria-label="Close menu"
                            onClick={() => setOpen(false)}
                        >
                            close
                        </a>
                    </div>

                    <div className={styles.section}>
                        <span className={styles.sectionLabel}>Theme</span>
                        <ThemeSwitcher
                            theme={theme}
                            themes={THEMES}
                            onChange={setTheme}
                        />
                    </div>
                </nav>
            )}
        </div>
    );
};