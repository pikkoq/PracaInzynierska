import React, { useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import './Navigation.scss';
import homeIcon from '../../assets/home.png';
import friendsIcon from '../../assets/friends.png';
import profileIcon from '../../assets/profile.png';
import libraryIcon from '../../assets/library.png';

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
        <li>
          <Link className="nav-link" to="/home" onClick={() => setIsMenuOpen(false)}>
            <img src={homeIcon} alt="Home" className="nav-icon" />
            <span className='home-text'>Home</span>
          </Link>
        </li>
        <li>
          <Link className="nav-link" to="/friends" onClick={() => setIsMenuOpen(false)}>
            <img src={friendsIcon} alt="Friends" className="nav-icon" />
            <span>Friends</span>
          </Link>
        </li>
        <li>
          <Link className="nav-link" to="/profile" onClick={() => setIsMenuOpen(false)}>
            <img src={profileIcon} alt="Profile" className="nav-icon" />
            <span>Profile</span>
          </Link>
        </li>
        <li>
          <Link className="nav-link" to="/library" onClick={() => setIsMenuOpen(false)}>
            <img src={libraryIcon} alt="Library" className="nav-icon" />
            <span>Library</span>
          </Link>
        </li>
      </ul>
    </nav>
  );
};

export default Navigation;
