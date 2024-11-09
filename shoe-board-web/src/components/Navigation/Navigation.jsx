import React from 'react';
import { Link } from 'react-router-dom';
import './Navigation.scss';

const Navigation = () => {
  return (
    <nav className="navigation">
      <ul className="nav-links">
        <li><Link to="/home">Home</Link></li>
        <li><Link to="/friends">Friends</Link></li>
        <li><Link to="/profile">Profile</Link></li>
        <li><Link to="/library">Library</Link></li>
      </ul>
    </nav>
  );
};

export default Navigation;
