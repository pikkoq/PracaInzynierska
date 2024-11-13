import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import SearchBar from './SearchBar';
import logo from '../../assets/LogoJasnoSzare.png';
import { logout } from '../../services/api';
import './TopNavbar.scss';

const TopNavbar = () => {
    const navigate = useNavigate();
    const [username, setUsername] = useState('');

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            try {
                const payload = JSON.parse(atob(token.split('.')[1]));
                setUsername(payload.username);
            } catch (error) {
                console.error('Error decoding token:', error);
            }
        }
    }, []);

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    const handleLogoClick = () => {
        navigate('/home');
    };

    return (
        <nav className="top-navbar">
            <div className="logo-container" onClick={handleLogoClick} style={{ cursor: 'pointer' }}>
                <img src={logo} alt="ShoeBoard Logo" className="logo" />
            </div>
            <div className="search-bar-container">
                <SearchBar />
            </div>
            <div className="user-section">
                <span className="welcome-message">Hello,</span>
                <span className="username">{username}</span>
                <button onClick={handleLogout} className="logout-button">Logout</button>
            </div>
        </nav>
    );
};

export default TopNavbar; 