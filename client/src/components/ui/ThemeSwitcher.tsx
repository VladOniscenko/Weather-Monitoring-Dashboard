import { type Theme } from '@/hooks/useTheme';

type Props = {
    theme: Theme;
    themes: Theme[];
    onChange: (theme: Theme) => void;
};

export const ThemeSwitcher = ({ theme, themes, onChange }: Props) => {
    return (
        <select
            value={theme}
            onChange={(e) => onChange(e.target.value as Theme)}
            className="border px-3 py-2 rounded bg-[var(--bg-card)]"
        >
            {themes.map((t) => (
                <option key={t} value={t}>
                    {t}
                </option>
            ))}
        </select>
    );
};
