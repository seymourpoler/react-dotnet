export async function login(email: string, password: string ) : Promise<Response> {
    return fetch('/api/v0/login', {
        method: 'POST',
        body: JSON.stringify({ email, password }) 
    });
}

