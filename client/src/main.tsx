import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import { OpenAPI } from './client';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import './index.css';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

// 1. Set the Base URL
OpenAPI.BASE = import.meta.env.VITE_API_URL || 'http://localhost:5001';

// 2. Inject the Token dynamically
// This function runs before every request to attach the token
OpenAPI.TOKEN = async () => {
  return localStorage.getItem('token') || '';
};


const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      // Professional tip: only refetch when the window is refocused if you really need it
      refetchOnWindowFocus: false, 
      retry: 1,
    },
  },
})

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <App />
      <ReactQueryDevtools initialIsOpen={false} />
    </QueryClientProvider>
  </React.StrictMode>
);