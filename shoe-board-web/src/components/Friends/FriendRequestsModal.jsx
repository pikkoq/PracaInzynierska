import React, { useState, useEffect } from 'react';
import { getFriendRequests, getSentFriendRequests, acceptFriendRequest, declineFriendRequest, rejectFriendRequest } from '../../services/api';
import { useNavigate } from 'react-router-dom';
import './FriendRequestsModal.scss';

const FriendRequestsModal = ({ onClose, onFriendsUpdate }) => {
    const navigate = useNavigate();
    const [activeTab, setActiveTab] = useState('received');
    const [receivedRequests, setReceivedRequests] = useState([]);
    const [sentRequests, setSentRequests] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [processingRequest, setProcessingRequest] = useState(null);

    const fetchRequests = async () => {
        setLoading(true);
        try {
            if (activeTab === 'received') {
                const response = await getFriendRequests();
                if (response.success) {
                    setReceivedRequests(response.data);
                }
            } else {
                const response = await getSentFriendRequests();
                if (response.success) {
                    setSentRequests(response.data);
                }
            }
        } catch (error) {
            setError('An error occurred while downloading invitations');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchRequests();
    }, [activeTab]);

    const handleAcceptRequest = async (requestId) => {
        setProcessingRequest(requestId);
        try {
            const response = await acceptFriendRequest(requestId);
            if (response.success) {
                await fetchRequests();
                onFriendsUpdate();
            } else {
                setError('Failed to accept invitation');
            }
        } catch (error) {
            setError('An error occurred while accepting the invitation');
        } finally {
            setProcessingRequest(null);
        }
    };

    const handleCancelRequest = async (requestId) => {
        setProcessingRequest(requestId);
        try {
            const response = await declineFriendRequest(requestId);
            if (response.success) {
                await fetchRequests();
            } else {
                setError('Failed to cancel invitation');
            }
        } catch (error) {
            setError('An error occurred while cancelling the invitation');
        } finally {
            setProcessingRequest(null);
        }
    };

    const handleRejectRequest = async (requestId) => {
        setProcessingRequest(requestId);
        try {
            const response = await rejectFriendRequest(requestId);
            if (response.success) {
                await fetchRequests();
            } else {
                setError('Failed to decline the invitation');
            }
        } catch (error) {
            setError('An error occurred while declining an invitation');
        } finally {
            setProcessingRequest(null);
        }
    };

    const handleViewProfile = (username) => {
        navigate(`/profile/${username}`);
        onClose();
    };

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="friend-requests-modal" onClick={e => e.stopPropagation()}>
                <div className="modal-header">
                    <h2>Friend invites</h2>
                    <button className="close-button" onClick={onClose}>&times;</button>
                </div>
                
                <div className="tabs">
                    <button 
                        className={`tab-button ${activeTab === 'received' ? 'active' : ''}`}
                        onClick={() => setActiveTab('received')}
                    >
                        Received
                    </button>
                    <button 
                        className={`tab-button ${activeTab === 'sent' ? 'active' : ''}`}
                        onClick={() => setActiveTab('sent')}
                    >
                        Sent
                    </button>
                </div>

                <div className="requests-content">
                    {loading ? (
                        <div className="loading">Loading invitations...</div>
                    ) : error ? (
                        <div className="error-message">{error}</div>
                    ) : (
                        <div className="requests-list">
                            {activeTab === 'received' ? (
                                receivedRequests.length === 0 ? (
                                    <p className="no-requests">No invitations received</p>
                                ) : (
                                    receivedRequests.map((request) => (
                                        <div key={request.id} className="request-card">
                                            <div className="request-avatar">
                                                <img 
                                                    src={request.userAvatar} 
                                                    alt={`${request.username}'s avatar`} 
                                                />
                                            </div>
                                            <div className="request-info">
                                                <h3 
                                                    onClick={() => handleViewProfile(request.username)}
                                                    style={{ cursor: 'pointer' }}
                                                >
                                                    {request.username}
                                                </h3>
                                                <p>Sent date: {new Date(request.requestDate).toLocaleDateString()}</p>
                                            </div>
                                            <div className="request-actions">
                                                <button 
                                                    className="accept-button"
                                                    onClick={() => handleAcceptRequest(request.id)}
                                                    disabled={processingRequest === request.id}
                                                >
                                                    {processingRequest === request.id ? 'Accepting...' : 'Accept'}
                                                </button>
                                                <button 
                                                    className="reject-button"
                                                    onClick={() => handleRejectRequest(request.id)}
                                                    disabled={processingRequest === request.id}
                                                >
                                                    {processingRequest === request.id ? 'Declining...' : 'Decline'}
                                                </button>
                                            </div>
                                        </div>
                                    ))
                                )
                            ) : (
                                sentRequests.length === 0 ? (
                                    <p className="no-requests">No invitations sent</p>
                                ) : (
                                    sentRequests.map((request) => (
                                        <div key={request.id} className="request-card">
                                            <div className="request-avatar">
                                                <img 
                                                    src={request.userAvatar} 
                                                    alt={`${request.username}'s avatar`} 
                                                />
                                            </div>
                                            <div className="request-info">
                                                <h3 
                                                    onClick={() => handleViewProfile(request.username)}
                                                    style={{ cursor: 'pointer' }}
                                                >
                                                    {request.username}
                                                </h3>
                                                <p>Sent date: {new Date(request.requestDate).toLocaleDateString()}</p>
                                            </div>
                                            <div className="request-actions">
                                                <button 
                                                    className="cancel-button"
                                                    onClick={() => handleCancelRequest(request.id)}
                                                    disabled={processingRequest === request.id}
                                                >
                                                    {processingRequest === request.id ? 'Canceling...' : 'Cancel invite'}
                                                </button>
                                            </div>
                                        </div>
                                    ))
                                )
                            )}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default FriendRequestsModal; 