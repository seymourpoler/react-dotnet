export type PurchaseRequest = {
    productId: string;
};

export async function createPurchase(request: PurchaseRequest): Promise<Response> {
    const url = '/api/v0/purchases';
    
    return fetch(url, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(request)
    });
}