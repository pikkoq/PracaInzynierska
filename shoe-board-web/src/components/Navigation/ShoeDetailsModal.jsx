import React from 'react';
import './ShoeDetailsModal.scss';

const ShoeDetailsModal = ({ shoe, isLoading, onClose }) => {
    if (!shoe && !isLoading) return null;

    return (
        <div className="shoe-details-modal" onClick={onClose}>
            <div className="shoe-details-content" onClick={e => e.stopPropagation()}>
                {isLoading ? (
                    <div className="loading">Loading details...</div>
                ) : (
                    <>
                        <h2>{shoe.title}</h2>
                        <div className="details-grid">
                            <img 
                                src={shoe.imageUrl}
                                alt={shoe.title} 
                                className="detail-image"
                            />
                            <p><strong>Brand:</strong> {shoe.brand}</p>
                            <p><strong>Series:</strong> {shoe.series}</p>
                            <p><strong>Model No:</strong> {shoe.model_No}</p>
                            <p><strong>Nickname:</strong> {shoe.nickname}</p>
                            <p><strong>Release Date:</strong> {new Date(shoe.releaseDate).toLocaleDateString('en-US', {
                                year: 'numeric',
                                month: 'long',
                                day: 'numeric'
                            })}</p>
                            <p><strong>Main Color:</strong> {shoe.mainColor}</p>
                            <p><strong>Colorway:</strong> {shoe.colorway}</p>
                            <p><strong>Price:</strong> ${shoe.price}</p>
                            <p><strong>Gender:</strong> {shoe.gender}</p>
                        </div>
                    </>
                )}
            </div>
        </div>
    );
};

export default ShoeDetailsModal; 