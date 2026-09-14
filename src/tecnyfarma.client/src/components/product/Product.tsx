import { useEffect, useState } from 'react';
import { find } from './ProductService';
import type { Product } from './ProductService';

export function Product() {
    const [products, setProducts] = useState<Product[]>();
    const [error, setError] = useState<string>();

    useEffect(() => {
        find()
            .then(async (response) => {
                if (!response.ok) {
                    setError(`Failed to load products (${response.status})`);
                    return;
                }
                const data = (await response.json()) as Product[];
                setProducts(Array.isArray(data) ? data : []);
            })
            .catch(() => setError('Failed to load products'));
    }, []);

    if (error !== undefined) {
        return (
            <div>
                <h1>Products Page</h1>
                <p>{error}</p>
            </div>
        );
    }

    return (
        <div>
            <h1>Products Page</h1>
            {
                products === undefined
                ? <p><em>Loading...</em></p>
                : products.length === 0
                    ? <p><em>No products found.</em></p>
                    : <table className="table">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Description</th>
                                <th>Price</th>
                            </tr>
                        </thead>
                        <tbody>
                            {
                                products.map(product =>
                                <tr key={product.id}>
                                    <td>{product.name}</td>
                                    <td>{product.description}</td>
                                    <td>{product.price}</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
            }
        </div>
    );
}