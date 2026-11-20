export type Purchase = {
    id: string;
    email: string;
    name: string;
    price: number;
    createdAtUtc: string;
};

export async function findPurchases(): Promise<Response> {
    const url = '/api/v0/users/purchases';

    return await fetch(url, {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
    });
}
