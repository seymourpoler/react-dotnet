export type userRegisterRequest = {
      email: string;
      password: string;
    };
export async function register(request: userRegisterRequest) : Promise<Response> {
    const url = '/api/v0/register';
    
    return await fetch(url, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({email: request.email, password: request.password})
    });
}