import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import './globals.css';
import App from './App.tsx';
import { SelectedVideoProvider } from './Context/SelectedVideoContext.tsx';
import { SelectedMenuProvider } from './Context/SelectedMenuContext';

// Keep React Query available for local components that may use it; the standalone app no longer
// mounts the online AuthProvider or profile/session lifecycle.
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60 * 5,
      gcTime: 1000 * 60 * 30,
    },
  },
});

// Segra provides its own context menus where right-click actions are supported.
document.addEventListener('contextmenu', (event) => {
  event.preventDefault();
  window.dispatchEvent(new Event('segra:close-content-context-menus'));
});

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <SelectedVideoProvider>
        <SelectedMenuProvider>
          <App />
        </SelectedMenuProvider>
      </SelectedVideoProvider>
    </QueryClientProvider>
  </StrictMode>,
);
