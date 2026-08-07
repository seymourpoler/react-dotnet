export type UserLoginRequest = {
  email: string;
  password: string;
};

export async function login(request: UserLoginRequest) : Promise<Response> {
    const url = '/api/v0/login';
    
    return fetch(url, {  
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({ email: request.email, password: request.password }) 
    });
}

