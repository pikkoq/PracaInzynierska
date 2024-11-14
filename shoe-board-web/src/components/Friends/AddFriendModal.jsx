import React, { useState } from 'react';
import { searchFriends, sendFriendRequest } from '../../services/api';
import './AddFriendModal.scss';

const AddFriendModal = ({ onClose }) => {
    const [searchTerm, setSearchTerm] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [searchPerformed, setSearchPerformed] = useState(false);
    const [processingRequest, setProcessingRequest] = useState(null);
    const [successMessage, setSuccessMessage] = useState('');

    const handleSearch = async (e) => {
        e.preventDefault();
        if (!searchTerm.trim()) return;

        setLoading(true);
        setError('');
        setSearchPerformed(true);
        setSuccessMessage('');

        try {
            const response = await searchFriends(searchTerm.trim());
            if (response.success) {
                setSearchResults(response.data);
            } else {
                setError('Failed to find that user.');
            }
        } catch (error) {
            setError('Error while searching user.');
        } finally {
            setLoading(false);
        }
    };

    const handleSendRequest = async (userId) => {
        setProcessingRequest(userId);
        setError('');
        setSuccessMessage('');

        try {
            const response = await sendFriendRequest(userId);
            if (response.success) {
                setSuccessMessage(`The invitation has been sent!`);
                setSearchResults(prevResults => 
                    prevResults.filter(user => user.id !== userId)
                );
            } else {
                setError('Failed to send invitation');
            }
        } catch (error) {
            setError('An error occurred while sending the invitation');
        } finally {
            setProcessingRequest(null);
        }
    };

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="add-friend-modal" onClick={e => e.stopPropagation()}>
                <div className="modal-header">
                    <h2>Add friend</h2>
                    <button className="close-button" onClick={onClose}>&times;</button>
                </div>

                <div className="search-section">
                    <form onSubmit={handleSearch}>
                        <input
                            type="text"
                            placeholder="Type username..."
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                            className="search-input"
                        />
                        <button type="submit" className="search-button">
                            Find
                        </button>
                    </form>
                </div>

                {successMessage && (
                    <div className="success-message">{successMessage}</div>
                )}

                <div className="search-results">
                    {loading ? (
                        <div className="loading">Searching...</div>
                    ) : error ? (
                        <div className="error-message">{error}</div>
                    ) : searchPerformed ? (
                        searchResults.length === 0 ? (
                            <div className="no-results">No users found</div>
                        ) : (
                            <div className="results-list">
                                {searchResults.map((user) => (
                                    <div key={user.id} className="user-card">
                                        <div className="user-avatar">
                                            <img 
                                                src={user.userAvatar} 
                                                alt={`${user.username}'s avatar`} 
                                            />
                                        </div>
                                        <div className="user-info">
                                            <h3>{user.username}</h3>
                                            <p>Dołączył: {new Date(user.dateJoined).toLocaleDateString()}</p>
                                        </div>
                                        <button 
                                            className="add-button"
                                            onClick={() => handleSendRequest(user.id)}
                                            disabled={processingRequest === user.id}
                                        >
                                            {processingRequest === user.id ? 'Sending...' : 'Add to friends'}
                                        </button>
                                    </div>
                                ))}
                            </div>
                        )
                    ) : null}
                </div>
            </div>
        </div>
    );
};

export default AddFriendModal; 