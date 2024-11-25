import React, { useState, useEffect, useRef } from 'react';
import { FaEdit, FaCheck, FaTimes, FaShoppingCart, FaChevronLeft, FaChevronRight } from 'react-icons/fa';
import { getShoesToAccept, acceptShoe, declineShoe, editNewShoe } from '../../services/api';
import './AdminShoesModals.scss';

const AdminShoesModal = ({ onClose }) => {
    const [shoes, setShoes] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [editingShoe, setEditingShoe] = useState(null);
    const [editFormData, setEditFormData] = useState({});
    const [isPageLoading, setIsPageLoading] = useState(false);
    const modalBodyRef = useRef(null);

    useEffect(() => {
        fetchShoes();
    }, [currentPage]);

    const fetchShoes = async () => {
        try {
            setIsPageLoading(true);
            const response = await getShoesToAccept(currentPage);
            if (response.success) {
                setShoes(response.data);
                setTotalPages(response.totalPages);
            }
        } catch (error) {
            setError('Failed to fetch shoes');
        } finally {
            setIsPageLoading(false);
        }
    };

    const handleAccept = async (shoeId) => {
        try {
            const response = await acceptShoe(shoeId);
            if (response.success) {
                await fetchShoes();
            }
        } catch (error) {
            setError('Failed to accept shoe');
        }
    };

    const handleDecline = async (shoeId) => {
        if (window.confirm('Are you sure you want to decline this shoe?')) {
            try {
                const response = await declineShoe(shoeId);
                if (response.success) {
                    await fetchShoes();
                }
            } catch (error) {
                setError('Failed to decline shoe');
            }
        }
    };

    const handleEdit = (shoe) => {
        setEditingShoe(shoe);
        setEditFormData({
            model_No: shoe.model_No,
            title: shoe.title,
            nickname: shoe.nickname,
            brand: shoe.brand,
            series: shoe.series,
            gender: shoe.gender,
            imagePath: shoe.image_Path,
            shopUrl: shoe.shopUrl,
            mainColor: shoe.main_Color,
            colorway: shoe.colorway,
            price: shoe.price
        });
    };

    const handleEditSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await editNewShoe(editingShoe.id, editFormData);
            if (response.success) {
                setEditingShoe(null);
                await fetchShoes();
            }
        } catch (error) {
            setError('Failed to edit shoe');
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setEditFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handlePageChange = (newPage) => {
        setCurrentPage(newPage);
        if (modalBodyRef.current) {
            modalBodyRef.current.scrollTop = 0;
        }
    };

    if (loading) return null;

    return (
        <div className="admin-modal-overlay" onClick={onClose}>
            <div className="admin-modal-content" onClick={e => e.stopPropagation()}>
                <div className="admin-modal-header">
                    <h2>Shoes for Approval</h2>
                    <button className="close-button" onClick={onClose}>&times;</button>
                </div>
                <div className="admin-modal-body" ref={modalBodyRef}>
                    {error && <div className="admin-error-message">{error}</div>}
                    <div className={`shoes-grid ${isPageLoading ? 'loading' : ''}`}>
                        {shoes.map(shoe => (
                            <div key={shoe.id} className="shoe-card">
                                <div className="shoe-image">
                                    <img src={`https://localhost:7117${shoe.image_Path}`} alt={shoe.title} />
                                </div>
                                <div className="shoe-info">
                                    <h3>{shoe.title}</h3>
                                    <p>Brand: {shoe.brand}</p>
                                    <p>Model: {shoe.model_No}</p>
                                    <p>Added by: {shoe.userName}</p>
                                    <p>Price: ${shoe.price}</p>
                                    <div className="shoe-actions">
                                        <button 
                                            className="action-button accept"
                                            onClick={() => handleAccept(shoe.id)}
                                        >
                                            <FaCheck /> Accept
                                        </button>
                                        <button 
                                            className="action-button edit"
                                            onClick={() => handleEdit(shoe)}
                                        >
                                            <FaEdit /> Edit
                                        </button>
                                        <button 
                                            className="action-button decline"
                                            onClick={() => handleDecline(shoe.id)}
                                        >
                                            <FaTimes /> Decline
                                        </button>
                                    </div>
                                    {shoe.shopUrl && (
                                        <a 
                                            href={shoe.shopUrl} 
                                            target="_blank" 
                                            rel="noopener noreferrer"
                                            className="shop-link"
                                        >
                                            <FaShoppingCart /> Visit Shop
                                        </a>
                                    )}
                                </div>
                            </div>
                        ))}
                    </div>
                    {isPageLoading && <div className="page-loading-overlay">Loading...</div>}
                    {editingShoe && (
                        <div className="edit-form-overlay">
                            <div className="edit-form">
                                <h3>Edit Shoe</h3>
                                <form onSubmit={handleEditSubmit}>
                                    {Object.entries(editFormData).map(([key, value]) => (
                                        <div key={key} className="form-group">
                                            <label>{key.replace(/_/g, ' ').toUpperCase()}</label>
                                            <input
                                                type={key === 'price' ? 'number' : 'text'}
                                                name={key}
                                                value={value}
                                                onChange={handleInputChange}
                                            />
                                        </div>
                                    ))}
                                    <div className="form-actions">
                                        <button type="submit" className="save-button">Save</button>
                                        <button 
                                            type="button" 
                                            className="cancel-button"
                                            onClick={() => setEditingShoe(null)}
                                        >
                                            Cancel
                                        </button>
                                    </div>
                                </form>
                            </div>
                        </div>
                    )}
                    <div className="pagination">
                        <button 
                            onClick={() => handlePageChange(currentPage - 1)}
                            disabled={currentPage === 1 || isPageLoading}
                        >
                            <FaChevronLeft />
                        </button>
                        <span>Page {currentPage} of {totalPages}</span>
                        <button 
                            onClick={() => handlePageChange(currentPage + 1)}
                            disabled={currentPage === totalPages || isPageLoading}
                        >
                            <FaChevronRight />
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AdminShoesModal; 