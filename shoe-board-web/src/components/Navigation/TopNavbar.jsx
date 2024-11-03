import React from 'react';
import { useNavigate } from 'react-router-dom';
import SearchBar from './SearchBar';
import logo from '../../assets/LogoJasnoSzare.png';
import { logout } from '../../services/api';
import './TopNavbar.css';

const TopNavbar = () => {
    const navigate = useNavigate();

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
            <button onClick={handleLogout} className="logout-button">Logout</button>
        </nav>
    );
};

export default TopNavbar; 