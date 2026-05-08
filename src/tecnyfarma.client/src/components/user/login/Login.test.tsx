import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import * as LoginService from './LoginService';
import { Login } from './Login';

vi.mock('./LoginService');

describe('Login component', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('renders email and password fields and login button', () => {
        render(<Login />);
        
        expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/password/i)).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /login/i })).toBeInTheDocument();
    });

    it('shows success message on successful login', async () => {
        (LoginService.login as any).mockResolvedValue({ ok: true });
        
        render(<Login />);
        
        fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'test@example.com' } });
        fireEvent.change(screen.getByLabelText(/password/i), { target: { value: 'password' } });
        fireEvent.click(screen.getByRole('button', { name: /login/i }));
        await waitFor(() => {
            expect(screen.getByText(/login successful/i)).toBeInTheDocument();
        });
    });

    it('shows error message on failed login', async () => {
        (LoginService.login as any).mockResolvedValue({ ok: false, text: async () => 'Invalid credentials' });
        
        render(<Login />);
        
        fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'fail@example.com' } });
        fireEvent.change(screen.getByLabelText(/password/i), { target: { value: 'wrong' } });
        fireEvent.click(screen.getByRole('button', { name: /login/i }));
        await waitFor(() => {
            expect(screen.getByText(/login failed: invalid credentials/i)).toBeInTheDocument();
        });
    });

    it('shows error message on network error', async () => {
        (LoginService.login as any).mockRejectedValue(new Error('Network error'));
        
        render(<Login />);
        
        fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'fail@example.com' } });
        fireEvent.change(screen.getByLabelText(/password/i), { target: { value: 'wrong' } });
        fireEvent.click(screen.getByRole('button', { name: /login/i }));
        await waitFor(() => {
            expect(screen.getByText(/login failed: network error/i)).toBeInTheDocument();
        });
    });
});

