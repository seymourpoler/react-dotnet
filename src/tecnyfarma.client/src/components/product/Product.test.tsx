import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import { Product } from './Product.tsx';
import * as ProductService from './ProductService';

vi.mock('./ProductService');

const find = vi.mocked(ProductService.find);

const products = [
    { id: '1', name: 'Aspirin', description: 'Painkiller', price: 5.5 },
    { id: '2', name: 'Paracetamol', description: 'Fever reducer', price: 3.25 },
];

function renderProduct() {
    return render(
        <MemoryRouter>
            <Product />
        </MemoryRouter>
    );
}

describe('Product', () => {
    beforeEach(() => {
        vi.clearAllMocks();
    });

    it('renders loading state initially', () => {
        find.mockReturnValue(new Promise(() => {}));
        
        renderProduct();
        
        expect(screen.getByText(/Loading.../i)).toBeInTheDocument();
    });

    it('renders products table after data loads', async () => {
        find.mockResolvedValue({ ok: true, json: async () => products } as unknown as Response);

        renderProduct();
        
        await waitFor(() => expect(screen.getByText('Aspirin')).toBeInTheDocument());
        expect(screen.getByText('Paracetamol')).toBeInTheDocument();
        expect(screen.getByText('Painkiller')).toBeInTheDocument();
        expect(screen.getByText('5.5')).toBeInTheDocument();
        expect(screen.getByText('3.25')).toBeInTheDocument();
    });

    it('renders no products message when the response is empty', async () => {
        find.mockResolvedValue({ ok: true, json: async () => [] } as unknown as Response);

        renderProduct();
        
        await waitFor(() => expect(screen.getByText(/No products found/i)).toBeInTheDocument());
    });

    it('renders error message when the response is not ok', async () => {
        find.mockResolvedValue({ ok: false, status: 500 } as unknown as Response);

        renderProduct();
        
        await waitFor(() => expect(screen.getByText(/Failed to load products/i)).toBeInTheDocument());
    });

    it('renders error message on network failure', async () => {
        find.mockRejectedValue(new Error('Network error'));

        renderProduct();
        
        await waitFor(() => expect(screen.getByText(/Failed to load products/i)).toBeInTheDocument());
    });
});