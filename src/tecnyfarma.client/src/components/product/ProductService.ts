export async function find(): Promise<Response>  {
    const url = "/api/v0/products";
    return await fetch(url, {
        method: 'GET',
        headers: {'Content-Type': 'application/json'}
    });
}