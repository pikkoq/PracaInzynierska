import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import { FaChevronLeft, FaChevronRight } from 'react-icons/fa';
import { searchShoes } from '../../services/api';
import SearchResultCard from './SearchResultCard';
import Navigation from '../Navigation/Navigation';
import TopNavbar from '../Navigation/TopNavbar';
import './SearchResults.css';

const SearchResults = () => {
    const [results, setResults] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalCount, setTotalCount] = useState(0);
    const location = useLocation();
    
    const searchTerm = new URLSearchParams(location.search).get('query') || '';

    useEffect(() => {
        setCurrentPage(1);
    }, [searchTerm]);

    useEffect(() => {
        fetchResults();
    }, [searchTerm, currentPage]);

    const fetchResults = async () => {
        if (!searchTerm) return;
        
        try {
            setIsLoading(true);
            const response = await searchShoes(searchTerm, currentPage);
            if (response.success) {
                setResults(response.data);
                setTotalPages(response.totalPages);
                setTotalCount(response.totalCount);
            }
        } catch (error) {
            setError('Failed to fetch search results');
            console.error('Error fetching search results:', error);
        } finally {
            setIsLoading(false);
        }
    };

    const handlePageChange = (page) => {
        setCurrentPage(page);
        window.scrollTo(0, 0);
    };

    const getPageNumbers = () => {
        const pages = [];
        if (totalPages <= 7) {
            for (let i = 1; i <= totalPages; i++) {
                pages.push(i);
            }
        } else {
            pages.push(1);
            
            if (currentPage <= 3) {
                for (let i = 2; i <= 5; i++) {
                    pages.push(i);
                }
                pages.push('...');
            } else if (currentPage >= totalPages - 2) {
                pages.push('...');
                for (let i = totalPages - 4; i < totalPages; i++) {
                    pages.push(i);
                }
            } else {
                pages.push('...');
                for (let i = currentPage - 1; i <= currentPage + 1; i++) {
                    pages.push(i);
                }
                pages.push('...');
            }
            
            pages.push(totalPages);
        }
        return pages;
    };

    return (
        <div className="search-results-container">
            <TopNavbar />
            <div className="main-content">
                <div className="left-nav">
                    <Navigation />
                </div>
                <div className="main-content-container">
                    {isLoading ? (
                        <div className="loading">Loading...</div>
                    ) : error ? (
                        <div className="error-message">{error}</div>
                    ) : (
                        <>
                            <h1>Search Results for "{searchTerm}"</h1>
                            <div className="results-grid">
                                {results.map(shoe => (
                                    <SearchResultCard key={shoe.id} shoe={shoe} />
                                ))}
                            </div>
                            {totalPages > 1 && (
                                <div className="pagination-container">
                                    <div className="total-results">
                                        Total Results: {totalCount}
                                    </div>
                                    <div className="pagination">
                                        <button 
                                            className="page-button nav-button"
                                            onClick={() => handlePageChange(currentPage - 1)}
                                            disabled={currentPage === 1}
                                        >
                                            <FaChevronLeft />
                                        </button>
                                        {getPageNumbers().map((page, index) => (
                                            <React.Fragment key={index}>
                                                {page === '...' ? (
                                                    <span className="page-ellipsis">...</span>
                                                ) : (
                                                    <button
                                                        onClick={() => handlePageChange(page)}
                                                        className={`page-button ${currentPage === page ? 'active' : ''}`}
                                                    >
                                                        {page}
                                                    </button>
                                                )}
                                            </React.Fragment>
                                        ))}
                                        <button 
                                            className="page-button nav-button"
                                            onClick={() => handlePageChange(currentPage + 1)}
                                            disabled={currentPage === totalPages}
                                        >
                                            <FaChevronRight />
                                        </button>
                                    </div>
                                </div>
                            )}
                        </>
                    )}
                </div>
            </div>
        </div>
    );
};

export default SearchResults;