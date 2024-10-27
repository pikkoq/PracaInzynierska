import React, { useState } from 'react';
import { FaSearch } from 'react-icons/fa';
import { searchShoes } from '../../services/api';
import './SearchBar.css';

const SearchBar = () => {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);

  const handleSearch = async (e) => {
    e.preventDefault();
    try {
      const data = await searchShoes(query);
      setResults(data);
    } catch (error) {
      console.error('Error searching shoes:', error);
    }
  };

  return (
    <div className="search-bar">
      <form onSubmit={handleSearch}>
        <FaSearch className="search-icon" />
        <input
          type="text"
          placeholder="Search shoes..."
          value={query}
          onChange={(e) => setQuery(e.target.value)}
        />
      </form>
      {results.length > 0 && (
        <ul className="search-results">
          {results.map((shoe) => (
            <li key={shoe.id}>{shoe.name}</li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default SearchBar;
