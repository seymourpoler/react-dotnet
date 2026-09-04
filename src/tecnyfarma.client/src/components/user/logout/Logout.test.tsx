import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { Logout } from './Logout';
import * as LogoutService from './LogoutService';

vi.mock('./LogoutService');

const mockedLogout = vi.mocked(LogoutService.logout);

function mockResponse(ok: boolean, body = '') {
    return {
        ok,
        text: () => Promise.resolve(body),
    } as Response;
}

beforeEach(() => {
    vi.clearAllMocks();
});

describe('Logout', () => {
    it('renders heading and button', () => {
        render(<Logout />);
        expect(screen.getByRole('heading', { name: /are you sure/i })).toBeInTheDocument();
        expect(screen.getByRole('button', { name: /login/i })).toBeInTheDocument();
    });

    it('shows success message on successful logout', async () => {
        mockedLogout.mockResolvedValue(mockResponse(true));

        render(<Logout />);
        fireEvent.click(screen.getByRole('button'));

        await waitFor(() => {
            expect(screen.getByText('Logout successful!')).toBeInTheDocument();
        });
    });

    it('shows error message when response is not ok', async () => {
        mockedLogout.mockResolvedValue(mockResponse(false, 'Unauthorized'));

        render(<Logout />);
        fireEvent.click(screen.getByRole('button'));

        await waitFor(() => {
            expect(screen.getByText('Logout failed: Unauthorized')).toBeInTheDocument();
        });
    });

    it('shows error message on network failure', async () => {
        mockedLogout.mockRejectedValue(new Error('Network error'));

        render(<Logout />);
        fireEvent.click(screen.getByRole('button'));

        await waitFor(() => {
            expect(screen.getByText('Logout failed: Network error')).toBeInTheDocument();
        });
    });

    it('disables button while loading', async () => {
        let resolveLogout!: (v: Response) => void;
        mockedLogout.mockImplementation(
            () => new Promise((resolve) => { resolveLogout = resolve; })
        );

        render(<Logout />);
        const button = screen.getByRole('button');

        fireEvent.click(button);
        expect(button).toBeDisabled();

        resolveLogout(mockResponse(true));
        await waitFor(() => {
            expect(button).not.toBeDisabled();
        });
    });

    it('calls logout service on button click', async () => {
        mockedLogout.mockResolvedValue(mockResponse(true));

        render(<Logout />);
        fireEvent.click(screen.getByRole('button'));

        expect(mockedLogout).toHaveBeenCalledTimes(1);
    });
});
