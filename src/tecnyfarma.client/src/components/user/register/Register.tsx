import { useState } from 'react';
import { register } from "./RegisterService";

export function Register() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [message, setMessage] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    async function handleRegister() {
        setLoading(true);
        setMessage(null);
        try {
            const response = await register({ email, password })
            if (response.ok) {
                setMessage('Registration successful!');
                setEmail('');
                setPassword('');
            } else {
                const error = await response.text();
                setMessage('Registration failed: ' + error);
            }
        } catch (err) {
            setMessage('Registration failed: ' + (err as Error).message);
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
            <h2>Register</h2>
            <div>
                <label>Email: <input type="email" value={email} onChange={onEmailChangeHandler} required /></label>
            </div>
            <div>
                <label>Password: <input type="password" value={password} onChange={onPasswordChangeHandler} required /></label>
            </div>
            <button type="button" onClick={handleRegister} disabled={loading}>Register</button>
            {message && <div style={{ marginTop: 8 }}>{message}</div>}
        </div>
    );
}

