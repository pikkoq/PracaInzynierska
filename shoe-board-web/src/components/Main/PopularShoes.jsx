import React, { useState, useEffect } from 'react';
import { getPopularShoes } from '../../services/api';
import './PopularShoes.css';

const PopularShoes = () => {
  const [popularShoes, setPopularShoes] = useState([]);

  useEffect(() => {
    const fetchPopularShoes = async () => {
      try {
        const data = await getPopularShoes();
        setPopularShoes(data);
      } catch (error) {
        console.error('Error fetching popular shoes:', error);
      }
    };

    fetchPopularShoes();
  }, []);

  return (
    <aside className="popular-shoes">
      <h2>Top 10 Shoes</h2>
      <ul>
        {popularShoes.map((shoe) => (
          <li key={shoe.id}>
            <span>{shoe.name}</span>
            <span>{shoe.count}</span>
          </li>
        ))}
      </ul>
    </aside>
  );
};

export default PopularShoes;
