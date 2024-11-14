import React, { useState, useEffect } from 'react';
import { getPopularShoes, getCatalogShoeDetails } from '../../services/api';
import './PopularShoes.scss';
import ShoeDetailsModal from '../Navigation/ShoeDetailsModal';

const PopularShoes = () => {
  const [popularShoes, setPopularShoes] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState('');
  const [selectedShoe, setSelectedShoe] = useState(null);
  const [showDetails, setShowDetails] = useState(false);
  const [isLoadingDetails, setIsLoadingDetails] = useState(false);
  const [isMobile, setIsMobile] = useState(window.innerWidth <= 768);
  const [isNavOpen, setIsNavOpen] = useState(false);

  useEffect(() => {
    const handleResize = () => {
      setIsMobile(window.innerWidth <= 768);
    };

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  useEffect(() => {
    const fetchPopularShoes = async () => {
      try {
        setIsLoading(true);
        const response = await getPopularShoes();
        if (response.success) {
          setPopularShoes(response.data);
        } else {
          setError('Failed to fetch popular shoes');
        }
      } catch (error) {
        console.error('Error fetching popular shoes:', error);
        setError('Error loading popular shoes');
      } finally {
        setIsLoading(false);
      }
    };

    fetchPopularShoes();
  }, []);

  const handleShoeClick = async (shoeId) => {
    try {
      setIsLoadingDetails(true);
      const response = await getCatalogShoeDetails(shoeId);
      if (response.success) {
        setSelectedShoe(response.data);
        setShowDetails(true);
      }
    } catch (error) {
      console.error('Error fetching shoe details:', error);
    } finally {
      setIsLoadingDetails(false);
    }
  };

  const toggleNavVisibility = () => {
    setIsNavOpen(!isNavOpen);
  };

  if (isLoading) {
    return (
      <aside className={`popular-shoes ${isNavOpen ? 'open' : ''} ${isMobile ? 'mobile' : ''}`}>
        <h2>Top Shoes</h2>
        <div className="loading">Loading...</div>
      </aside>
    );
  }

  if (error) {
    return (
      <aside className={`popular-shoes ${isNavOpen ? 'open' : ''} ${isMobile ? 'mobile' : ''}`}>
        <h2>Top Shoes</h2>
        <div className="error-message">{error}</div>
      </aside>
    );
  }

  return (
    <>
      {isMobile && (
        <button className="toggle-nav-button" onClick={toggleNavVisibility}>
          {isNavOpen ? '▲' : '▼'}
        </button>
      )}

      <aside className={`popular-shoes ${isNavOpen ? 'open' : ''} ${isMobile ? 'mobile' : ''}`}>
        <h2>Top Shoes</h2>
        <ul className="popular-shoes-list">
          {popularShoes.map((shoe) => (
            <li 
              key={shoe.id} 
              className="popular-shoe-item"
              onClick={() => handleShoeClick(shoe.id)}
            >
              {shoe.title}
            </li>
          ))}
        </ul>
      </aside>

      {showDetails && (
        <ShoeDetailsModal 
          shoe={selectedShoe}
          isLoading={isLoadingDetails}
          onClose={() => setShowDetails(false)}
        />
      )}
    </>
  );
};

export default PopularShoes;
