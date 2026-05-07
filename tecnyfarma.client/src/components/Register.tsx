import { useState } from 'react';

export function Register() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [message, setMessage] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    async function handleRegister() {
        setLoading(true);
        setMessage(null);
        try {
            const response = await fetch('/api/v0/register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ email, password })
            });
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

    function onEmailChangeHandler(e: React.ChangeEvent<HTMLInputElement>) {
        setEmail(e.target.value);
    }
    
    function onPasswordChangeHandler(e: React.ChangeEvent<HTMLInputElement>) {
        setPassword(e.target.value);
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

