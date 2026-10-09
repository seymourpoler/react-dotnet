import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import { MemoryRouter, Routes, Route } from 'react-router-dom';
import { Purchase } from './Purchase';
import * as ProductService from '../product/ProductService';
import * as PurchaseService from './PurchaseService';

vi.mock('../product/ProductService');
vi.mock('./PurchaseService');

const find = vi.mocked(ProductService.find);
const createPurchase = vi.mocked(PurchaseService.createPurchase);

const mockProduct = { id: '1', name: 'Aspirin', description: 'Painkiller', price: 5.5 };

function renderPurchase(params: { productId: string } = { productId: '1' }) {
    return render(
        <MemoryRouter initialEntries={[`/purchase/${params.productId}`]}>
            <Routes>
                <Route path="/purchase/:productId" element={<Purchase />} />
            </Routes>
        </MemoryRouter>
    );
}

describe('Purchase', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('renders loading state initially', () => {
        find.mockImplementation(() => new Promise(() => {}));

        renderPurchase();

        expect(screen.getByText(/Loading.../i)).toBeInTheDocument();
    });

    it('renders product details after data loads', async () => {
        find.mockImplementation(() => Promise.resolve({ 
            ok: true, 
            json: async () => [mockProduct] 
        } as unknown as Response));

        renderPurchase();

        await waitFor(() => expect(screen.getByText('Aspirin')).toBeInTheDocument());
        expect(screen.getByText('Painkiller')).toBeInTheDocument();
        expect(screen.getAllByText(/5\.50/)).toHaveLength(2);
    });

    it('renders error when product not found', async () => {
        find.mockImplementation(() => Promise.resolve({ 
            ok: true, 
            json: async () => [] 
        } as unknown as Response));

        renderPurchase({ productId: '999' });

        await waitFor(() => expect(screen.getByText('Product not found')).toBeInTheDocument());
    });

    it('renders error when response is not ok', async () => {
        find.mockImplementation(() => Promise.resolve({ 
            ok: false, 
            status: 500 
        } as unknown as Response));

        renderPurchase();

        await waitFor(() => expect(screen.getByText(/Failed to load product/i)).toBeInTheDocument());
    });

    it('renders error on network failure', async () => {
        find.mockImplementation(() => Promise.reject(new Error('Network error')));

        renderPurchase();

        await waitFor(() => expect(screen.getByText(/Failed to load product/i)).toBeInTheDocument());
    });

    it('shows confirm purchase button with product price', async () => {
        find.mockImplementation(() => Promise.resolve({ 
            ok: true, 
            json: async () => [mockProduct] 
        } as unknown as Response));

        renderPurchase();

        await waitFor(() => expect(screen.getByText('Confirm Purchase')).toBeInTheDocument());
        expect(screen.getAllByText(/5\.50/)).toHaveLength(2);
    });

    it('calls createPurchase with correct data on button click', async () => {
        find.mockImplementation(() => Promise.resolve({ 
            ok: true, 
            json: async () => [mockProduct] 
        } as unknown as Response));
        createPurchase.mockImplementation(() => Promise.resolve({ ok: true } as unknown as Response));

        renderPurchase();

        await waitFor(() => expect(screen.getByText('Confirm Purchase')).toBeInTheDocument());
        
        const button = screen.getByText('Confirm Purchase');
        button.click();

        await waitFor(() => expect(createPurchase).toHaveBeenCalledWith({ productId: '1' }));
    });

    it('shows success message on successful purchase', async () => {
        find.mockImplementation(() => Promise.resolve({ 
            ok: true, 
            json: async () => [mockProduct] 
        } as unknown as Response));
        createPurchase.mockImplementation(() => Promise.resolve({ ok: true } as unknown as Response));

        renderPurchase();

        await waitFor(() => expect(screen.getByText('Confirm Purchase')).toBeInTheDocument());
        
        screen.getByText('Confirm Purchase').click();

        await waitFor(() => expect(screen.getByText('Purchase successful!')).toBeInTheDocument());
    });

    it('shows error message on failed purchase', async () => {
        find.mockImplementation(() => Promise.resolve({ 
            ok: true, 
            json: async () => [mockProduct] 
        } as unknown as Response));
        createPurchase.mockImplementation(() => Promise.resolve({ 
            ok: false, 
            text: async () => 'Insufficient stock' 
        } as unknown as Response));

        renderPurchase();

        await waitFor(() => expect(screen.getByText('Confirm Purchase')).toBeInTheDocument());
        
        screen.getByText('Confirm Purchase').click();

        await waitFor(() => expect(screen.getByText(/Purchase failed: Insufficient stock/i)).toBeInTheDocument());
    });

    it('shows error message on purchase network error', async () => {
        find.mockImplementation(() => Promise.resolve({ 
            ok: true, 
            json: async () => [mockProduct] 
        } as unknown as Response));
        createPurchase.mockImplementation(() => Promise.reject(new Error('Network error')));

        renderPurchase();

        await waitFor(() => expect(screen.getByText('Confirm Purchase')).toBeInTheDocument());
        
        screen.getByText('Confirm Purchase').click();

        await waitFor(() => expect(screen.getByText(/Purchase failed: Network error/i)).toBeInTheDocument());
    });

    it('disables button during purchase', async () => {
        find.mockImplementation(() => Promise.resolve({ 
            ok: true, 
            json: async () => [mockProduct] 
        } as unknown as Response));
        let resolvePurchase: (value: Response) => void;
        createPurchase.mockImplementation(() => new Promise((resolve) => {
            resolvePurchase = resolve;
        }));

        renderPurchase();

        await waitFor(() => expect(screen.getByText('Confirm Purchase')).toBeInTheDocument());
        
        const button = screen.getByText('Confirm Purchase');
        button.click();

        await waitFor(() => expect(screen.getByText('Processing...')).toBeInTheDocument());
        expect(button).toBeDisabled();

        resolvePurchase!({ ok: true } as unknown as Response);
        await waitFor(() => expect(screen.getByText('Confirm Purchase')).toBeInTheDocument());
    });

    it('shows back to products link', async () => {
        find.mockImplementation(() => Promise.resolve({ 
            ok: true, 
            json: async () => [mockProduct] 
        } as unknown as Response));

        renderPurchase();

        await waitFor(() => expect(screen.getByText('Cancel')).toBeInTheDocument());
        expect(screen.getByText('Cancel')).toHaveAttribute('href', '/products');
    });
});