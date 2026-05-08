import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { Register } from './Register';

describe('Register', () => {
    beforeEach(() => {
        vi.restoreAllMocks();
    });

    it('renders the Register heading', () => {
        render(<Register />);
        expect(screen.getByRole('heading', { level: 2, name: /register/i })).toBeInTheDocument();
    });

    it('renders email and password inputs', () => {
        render(<Register />);
        expect(screen.getByLabelText(/email/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/password/i)).toBeInTheDocument();
    });

    it('renders a Register button', () => {
        render(<Register />);
        expect(screen.getByRole('button', { name: /register/i })).toBeInTheDocument();
    });

    it('updates email and password fields on change', () => {
        render(<Register />);
        const email = screen.getByLabelText(/email/i);
        const password = screen.getByLabelText(/password/i);

        fireEvent.change(email, { target: { value: 'test@example.com' } });
        fireEvent.change(password, { target: { value: 'secret123' } });

        expect(email).toHaveValue('test@example.com');
        expect(password).toHaveValue('secret123');
    });

    it('does not show a message initially', () => {
        render(<Register />);
        expect(screen.queryByText(/registration/i)).not.toBeInTheDocument();
    });

    it('shows success message and clears inputs on successful registration', async () => {
        vi.stubGlobal('fetch', vi.fn().mockResolvedValue({ ok: true }));

        render(<Register />);
        fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'test@example.com' } });
        fireEvent.change(screen.getByLabelText(/password/i), { target: { value: 'secret123' } });
        fireEvent.click(screen.getByRole('button', { name: /register/i }));

        await waitFor(() => expect(screen.getByText('Registration successful!')).toBeInTheDocument());
        expect(screen.getByLabelText(/email/i)).toHaveValue('');
        expect(screen.getByLabelText(/password/i)).toHaveValue('');
    });

    it('calls fetch with the correct payload', async () => {
        const fetch = vi.fn().mockResolvedValue({ ok: true });
        vi.stubGlobal('fetch', fetch);

        render(<Register />);
        fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'user@example.com' } });
        fireEvent.change(screen.getByLabelText(/password/i), { target: { value: 'pass456' } });
        fireEvent.click(screen.getByRole('button', { name: /register/i }));

        await waitFor(() => expect(fetch).toHaveBeenCalledOnce());
        expect(fetch).toHaveBeenCalledWith('/api/v0/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email: 'user@example.com', password: 'pass456' }),
        });
    });

    it('shows error message when registration fails with server error', async () => {
        vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
            ok: false,
            text: async () => 'Email already taken',
        }));

        render(<Register />);
        fireEvent.click(screen.getByRole('button', { name: /register/i }));

        await waitFor(() =>
            expect(screen.getByText('Registration failed: Email already taken')).toBeInTheDocument()
        );
    });

    it('shows error message on network error', async () => {
        vi.stubGlobal('fetch', vi.fn().mockRejectedValue(new Error('Network error')));

        render(<Register />);
        fireEvent.click(screen.getByRole('button', { name: /register/i }));

        await waitFor(() =>
            expect(screen.getByText('Registration failed: Network error')).toBeInTheDocument()
        );
    });

    it('disables the button while loading', async () => {
        let resolveFetch!: () => void;
        const promise = new Promise<{ ok: boolean }>((resolve) => {
            resolveFetch = () => resolve({ ok: true });
        });
        vi.stubGlobal('fetch', vi.fn().mockReturnValue(promise));

        render(<Register />);
        const button = screen.getByRole('button', { name: /register/i });
        fireEvent.click(button);

        expect(button).toBeDisabled();
        resolveFetch();
        await waitFor(() => expect(button).not.toBeDisabled());
    });
});

