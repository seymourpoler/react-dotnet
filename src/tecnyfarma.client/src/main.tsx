import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { App } from './App.tsx'

const root = document.getElementById('root');
if (!root) {
    console.log("Root element not found");
}
createRoot(root!).render(
    <StrictMode>
        <App />
    </StrictMode>
);
