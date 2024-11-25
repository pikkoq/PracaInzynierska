import React, { useState } from 'react';
import { registerNewShoe } from '../../services/api';
import './AddNewShoeModal.scss';

const AddNewShoeModal = ({ onClose }) => {
    const [formData, setFormData] = useState({
        gender: 'MENS',
        releaseDate: '',
        brand: '',
        price: '',
        colorway: '',
        nickname: '',
        imageFile: null,
        series: '',
        modelNo: '',
        title: '',
        shopUrl: '',
        mainColor: ''
    });
    const [imagePreview, setImagePreview] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleImageChange = (e) => {
        const file = e.target.files[0];
        if (file) {
            setFormData(prev => ({
                ...prev,
                imageFile: file
            }));
            setImagePreview(URL.createObjectURL(file));
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');
        setSuccess('');

        try {
            const response = await registerNewShoe(formData);
            if (response.success) {
                setSuccess('Shoe has been submitted for approval!');
                setTimeout(() => onClose(), 2000);
            }
        } catch (error) {
            setError('Failed to submit shoe. Please try again.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="add-shoe-modal-overlay" onClick={onClose}>
            <div className="add-shoe-modal-content" onClick={e => e.stopPropagation()}>
                <div className="add-shoe-modal-header">
                    <h2>Add New Shoe</h2>
                    <button className="close-button" onClick={onClose}>&times;</button>
                </div>
                <div className="add-shoe-modal-body">
                    {error && <div className="error-message">{error}</div>}
                    {success && <div className="success-message">{success}</div>}
                    
                    <form onSubmit={handleSubmit}>
                        <div className="form-grid">
                            <div className="form-group">
                                <label>Title*</label>
                                <input
                                    type="text"
                                    name="title"
                                    value={formData.title}
                                    onChange={handleInputChange}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Brand*</label>
                                <input
                                    type="text"
                                    name="brand"
                                    value={formData.brand}
                                    onChange={handleInputChange}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Model Number*</label>
                                <input
                                    type="text"
                                    name="modelNo"
                                    value={formData.modelNo}
                                    onChange={handleInputChange}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Series</label>
                                <input
                                    type="text"
                                    name="series"
                                    value={formData.series}
                                    onChange={handleInputChange}
                                />
                            </div>
                            <div className="form-group">
                                <label>Nickname</label>
                                <input
                                    type="text"
                                    name="nickname"
                                    value={formData.nickname}
                                    onChange={handleInputChange}
                                />
                            </div>
                            <div className="form-group">
                                <label>Gender*</label>
                                <select
                                    name="gender"
                                    value={formData.gender}
                                    onChange={handleInputChange}
                                    required
                                >
                                    <option value="MENS">Men's</option>
                                    <option value="WOMENS">Women's</option>
                                    <option value="UNISEX">Unisex</option>
                                </select>
                            </div>
                            <div className="form-group">
                                <label>Release Date*</label>
                                <input
                                    type="date"
                                    name="releaseDate"
                                    value={formData.releaseDate}
                                    onChange={handleInputChange}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Price*</label>
                                <input
                                    type="number"
                                    name="price"
                                    value={formData.price}
                                    onChange={handleInputChange}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Main Color*</label>
                                <input
                                    type="text"
                                    name="mainColor"
                                    value={formData.mainColor}
                                    onChange={handleInputChange}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Colorway*</label>
                                <input
                                    type="text"
                                    name="colorway"
                                    value={formData.colorway}
                                    onChange={handleInputChange}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label>Shop URL</label>
                                <input
                                    type="url"
                                    name="shopUrl"
                                    value={formData.shopUrl}
                                    onChange={handleInputChange}
                                />
                            </div>
                            <div className="form-group full-width">
                                <label>Image*</label>
                                <input
                                    type="file"
                                    accept="image/*"
                                    onChange={handleImageChange}
                                    required
                                />
                                {imagePreview && (
                                    <div className="image-preview">
                                        <img src={imagePreview} alt="Preview" />
                                    </div>
                                )}
                            </div>
                        </div>
                        <div className="form-actions">
                            <button 
                                type="submit" 
                                className="submit-button"
                                disabled={loading}
                            >
                                {loading ? 'Submitting...' : 'Submit for Approval'}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default AddNewShoeModal; 