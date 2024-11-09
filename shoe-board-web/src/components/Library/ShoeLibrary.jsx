import React, { useState, useEffect } from 'react';
import { getUserShoes, deleteUserShoe, updateUserShoe } from '../../services/api';
import ShoeCard from './ShoeCard';
import './ShoeLibrary.scss';

const ShoeLibrary = () => {
    const [shoes, setShoes] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        fetchShoes();
    }, []);

    const fetchShoes = async () => {
        try {
            setIsLoading(true);
            const response = await getUserShoes();
            if (response.success) {
                setShoes(response.data);
            }
        } catch (error) {
            setError('Failed to fetch shoes');
            console.error('Error fetching shoes:', error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleDelete = async (shoeId) => {
        try {
            const response = await deleteUserShoe(shoeId);
            if (response.success) {
                setShoes(shoes.filter(shoe => shoe.id !== shoeId));
            }
        } catch (error) {
            setError('Failed to delete shoe');
            console.error('Error deleting shoe:', error);
        }
    };

    const handleUpdate = async (shoeId, updatedData) => {
        try {
            const response = await updateUserShoe(shoeId, updatedData);
            if (response.success) {
                setShoes(shoes.map(shoe => 
                    shoe.id === shoeId ? { ...shoe, ...updatedData } : shoe
                ));
                return response;
            }
        } catch (error) {
            setError('Failed to update shoe');
            console.error('Error updating shoe:', error);
            throw error;
        }
    };

    if (isLoading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="shoe-library">
            {error && <div className="error-message">{error}</div>}
            <h1>My Shoe Collection</h1>
            <div className="shoe-grid">
                {shoes.map(shoe => (
                    <ShoeCard
                        key={shoe.id}
                        shoe={shoe}
                        onDelete={handleDelete}
                        onUpdate={handleUpdate}
                    />
                ))}
            </div>
        </div>
    );
};

export default ShoeLibrary; 