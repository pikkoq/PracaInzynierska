import React, { useState, useEffect } from 'react';
import { getFriends, deleteFriend } from '../../services/api';
import './FriendsList.scss';

const FriendsList = ({ refreshTrigger }) => {
    const [friends, setFriends] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [processingDelete, setProcessingDelete] = useState(null);

    const fetchFriends = async () => {
        try {
            const response = await getFriends();
            if (response.success) {
                setFriends(response.data);
            } else {
                setError('Nie udało się pobrać listy znajomych');
            }
        } catch (error) {
            setError('Wystąpił błąd podczas pobierania znajomych');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchFriends();
    }, [refreshTrigger]);

    const handleDeleteFriend = async (friendId) => {
        setProcessingDelete(friendId);
        try {
            const response = await deleteFriend(friendId);
            if (response.success) {
                await fetchFriends();
            } else {
                setError('Nie udało się usunąć znajomego');
            }
        } catch (error) {
            setError('Wystąpił błąd podczas usuwania znajomego');
        } finally {
            setProcessingDelete(null);
        }
    };

    if (loading) {
        return <div className="loading">Ładowanie znajomych...</div>;
    }

    if (error) {
        return <div className="error-message">{error}</div>;
    }

    if (friends.length === 0) {
        return <div className="no-friends">Nie masz jeszcze żadnych znajomych</div>;
    }

    return (
        <div className="friends-list">
            {friends.map((friend) => (
                <div key={friend.id} className="friend-card">
                    <div className="friend-avatar">
                        <img 
                            src={friend.userAvatar} 
                            alt={`${friend.username}'s avatar`} 
                        />
                    </div>
                    <div className="friend-info">
                        <h3 className="friend-username">{friend.username}</h3>
                        <p className="friend-date">
                            Znajomi od: {new Date(friend.dateAdded).toLocaleDateString()}
                        </p>
                    </div>
                    <div className="friend-actions">
                        <button className="view-profile-button">
                            Zobacz profil
                        </button>
                        <button 
                            className="remove-friend-button"
                            onClick={() => handleDeleteFriend(friend.id)}
                            disabled={processingDelete === friend.id}
                        >
                            {processingDelete === friend.id ? 'Usuwanie...' : 'Usuń znajomego'}
                        </button>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default FriendsList; 