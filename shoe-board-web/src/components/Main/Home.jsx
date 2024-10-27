import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Navigation from './Navigation';
import SearchBar from './SearchBar';
import PopularShoes from './PopularShoes';
import PostFeed from './PostFeed';
import { getFriendPosts, logout } from '../../services/api';
import logo from '../../assets/LogoJasnoSzare.png';
import './Home.css';
import { FaSearch } from 'react-icons/fa';

const Home = () => {
  const [posts, setPosts] = useState([]);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const checkSession = () => {
      const token = localStorage.getItem('token');
      const expiration = localStorage.getItem('tokenExpiration');
      if (!token || !expiration || Date.now() > parseInt(expiration)) {
        logout();
        navigate('/login');
      }
    };

    checkSession();
    fetchPosts();

    const intervalId = setInterval(checkSession, 60000);

    return () => clearInterval(intervalId);
  }, [navigate]);

  const fetchPosts = async () => {
    setLoading(true);
    try {
      const response = await getFriendPosts();
      if (response.success && Array.isArray(response.data)) {
        setPosts(response.data);
      } else {
        console.error('Unexpected response structure:', response);
        setPosts([]);
      }
    } catch (error) {
      console.error('Error fetching posts:', error);
      if (error.response && error.response.status === 401) {
        logout();
        navigate('/login');
      }
      setPosts([]);
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="home-container">
      <nav className="top-navbar">
        <div className="logo-container">
          <img src={logo} alt="ShoeBoard Logo" className="logo" />
        </div>
        <div className="search-bar-container">
          <SearchBar />
        </div>
        <button onClick={handleLogout} className="logout-button">Logout</button>
      </nav>
      <div className="main-content">
        <Navigation />
        <div className="scrollable-content">
          <PostFeed initialPosts={posts} loading={loading} />
        </div>
        <PopularShoes />
      </div>
    </div>
  );
};

export default Home;
