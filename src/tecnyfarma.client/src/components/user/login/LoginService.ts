export async function login(email: string, password: string ) : Promise<Response> {
    const url = '/api/v0/login';
    
    return fetch(url, {  
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({ email, password }) 
    });
}

