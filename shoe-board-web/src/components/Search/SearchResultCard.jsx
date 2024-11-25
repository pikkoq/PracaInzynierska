import React, { useState } from 'react';
import { FaPlus, FaSave, FaTimes } from 'react-icons/fa';
import { addShoeToUserCollection, getCatalogShoeDetails } from '../../services/api';
import ShoeDetailsModal from '../Navigation/ShoeDetailsModal';
import './SearchResultCard.scss';

const SearchResultCard = ({ shoe }) => {
    const [showAddModal, setShowAddModal] = useState(false);
    const [showDetails, setShowDetails] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState('');
    const [shoeDetails, setShoeDetails] = useState(null);
    const [shoeData, setShoeData] = useState({
        size: '40',
        comfortRating: '5',
        styleRating: '5',
        season: 'summer',
        review: ''
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setShoeData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async () => {
        try {
            setIsLoading(true);
            setError('');
            const response = await addShoeToUserCollection(shoe.id, shoeData);
            if (response.success) {
                setShowAddModal(false);
            } else {
                setError('Failed to add shoe to collection');
            }
        } catch (error) {
            setError('Error adding shoe to collection');
            console.error('Error:', error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleCardClick = async (e) => {
        if (e.target.closest('.add-button')) {
            return;
        }

        try {
            setIsLoading(true);
            const response = await getCatalogShoeDetails(shoe.id);
            if (response.success) {
                setShoeDetails(response.data);
                setShowDetails(true);
            }
        } catch (error) {
            console.error('Error fetching shoe details:', error);
        } finally {
            setIsLoading(false);
        }
    };

    const generateSizeOptions = () => {
        const sizes = [];
        for (let size = 35; size <= 50; size += 0.5) {
            sizes.push(size);
        }
        return sizes;
    };

    const ratingOptions = Array.from({ length: 11 }, (_, i) => i);
    const seasonOptions = ['Spring', 'Summer', 'Autumn', 'Winter'];

    const getImageUrl = (imageUrl) => {
        if (!imageUrl) return '';

        if (imageUrl.startsWith('https')) {
            return imageUrl;
        }

        if (imageUrl.startsWith('/uploads')) {
            return `https://localhost:7117${imageUrl}`;
        }

        return imageUrl;
    };

    return (
        <>
            <div className="search-result-card" onClick={handleCardClick}>
                <img src={getImageUrl(shoe.image_Url)} alt={shoe.title} className="shoe-image" />
                <div className="shoe-content">
                    <h3>{shoe.title}</h3>
                    <div className="shoe-details">
                        <p><strong>Brand:</strong> {shoe.brand}</p>
                        <p><strong>Gender:</strong> {shoe.gender}</p>
                        <p><strong>Price:</strong> ${shoe.price}</p>
                    </div>
                    <button 
                        className="add-button"
                        onClick={(e) => {
                            e.stopPropagation();
                            setShowAddModal(true);
                        }}
                    >
                        <FaPlus /> Add to Collection
                    </button>
                </div>
            </div>

            {showDetails && (
                <ShoeDetailsModal 
                    shoe={shoeDetails}
                    isLoading={isLoading}
                    onClose={() => setShowDetails(false)}
                />
            )}

            {showAddModal && (
                <div className="add-shoe-modal" onClick={() => setShowAddModal(false)}>
                    <div className="add-shoe-content" onClick={e => e.stopPropagation()}>
                        <h2>Add to Collection</h2>
                        {error && <div className="error-message">{error}</div>}
                        <div className="add-shoe-form">
                            <div className="form-group">
                                <label>Size:</label>
                                <select
                                    name="size"
                                    value={shoeData.size}
                                    onChange={handleInputChange}
                                    className="form-input"
                                >
                                    {generateSizeOptions().map(size => (
                                        <option key={size} value={size}>{size}</option>
                                    ))}
                                </select>
                            </div>

                            <div className="form-group">
                                <label>Comfort Rating:</label>
                                <select
                                    name="comfortRating"
                                    value={shoeData.comfortRating}
                                    onChange={handleInputChange}
                                    className="form-input"
                                >
                                    {ratingOptions.map(rating => (
                                        <option key={rating} value={rating}>{rating}</option>
                                    ))}
                                </select>
                            </div>

                            <div className="form-group">
                                <label>Style Rating:</label>
                                <select
                                    name="styleRating"
                                    value={shoeData.styleRating}
                                    onChange={handleInputChange}
                                    className="form-input"
                                >
                                    {ratingOptions.map(rating => (
                                        <option key={rating} value={rating}>{rating}</option>
                                    ))}
                                </select>
                            </div>

                            <div className="form-group">
                                <label>Season:</label>
                                <select
                                    name="season"
                                    value={shoeData.season}
                                    onChange={handleInputChange}
                                    className="form-input"
                                >
                                    {seasonOptions.map(season => (
                                        <option key={season} value={season.toLowerCase()}>{season}</option>
                                    ))}
                                </select>
                            </div>

                            <div className="form-group">
                                <label>Review:</label>
                                <textarea
                                    name="review"
                                    value={shoeData.review}
                                    onChange={handleInputChange}
                                    className="form-input"
                                    placeholder="Write your review..."
                                />
                            </div>

                            <div className="button-group">
                                <button 
                                    onClick={handleSubmit} 
                                    className="save-button"
                                    disabled={isLoading}
                                >
                                    <FaSave /> {isLoading ? 'Adding...' : 'Add to Collection'}
                                </button>
                                <button 
                                    onClick={() => setShowAddModal(false)} 
                                    className="cancel-button"
                                >
                                    <FaTimes /> Cancel
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </>
    );
};

export default SearchResultCard; 