import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import { MyPurchases } from './MyPurchases.tsx';
import * as PurchaseService from './PurchaseService';

vi.mock('./PurchaseService');

const findPurchases = vi.mocked(PurchaseService.findPurchases);

const purchases = [
    { id: '1', email: 'user@example.com', name: 'Aspirin', price: 5.5, createdAtUtc: '2026-01-02T10:30:00Z' },
    { id: '2', email: 'user@example.com', name: 'Paracetamol', price: 3.25, createdAtUtc: '2026-01-03T12:00:00Z' },
];

describe('MyPurchases', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('renders loading state initially', () => {
        findPurchases.mockReturnValue(new Promise(() => {}));

        render(<MyPurchases />);

        expect(screen.getByText(/Loading.../i)).toBeInTheDocument();
    });

    it('renders purchases with name, date and price after data loads', async () => {
        findPurchases.mockResolvedValue({ ok: true, json: async () => purchases } as unknown as Response);

        render(<MyPurchases />);

        await waitFor(() => expect(screen.getByText('Aspirin')).toBeInTheDocument());
        expect(screen.getByText('Paracetamol')).toBeInTheDocument();
        expect(screen.getByText(new Date(purchases[0].createdAtUtc).toLocaleString())).toBeInTheDocument();
        expect(screen.getByText(new Date(purchases[1].createdAtUtc).toLocaleString())).toBeInTheDocument();
        expect(screen.getByText('5.50')).toBeInTheDocument();
        expect(screen.getByText('3.25')).toBeInTheDocument();
    });

    it('renders no purchases message when the response is empty', async () => {
        findPurchases.mockResolvedValue({ ok: true, json: async () => [] } as unknown as Response);

        render(<MyPurchases />);

        await waitFor(() => expect(screen.getByText(/No purchases found/i)).toBeInTheDocument());
    });

    it('renders error message when the response is not ok', async () => {
        findPurchases.mockResolvedValue({ ok: false, status: 500 } as unknown as Response);

        render(<MyPurchases />);

        await waitFor(() => expect(screen.getByText(/Failed to load purchases \(500\)/i)).toBeInTheDocument());
    });

    it('renders error message on network failure', async () => {
        findPurchases.mockRejectedValue(new Error('Network error'));

        render(<MyPurchases />);

        await waitFor(() => expect(screen.getByText(/Failed to load purchases/i)).toBeInTheDocument());
    });
});
