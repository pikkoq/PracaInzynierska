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
                setError('Failed to download friends list');
            }
        } catch (error) {
            setError('An error occurred while downloading friends');
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
                setError('Failed to remove friend');
            }
        } catch (error) {
            setError('An error occurred while deleting a friend');
        } finally {
            setProcessingDelete(null);
        }
    };

    if (loading) {
        return <div className="loading">Loading friends...</div>;
    }

    if (error) {
        return <div className="error-message">{error}</div>;
    }

    if (friends.length === 0) {
        return <div className="no-friends">You don't have any friends yet</div>;
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
                            Friends since: {new Date(friend.dateAdded).toLocaleDateString()}
                        </p>
                    </div>
                    <div className="friend-actions">
                        <button className="view-profile-button">
                            See profile
                        </button>
                        <button 
                            className="remove-friend-button"
                            onClick={() => handleDeleteFriend(friend.id)}
                            disabled={processingDelete === friend.id}
                        >
                            {processingDelete === friend.id ? 'Removing...' : 'Remove friend'}
                        </button>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default FriendsList; 