import React, { useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import './Navigation.scss';

const Navigation = () => {
  const [isMobile, setIsMobile] = useState(window.innerWidth <= 768);
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const navRef = useRef(null);

  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth <= 768);
      if (window.innerWidth > 768) {
        setIsMenuOpen(false);
      }
    };

    const handleClickOutside = (event) => {
      if (navRef.current && !navRef.current.contains(event.target)) {
        setIsMenuOpen(false);
      }
    };

    window.addEventListener('resize', handleResize);
    document.addEventListener('mousedown', handleClickOutside);

    return () => {
      window.removeEventListener('resize', handleResize);
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  return (
    <nav 
      ref={navRef}
      className={`navigation ${isMobile ? 'mobile' : ''} ${isMenuOpen ? 'open' : ''}`}
    >
      {isMobile && (
        <button className="burger-menu" onClick={toggleMenu}>
          <span></span>
          <span></span>
          <span></span>
        </button>
      )}
      <ul className={`nav-links ${isMenuOpen ? 'show' : ''}`}>
        <li><Link className="home-button" to="/home" onClick={() => setIsMenuOpen(false)}>Home</Link></li>
        <li><Link to="/friends" onClick={() => setIsMenuOpen(false)}>Friends</Link></li>
        <li><Link to="/profile" onClick={() => setIsMenuOpen(false)}>Profile</Link></li>
        <li><Link to="/library" onClick={() => setIsMenuOpen(false)}>Library</Link></li>
      </ul>
    </nav>
  );
};

export default Navigation;
