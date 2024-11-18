import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { FaTrash, FaUserPlus, FaCheck, FaUserClock } from 'react-icons/fa';
import TopNavbar from '../Navigation/TopNavbar';
import Navigation from "../Navigation/Navigation";
import CommentsModal from '../Main/CommentsModal';
import { getUserProfile, sendFriendRequest, acceptFriendRequest } from '../../services/api';
import './UserProfile.scss';

const UserProfile = () => {
    const { userName } = useParams();
    const [userData, setUserData] = useState({
        userName: '',
        userProfileAvatar: '',
        bio: '',
        posts: [],
        isFriend: false,
        isRequestSent: false,
        isRequestRecived: false
    });
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');
    const [userNotFound, setUserNotFound] = useState(false);
    const [successMessage, setSuccessMessage] = useState('');
    const [showComments, setShowComments] = useState(false);
    const [selectedPostId, setSelectedPostId] = useState(null);
    const [isProcessingRequest, setIsProcessingRequest] = useState(false);

    useEffect(() => {
        fetchUserProfile();
    }, [userName]);

    const fetchUserProfile = async () => {
        try {
            setIsLoading(true);
            setUserNotFound(false);
            const response = await getUserProfile(userName);
            if (response.success) {
                setUserData(response.data);
            }
        } catch (error) {
            console.error('Error fetching user data:', error);
            if (error.response?.status === 404) {
                setUserNotFound(true);
            } else {
                setError('Failed to download user data.');
            }
        } finally {
            setIsLoading(false);
        }
    };

    const handleSendFriendRequest = async () => {
        if (isProcessingRequest) return;
        
        setIsProcessingRequest(true);
        try {
            const response = await sendFriendRequest(userData.id);
            if (response.success) {
                setUserData(prev => ({
                    ...prev,
                    isRequestSent: true
                }));
                setSuccessMessage('Friend request sent successfully!');
                setTimeout(() => setSuccessMessage(''), 3000);
            } else {
                setError('Failed to send friend request');
                setTimeout(() => setError(''), 3000);
            }
        } catch (error) {
            setError('Error sending friend request');
            setTimeout(() => setError(''), 3000);
        } finally {
            setIsProcessingRequest(false);
        }
    };

    const handleCommentAdded = (postId) => {
        setUserData(prev => ({
            ...prev,
            posts: prev.posts.map(post => {
                if (post.id === postId) {
                    return { ...post, commentsCount: (post.commentsCount || 0) + 1 };
                }
                return post;
            })
        }));
    };

    const handleAcceptRequest = async () => {
        if (isProcessingRequest) return;
        
        setIsProcessingRequest(true);
        try {
            const response = await acceptFriendRequest(userData.requestId);
            if (response.success) {
                setUserData(prev => ({
                    ...prev,
                    isRequestRecived: false,
                    isFriend: true
                }));
                setSuccessMessage('Friend request accepted!');
                setTimeout(() => setSuccessMessage(''), 3000);
            } else {
                setError('Failed to accept friend request');
                setTimeout(() => setError(''), 3000);
            }
        } catch (error) {
            setError('Error accepting friend request');
            setTimeout(() => setError(''), 3000);
        } finally {
            setIsProcessingRequest(false);
        }
    };

    const renderFriendshipButton = () => {
        if (localStorage.getItem('userId') === userData.id) {
            return null;
        }

        if (userData.isFriend) {
            return (
                <span className="friend-status">
                    Friends <FaCheck />
                </span>
            );
        }

        if (userData.isRequestSent) {
            return (
                <span className="pending-request-info">
                    <FaUserClock /> Request Sent
                </span>
            );
        }

        if (userData.isRequestRecived) {
            return (
                <button 
                    className="accept-friend-button"
                    onClick={handleAcceptRequest}
                    disabled={isProcessingRequest}
                >
                    {isProcessingRequest ? 'Accepting...' : 'Accept Request'} <FaUserPlus />
                </button>
            );
        }

        return (
            <button 
                className="add-friend-button"
                onClick={handleSendFriendRequest}
                disabled={isProcessingRequest}
            >
                <FaUserPlus /> {isProcessingRequest ? 'Sending...' : 'Add Friend'}
            </button>
        );
    };

    if (isLoading) {
        return <div className="profile-loading">Loading...</div>;
    }

    if (userNotFound) {
        return (
            <div className="profile-container">
                <TopNavbar />
                <div className="main-content">
                    <div className="left-nav">
                        <Navigation />
                    </div>
                    <div className="main-content-container">
                        <div className="user-not-found">
                            <h2>User not found</h2>
                            <p>The user "{userName}" does not exist.</p>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className="profile-container">
            <TopNavbar />
            <div className="main-content">
                <div className="left-nav">
                    <Navigation />
                </div>
                <div className="main-content-container">
                    <div className="profile-card">
                        <div className="profile-main-content">
                            <div className="profile-left-section">
                                <img 
                                    src={userData.userProfileAvatar || require('../../assets/DefaultUser.png')} 
                                    alt={userData.userName}
                                    className="profile-avatar"
                                />
                                <div className="profile-info">
                                    <h2 className="username">{userData.userName}</h2>
                                    <pre className="bio">{userData.bio || 'There is no bio yet...'}</pre>
                                </div>
                            </div>
                            {renderFriendshipButton()}
                        </div>
                    </div>

                    <div className="profile-posts-container">
                        <h2>{userData.userName}'s posts</h2>
                        {error && <div className="profile-error-message">{error}</div>}
                        {successMessage && <div className="profile-success-message">{successMessage}</div>}
                        {userData.posts.length === 0 ? (
                            <div className="profile-no-posts">No posts yet.</div>
                        ) : (
                            <div className="profile-posts">
                                {userData.posts.map((post) => (
                                    <div key={post.id} className="profile-post">
                                        <div className="profile-post-content">
                                            <div className="profile-post-header">
                                                <p className="profile-post-date">
                                                    {new Date(post.datePosted).toLocaleString()}
                                                </p>
                                            </div>
                                            <h3>{post.title}</h3>
                                            <pre className="profile-post-text">{post.content}</pre>
                                            <div className="profile-shoe-details">
                                                <div className="profile-details-box">
                                                    <p>Size: {post.size}</p>
                                                    <p>Comfort: {post.comfortRating} ⭐</p>
                                                    <p>Style: {post.styleRating} ⭐</p>
                                                    <p>Season: {post.season}</p>
                                                    <p>Review: {post.review}</p>
                                                </div>
                                            </div>
                                            <div className="profile-post-stats">
                                                <div className="profile-like-section">
                                                    <span>❤️ {post.likeCount}</span>
                                                </div>
                                                <button 
                                                    className="profile-comment-button"
                                                    onClick={() => {
                                                        setSelectedPostId(post.id);
                                                        setShowComments(true);
                                                    }}
                                                >
                                                    💬 {post.commentsCount || 0}
                                                </button>
                                            </div>
                                        </div>
                                        <div className="profile-post-right-section">
                                            <div className="profile-post-image">
                                                <img src={post.image_Url} alt={post.title} />
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>

                    {showComments && (
                        <CommentsModal 
                            postId={selectedPostId}
                            onClose={() => setShowComments(false)}
                            onCommentAdded={handleCommentAdded}
                        />
                    )}
                </div>
            </div>
        </div>
    );
};

export default UserProfile;