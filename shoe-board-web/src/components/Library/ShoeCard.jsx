import React, { useState } from 'react';
import { FaTrash, FaSave, FaTimes } from 'react-icons/fa';
import { getShoeDetails } from '../../services/api';
import './ShoeCard.scss';
import ShoeImage from '../common/ShoeImage';

const ShoeCard = ({ shoe, onDelete, onUpdate }) => {
    const [showDetails, setShowDetails] = useState(false);
    const [isEditing, setIsEditing] = useState(false);
    const [shoeDetails, setShoeDetails] = useState(null);
    const [editedShoe, setEditedShoe] = useState(null);
    const [isLoading, setIsLoading] = useState(false);

    const handleCardClick = async () => {
        if (!showDetails) {
            setIsLoading(true);
            try {
                const response = await getShoeDetails(shoe.id);
                if (response.success) {
                    setShoeDetails(response.data);
                    setEditedShoe(response.data);
                }
            } catch (error) {
                console.error('Error fetching shoe details:', error);
            }
            setIsLoading(false);
        }
        setShowDetails(true);
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setEditedShoe(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async () => {
        try {
            const hasChanges = 
                editedShoe.size !== shoeDetails.size ||
                editedShoe.comfortRating !== shoeDetails.comfortRating ||
                editedShoe.styleRating !== shoeDetails.styleRating ||
                editedShoe.season !== shoeDetails.season ||
                editedShoe.review !== shoeDetails.review;

            if (!hasChanges) {
                setIsEditing(false);
                setShowDetails(false);
                return;
            }

            setIsLoading(true);

            try {
                const response = await onUpdate(shoe.id, {
                    size: editedShoe.size.toString(),
                    comfortRating: parseInt(editedShoe.comfortRating),
                    styleRating: parseInt(editedShoe.styleRating),
                    season: editedShoe.season.toLowerCase(),
                    review: editedShoe.review
                });

                if (response && response.success) {
                    setShoeDetails({
                        ...shoeDetails,
                        size: editedShoe.size,
                        comfortRating: editedShoe.comfortRating,
                        styleRating: editedShoe.styleRating,
                        season: editedShoe.season,
                        review: editedShoe.review
                    });
                    setIsEditing(false);
                    setShowDetails(false);
                }
            } catch (error) {
                console.error('Failed to update shoe:', error);
            }
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

    const ratingOptions = Array.from({ length: 11 }, (_, i) => i);
    const seasonOptions = ['Spring', 'Summer', 'Autumn', 'Winter'];

    return (
        <>
            <div className="shoe-card" onClick={handleCardClick}>
                <ShoeImage src={getImageUrl(shoe.image_Url)} alt={shoe.title} className="shoe-image" />
                <div className="shoe-content">
                    <h3>{shoe.title}</h3>
                    <div className="shoe-details">
                        <p><strong>Gender:</strong> {shoe.gender === 'MENS' ? 'Men\'s' : 
                                                 shoe.gender === 'WOMENS' ? 'Women\'s' : 'Unisex'}</p>
                        <p><strong>Size:</strong> {shoe.size}</p>
                        <p><strong>Added:</strong> {new Date(shoe.dateAdded).toLocaleDateString('en-US', {
                            year: 'numeric',
                            month: 'long',
                            day: 'numeric'
                        })}</p>
                    </div>
                    <div className="button-group">
                        <button 
                            onClick={(e) => {
                                e.stopPropagation();
                                onDelete(shoe.id);
                            }} 
                            className="delete-button"
                        >
                            <FaTrash /> Delete
                        </button>
                    </div>
                </div>
            </div>

            {showDetails && shoeDetails && (
                <div className="shoe-details-modal" onClick={() => setShowDetails(false)}>
                    <div className="shoe-details-content" onClick={e => e.stopPropagation()}>
                        {isLoading ? (
                            <div className="loading">Loading details...</div>
                        ) : (
                            <>
                                <h2>{shoeDetails.title}</h2>
                                <div className="details-grid">
                                    <div className="details-column">
                                        <p><strong>Brand:</strong> {shoeDetails.brand}</p>
                                        <p><strong>Series:</strong> {shoeDetails.series}</p>
                                        <p><strong>Model No:</strong> {shoeDetails.model_No}</p>
                                        <p><strong>Nickname:</strong> {shoeDetails.nickname}</p>
                                        <p><strong>Release Date:</strong> {new Date(shoeDetails.release_Date).toLocaleDateString()}</p>
                                        <p><strong>Main Color:</strong> {shoeDetails.main_Color}</p>
                                        <p><strong>Colorway:</strong> {shoeDetails.colorway}</p>
                                        <p><strong>Price:</strong> ${shoeDetails.price}</p>
                                    </div>

                                    {isEditing ? (
                                        <div className="edit-form">
                                            <select
                                                name="size"
                                                value={editedShoe.size}
                                                onChange={handleInputChange}
                                                className="edit-input"
                                            >
                                                {generateSizeOptions().map(size => (
                                                    <option key={size} value={size}>{size}</option>
                                                ))}
                                            </select>

                                            <select
                                                name="comfortRating"
                                                value={editedShoe.comfortRating}
                                                onChange={handleInputChange}
                                                className="edit-input"
                                            >
                                                {ratingOptions.map(rating => (
                                                    <option key={rating} value={rating}>{rating}</option>
                                                ))}
                                            </select>

                                            <select
                                                name="styleRating"
                                                value={editedShoe.styleRating}
                                                onChange={handleInputChange}
                                                className="edit-input"
                                            >
                                                {ratingOptions.map(rating => (
                                                    <option key={rating} value={rating}>{rating}</option>
                                                ))}
                                            </select>

                                            <select
                                                name="season"
                                                value={editedShoe.season}
                                                onChange={handleInputChange}
                                                className="edit-input"
                                            >
                                                {seasonOptions.map(season => (
                                                    <option key={season} value={season.toLowerCase()}>{season}</option>
                                                ))}
                                            </select>

                                            <textarea
                                                name="review"
                                                value={editedShoe.review}
                                                onChange={handleInputChange}
                                                className="edit-input"
                                                placeholder="Write your review..."
                                            />

                                            <div className="button-group">
                                                <button onClick={handleSubmit} className="save-button">
                                                    <FaSave /> Save
                                                </button>
                                                <button 
                                                    onClick={() => {
                                                        setIsEditing(false);
                                                        setEditedShoe(shoeDetails);
                                                    }} 
                                                    className="cancel-button"
                                                >
                                                    <FaTimes /> Cancel
                                                </button>
                                            </div>
                                        </div>
                                    ) : (
                                        <div className="user-ratings">
                                            <p><strong>Size:</strong> {shoeDetails.size}</p>
                                            <p><strong>Comfort Rating:</strong> {shoeDetails.comfortRating}/10</p>
                                            <p><strong>Style Rating:</strong> {shoeDetails.styleRating}/10</p>
                                            <p><strong>Season:</strong> {shoeDetails.season}</p>
                                            <p><strong>Review:</strong> {shoeDetails.review}</p>
                                            <button 
                                                onClick={() => setIsEditing(true)}
                                                className="edit-button"
                                            >
                                                Edit Details
                                            </button>
                                        </div>
                                    )}
                                </div>
                            </>
                        )}
                    </div>
                </div>
            )}
        </>
    );
};

export default ShoeCard; 