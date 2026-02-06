import { type Theme } from '@/hooks/useTheme';
type Props = {
    theme: Theme;
    themes: Theme[];
    onChange: (theme: Theme) => void;
};

export const ThemeSwitcher = ({ theme, themes, onChange }: Props) => {
    return (
        <>
            <span className={`mb-0.5`}>Theme</span>
            <select
                value={theme}
                onChange={(e) => onChange(e.target.value as Theme)}
                className="border px-3 py-2 rounded bg-[var(--bg-card)]"
            >
                {themes.map((t) => (
                    <option key={t} value={t}>
                        {t[0].toUpperCase() + t.slice(1)}
                    </option>
                ))}
            </select>
        </>
    );
};
