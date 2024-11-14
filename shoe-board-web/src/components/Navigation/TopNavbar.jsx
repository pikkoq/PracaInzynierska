import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import SearchBar from './SearchBar';
import logo from '../../assets/LogoJasnoSzare.png';
import { logout } from '../../services/api';
import './TopNavbar.scss';
import { FaSearch } from 'react-icons/fa';

const TopNavbar = () => {
    const navigate = useNavigate();
    const [username, setUsername] = useState('');
    const [isMobile, setIsMobile] = useState(window.innerWidth <= 768);
    const [showMobileSearch, setShowMobileSearch] = useState(false);

    useEffect(() => {
        const handleResize = () => {
            const mobile = window.innerWidth <= 768;
            setIsMobile(mobile);
            if (!mobile) {
                setShowMobileSearch(false);
            }
        };

        window.addEventListener('resize', handleResize);
        return () => window.removeEventListener('resize', handleResize);
    }, []);

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

    const toggleMobileSearch = () => {
        setShowMobileSearch(!showMobileSearch);
    };

    return (
        <nav className={`top-navbar ${isMobile ? 'mobile' : ''}`}>
            <div className="logo-container" onClick={handleLogoClick}>
                <img src={logo} alt="ShoeBoard Logo" className="logo" />
            </div>

            {isMobile ? (
                <>
                    <button className="mobile-search-button" onClick={toggleMobileSearch}>
                        <FaSearch />
                    </button>
                    {showMobileSearch && (
                        <div className="mobile-search-overlay">
                            <div className="mobile-search-container">
                                <SearchBar />
                                <button 
                                    className="close-search-button"
                                    onClick={toggleMobileSearch}
                                >
                                    ×
                                </button>
                            </div>
                        </div>
                    )}
                </>
            ) : (
                <div className="search-bar-container">
                    <SearchBar />
                </div>
            )}

            <div className={`user-section ${isMobile ? 'mobile' : ''}`}>
                <div className="user-info">
                    <span className="welcome-message">Hello,</span>
                    <span className="username">{username}</span>
                </div>
                <button onClick={handleLogout} className="logout-button">
                    Logout
                </button>
            </div>
        </nav>
    );
};

export default TopNavbar; 