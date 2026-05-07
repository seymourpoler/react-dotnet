export type userRegisterRequest = {
      email: string;
      password: string;
    };
export async function register(request: userRegisterRequest) : Promise<Response> {
    const response = await fetch('/api/v0/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email: request.email, password: request.password })
    });
    return response;
}