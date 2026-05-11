import { useState } from 'react';
import { login } from "./LoginService";

export function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [message, setMessage] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    async function handleLogin() {
        setLoading(true);
        setMessage(null);
        try {
            const response = await login( email, password);
            if (response.ok) {
                setMessage('Login successful!');
                setEmail('');
                setPassword('');
            } else {
                const error = await response.text();
                setMessage('Login failed: ' + error);
            }
        } catch (err) {
            setMessage('Login failed: ' + (err as Error).message);
        } finally {
            setLoading(false);
        }
    }

    function onEmailChangeHandler(event: React.ChangeEvent<HTMLInputElement>) {
        event.preventDefault();
        setEmail(event.target.value);
    }
    
    function onPasswordChangeHandler(event: React.ChangeEvent<HTMLInputElement>) {
        event.preventDefault();
        setPassword(event.target.value);
    }

    return (
        <div style={{ marginBottom: 24 }}>
            <h2>Login</h2>
            <div>
                <label>Email: <input type="email" value={email} onChange={onEmailChangeHandler} required /></label>
            </div>
            <div>
                <label>Password: <input type="password" value={password} onChange={onPasswordChangeHandler} required /></label>
            </div>
            <button type="button" onClick={handleLogin} disabled={loading}>Login</button>
            {message && <div style={{ marginTop: 8 }}>{message}</div>}
        </div>
    );
}

