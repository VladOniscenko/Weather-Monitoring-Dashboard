import { useEffect, useState } from "react";

export type Theme = 
  | "light"         // High-clarity productivity (Standard)
  | "latte"         // Warm, paper-like light mode (Soft)
  | "dark"          // Standard professional slate
  | "dracula"       // High-contrast developer favorite (Purple/Pink)
  | "nord"          // Eye-strain reducing arctic blue/gray
  | "cyber"         // High-contrast glow / Outrun style
  | "forest"        // Eco-digital / moss tones

export const THEMES: Theme[] = [
  "light",
  "dark",
  "dracula",
  "nord",
  "latte",
  "cyber",
  "forest",
];

const getSystemTheme = (): Theme =>
  window.matchMedia("(prefers-color-scheme: dark)").matches
    ? "dark"
    : "light";

export const useTheme = () => {
  const [theme, setTheme] = useState<Theme>(() => {
    const stored = localStorage.getItem("theme");
    return (stored && THEMES.includes(stored as Theme))
      ? (stored as Theme)
      : getSystemTheme();
  });

  useEffect(() => {
    document.documentElement.setAttribute("data-theme", theme);
    localStorage.setItem("theme", theme);
  }, [theme]);

  return { theme, setTheme, THEMES };
};
