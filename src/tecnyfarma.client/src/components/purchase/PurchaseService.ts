export type PurchaseRequest = {
    productId: string;
};

export type PurchaseResponse = {
    id: string;
    productId: string;
    total: number;
    status: 'pending' | 'completed' | 'failed';
    createdAt: string;
};

export async function createPurchase(request: PurchaseRequest): Promise<Response> {
    const url = '/api/v0/purchases';
    
    return fetch(url, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(request)
    });
}