import React, { useState } from "react";
import { ThemeSwitcher } from "@/components/ui/ThemeSwitcher";
import { useAppTheme } from "@/context/ThemeContext";
import styles from "./navbar.module.css";
import { Settings } from "lucide-react";

export const Navbar: React.FC = () => {
  const [open, setOpen] = useState(false);
  const { theme, setTheme, THEMES } = useAppTheme();

  return (
    <div className={styles.navbar}>
      {!open && (
        <Settings onClick={() => setOpen(true)} className={styles.settingsBtn} />
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

          <div className={styles.section}>
            <span className={styles.sectionLabel}>Theme</span>
            <ThemeSwitcher theme={theme} themes={THEMES} onChange={setTheme} />
          </div>
        </nav>
      )}
    </div>
  );
};