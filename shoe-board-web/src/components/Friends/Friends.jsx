import React, { useState } from "react";
import TopNavbar from '../Navigation/TopNavbar';
import Navigation from "../Navigation/Navigation";
import FriendsList from "./FriendsList";
import FriendRequestsModal from "./FriendRequestsModal";
import AddFriendModal from "./AddFriendModal";
import './Friends.scss';

const Friends = () => {
    const [showInvites, setShowInvites] = useState(false);
    const [showAddFriend, setShowAddFriend] = useState(false);
    const [refreshFriendsList, setRefreshFriendsList] = useState(0);

    const handleFriendsUpdate = () => {
        setRefreshFriendsList(prev => prev + 1);
    };

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
                            <button 
                                className="friend-action-button invites-button"
                                onClick={() => setShowInvites(true)}
                            >
                                Invites
                            </button>
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