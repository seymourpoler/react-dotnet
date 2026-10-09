import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { find } from '../product/ProductService';
import type { Product } from '../product/Product';
import { createPurchase } from './PurchaseService';

export function Purchase() {
    const { productId } = useParams<{ productId: string }>();
    const [product, setProduct] = useState<Product | null>(null);
    const [message, setMessage] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!productId) return;

        find().then(async (response) => {
            if (!response.ok) {
                setError(`Failed to load product (${response.status})`);
                return;
            }
            const products = (await response.json()) as Product[];
            const product = products.find(p => p.id === productId);
            if (product) {
                setProduct(product);
            } else {
                setError('Product not found');
            }
        }).catch(() => setError('Failed to load product'));
    }, [productId]);

    async function handlePurchase() {
        if (!productId) return;
        setLoading(true);
        setMessage(null);
        try {
            const response = await createPurchase({ productId });
            if (response.ok) {
                setMessage('Purchase successful!');
                return;
            }
            const errorText = await response.text();
            setMessage('Purchase failed: ' + errorText);
        } catch (err) {
            setMessage('Purchase failed: ' + (err as Error).message);
        } finally {
            setLoading(false);
        }
    }

    if (error !== null) {
        return (
            <div>
                <h1>Purchase Product</h1>
                <p>{error}</p>
                <Link to="/products">Back to Products</Link>
            </div>
        );
    }

    if (product === null) {
        return (
            <div>
                <h1>Purchase Product</h1>
                <p><em>Loading...</em></p>
            </div>
        );
    }

    return (
        <div style={{ marginBottom: 24 }}>
            <h1>Purchase Product</h1>
            <div style={{ marginBottom: 16 }}>
                <h2>{product.name}</h2>
                <p><strong>Description:</strong> {product.description}</p>
                <p><strong>Price:</strong> ${product.price.toFixed(2)}</p>
            </div>

            <p><strong>Total:</strong> ${product.price.toFixed(2)}</p>

            <button type="button" onClick={handlePurchase} disabled={loading} style={{ marginRight: 8 }}>
                {loading ? 'Processing...' : 'Confirm Purchase'}
            </button>
            <Link to="/products" style={{ marginLeft: 8 }}>Cancel</Link>

            {message && <div style={{ marginTop: 8, color: message.includes('failed') ? 'red' : 'green' }}>{message}</div>}
        </div>
    );
}