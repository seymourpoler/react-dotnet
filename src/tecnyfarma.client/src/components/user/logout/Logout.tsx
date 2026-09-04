import {useState} from "react";
import { logout } from "./LogoutService";

export function Logout(){
    const [message, setMessage] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    async function handleLogout() {
        setLoading(true);
        setMessage(null);
        try {
            const response = await logout();
            if (response.ok) {
                setMessage('Logout successful!');
                return;
            }
            const error = await response.text();
            setMessage('Logout failed: ' + error);
        } catch (err) {
            setMessage('Logout failed: ' + (err as Error).message);
        } finally {
            setLoading(false);
        }
    }
    
    return (
        <div style={{ marginBottom: 24 }}>
            <h2>Are you sure?</h2>
            <button type="button" onClick={handleLogout} disabled={loading}>Login</button>
            {message && <div style={{ marginTop: 8 }}>{message}</div>}
        </div>
    );

}