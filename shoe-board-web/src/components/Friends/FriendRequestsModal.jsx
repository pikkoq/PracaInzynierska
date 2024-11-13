import React, { useState, useEffect } from 'react';
import { getFriendRequests, getSentFriendRequests, acceptFriendRequest, declineFriendRequest, rejectFriendRequest } from '../../services/api';
import './FriendRequestsModal.scss';

const FriendRequestsModal = ({ onClose, onFriendsUpdate }) => {
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
            setError('Wystąpił błąd podczas pobierania zaproszeń');
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
                setError('Nie udało się zaakceptować zaproszenia');
            }
        } catch (error) {
            setError('Wystąpił błąd podczas akceptowania zaproszenia');
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
                setError('Nie udało się anulować zaproszenia');
            }
        } catch (error) {
            setError('Wystąpił błąd podczas anulowania zaproszenia');
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
                setError('Nie udało się odrzucić zaproszenia');
            }
        } catch (error) {
            setError('Wystąpił błąd podczas odrzucania zaproszenia');
        } finally {
            setProcessingRequest(null);
        }
    };

    return (
        <div className="modal-overlay">
            <div className="friend-requests-modal">
                <div className="modal-header">
                    <h2>Zaproszenia do znajomych</h2>
                    <button className="close-button" onClick={onClose}>&times;</button>
                </div>
                
                <div className="tabs">
                    <button 
                        className={`tab-button ${activeTab === 'received' ? 'active' : ''}`}
                        onClick={() => setActiveTab('received')}
                    >
                        Otrzymane
                    </button>
                    <button 
                        className={`tab-button ${activeTab === 'sent' ? 'active' : ''}`}
                        onClick={() => setActiveTab('sent')}
                    >
                        Wysłane
                    </button>
                </div>

                <div className="requests-content">
                    {loading ? (
                        <div className="loading">Ładowanie zaproszeń...</div>
                    ) : error ? (
                        <div className="error-message">{error}</div>
                    ) : (
                        <div className="requests-list">
                            {activeTab === 'received' ? (
                                receivedRequests.length === 0 ? (
                                    <p className="no-requests">Brak otrzymanych zaproszeń</p>
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
                                                <h3>{request.username}</h3>
                                                <p>Data wysłania: {new Date(request.requestDate).toLocaleDateString()}</p>
                                            </div>
                                            <div className="request-actions">
                                                <button 
                                                    className="accept-button"
                                                    onClick={() => handleAcceptRequest(request.id)}
                                                    disabled={processingRequest === request.id}
                                                >
                                                    {processingRequest === request.id ? 'Akceptowanie...' : 'Akceptuj'}
                                                </button>
                                                <button 
                                                    className="reject-button"
                                                    onClick={() => handleRejectRequest(request.id)}
                                                    disabled={processingRequest === request.id}
                                                >
                                                    {processingRequest === request.id ? 'Odrzucanie...' : 'Odrzuć'}
                                                </button>
                                            </div>
                                        </div>
                                    ))
                                )
                            ) : (
                                sentRequests.length === 0 ? (
                                    <p className="no-requests">Brak wysłanych zaproszeń</p>
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
                                                <h3>{request.username}</h3>
                                                <p>Data wysłania: {new Date(request.requestDate).toLocaleDateString()}</p>
                                            </div>
                                            <div className="request-actions">
                                                <button 
                                                    className="cancel-button"
                                                    onClick={() => handleCancelRequest(request.id)}
                                                    disabled={processingRequest === request.id}
                                                >
                                                    {processingRequest === request.id ? 'Anulowanie...' : 'Anuluj zaproszenie'}
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