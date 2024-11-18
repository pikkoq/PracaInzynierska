import React, { useState, useEffect } from "react";
import TopNavbar from '../Navigation/TopNavbar';
import Navigation from "../Navigation/Navigation";
import FriendsList from "./FriendsList";
import FriendRequestsModal from "./FriendRequestsModal";
import AddFriendModal from "./AddFriendModal";
import { getFriendRequests } from '../../services/api';
import './Friends.scss';

const Friends = () => {
    const [showInvites, setShowInvites] = useState(false);
    const [showAddFriend, setShowAddFriend] = useState(false);
    const [refreshFriendsList, setRefreshFriendsList] = useState(0);
    const [pendingRequests, setPendingRequests] = useState(0);

    const handleFriendsUpdate = () => {
        setRefreshFriendsList(prev => prev + 1);
        fetchPendingRequests();
    };

    const fetchPendingRequests = async () => {
        try {
            const response = await getFriendRequests();
            if (response.success) {
                setPendingRequests(response.data.length);
            }
        } catch (error) {
            console.error('Error fetching friend requests:', error);
        }
    };

    useEffect(() => {
        fetchPendingRequests();
    }, []);

    return (
        <div className="friends-container">
            <TopNavbar />
            <div className="main-content">
                <div className="left-nav">
                    <Navigation />
                </div>
                <div className="main-content-container">
                    <div className="friends-header">
                        <h1>Friends</h1>
                        <div className="friends-actions">
                            <div className="invite-button-container">
                                <button 
                                    className="friend-action-button invites-button"
                                    onClick={() => setShowInvites(true)}
                                >
                                    Invites
                                    {pendingRequests > 0 && (
                                        <span className="pending-requests-badge">
                                            {pendingRequests}
                                        </span>
                                    )}
                                </button>
                            </div>
                            <button 
                                className="friend-action-button add-friend-button"
                                onClick={() => setShowAddFriend(true)}
                            >
                                Add friend
                            </button>
                        </div>
                    </div>
                    <FriendsList refreshTrigger={refreshFriendsList} />
                    {showInvites && (
                        <FriendRequestsModal 
                            onClose={() => setShowInvites(false)} 
                            onFriendsUpdate={handleFriendsUpdate}
                        />
                    )}
                    {showAddFriend && <AddFriendModal onClose={() => setShowAddFriend(false)} />}
                </div>
            </div>
        </div>
    );
};

export default Friends; 