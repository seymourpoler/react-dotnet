import './App.css';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import { About } from './components/About';
import { Home } from './components/Home';
import { ContactUs } from './components/ContactUs';
import { Weather } from './components/weather/Weather';
import { Register } from './components/user/register/Register';

export function App() {
    return (
        <BrowserRouter>
            <nav style={{ marginBottom: 24 }}>
                <Link to="/">Home</Link> |{' '}
                <Link to="/about">About</Link> |{' '}
                <Link to="/contact">Contact</Link> |{' '}
                <Link to="/weather">Weather</Link> |{' '}
                <Link to="/register">Register</Link> |{' '}
            </nav>

            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/about" element={<About />} />
                <Route path="/contact" element={<ContactUs />} />
                <Route path="/weather" element={<Weather />} />
                <Route path="/register" element={<Register />} />
            </Routes>
        </BrowserRouter>
    );
}