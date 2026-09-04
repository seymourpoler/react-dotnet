export async function logout(): Promise<Response> {
    const url = '/api/v0/users/logout';

    return fetch(url, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'}
    });
}