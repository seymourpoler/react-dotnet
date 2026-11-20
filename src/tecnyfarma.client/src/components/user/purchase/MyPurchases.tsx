import { useEffect, useState } from 'react';
import { findPurchases, type Purchase } from './PurchaseService';

export function MyPurchases() {
    const [purchases, setPurchases] = useState<Purchase[]>();
    const [error, setError] = useState<string>();

    useEffect(() => {
        findPurchases().then(async (response) => {
                if (!response.ok) {
                    setError(`Failed to load purchases (${response.status})`);
                    return;
                }
                const data = (await response.json()) as Purchase[];
                setPurchases(Array.isArray(data) ? data : []);
            }).catch(() => setError('Failed to load purchases'));
    }, []);

    if (error !== undefined) {
        return (
            <div>
                <h1>My Purchases</h1>
                <p>{error}</p>
            </div>
        );
    }

    return (
        <div>
            <h1>My Purchases</h1>
            {
                purchases === undefined
                ? <p><em>Loading...</em></p>
                : purchases.length === 0
                    ? <p><em>No purchases found.</em></p>
                    : <table className="table">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Date</th>
                                <th>Price</th>
                            </tr>
                        </thead>
                        <tbody>
                            {purchases.map(purchase =>
                                <tr key={purchase.id}>
                                    <td>{purchase.name}</td>
                                    <td>{new Date(purchase.createdAtUtc).toLocaleString()}</td>
                                    <td>{purchase.price.toFixed(2)}</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
            }
        </div>
    );
}
