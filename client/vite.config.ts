import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [react()],
    resolve: {
        alias: {
            // This tells Vite: "@" means "current_directory/src"
            '@': path.resolve(__dirname, './src'),
        },
    },
});
