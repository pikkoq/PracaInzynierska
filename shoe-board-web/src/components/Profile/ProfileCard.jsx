import React, { useState, useEffect } from "react";
import { FaPen } from 'react-icons/fa';
import { getUserData, updateUserData } from '../../services/api';
import './ProfileCard.scss';

const ProfileCard = () => {
    const [isEditing, setIsEditing] = useState(false);
    const [showAvatarEdit, setShowAvatarEdit] = useState(false);
    const [userData, setUserData] = useState({
        username: '',
        email: '',
        bio: '',
        profilePicturePath: ''
    });
    const [newAvatarUrl, setNewAvatarUrl] = useState('');
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        fetchUserData();
    }, []);

    const fetchUserData = async () => {
        try {
            setIsLoading(true);
            const response = await getUserData();
            if (response.success) {
                setUserData(response.data);
                setNewAvatarUrl(response.data.profilePicturePath || '');
            }
        } catch (error) {
            setError('Failed to download user data.');
            console.error('Error fetching user data:', error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setUserData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleAvatarClick = () => {
        if (isEditing) {
            setShowAvatarEdit(true);
        }
    };

    const handleAvatarSubmit = () => {
        setUserData(prev => ({
            ...prev,
            profilePicturePath: newAvatarUrl
        }));
        setShowAvatarEdit(false);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            setError('');
            const response = await updateUserData({
                username: userData.username,
                email: userData.email,
                bio: userData.bio,
                profilePicturePath: userData.profilePicturePath
            });
            
            if (response.success) {
                setIsEditing(false);
                await fetchUserData();
            } else {
                setError('Failed to update data.');
            }
        } catch (error) {
            setError('Error while saving update.');
            console.error('Error updating user data:', error);
        }
    };

    if (isLoading) {
        return <div className="profile-container">Loading...</div>;
    }

    return (
        <div className="profile-container">
            <div className="profile-card">
                {error && <div className="error-message">{error}</div>}
                <div className="profile-header">
                    <div className="avatar-container">
                        <div className="avatar-wrapper">
                            <img 
                                src={userData.profilePicturePath || '../../assets/DefaultUser.png'} 
                                alt={userData.username}
                                className={`avatar-image ${isEditing ? 'editable' : ''}`}
                                onClick={handleAvatarClick}
                                title={isEditing ? "Click, to edit profile picture" : ""}
                            />
                            {isEditing && (
                                <div className="avatar-edit-icon">
                                    <FaPen />
                                </div>
                            )}
                        </div>
                    </div>
                    <h2 className="profile-title">User profile</h2>
                </div>
                
                {showAvatarEdit && (
                    <div className="avatar-edit-modal">
                        <div className="avatar-edit-content">
                            <h3>Edit Avatar</h3>
                            <input
                                type="text"
                                value={newAvatarUrl}
                                onChange={(e) => setNewAvatarUrl(e.target.value)}
                                placeholder="Enter avatar URL"
                            />
                            <div className="avatar-edit-buttons">
                                <button 
                                    className="save-button"
                                    onClick={handleAvatarSubmit}
                                >
                                    Save
                                </button>
                                <button 
                                    className="cancel-button"
                                    onClick={() => {
                                        setShowAvatarEdit(false);
                                        setNewAvatarUrl(userData.profilePicturePath || '');
                                    }}
                                >
                                    Cancel
                                </button>
                            </div>
                        </div>
                    </div>
                )}
                
                <div className="profile-content">
                    <form onSubmit={handleSubmit}>
                        <div className="form-fields">
                            <div className="form-group">
                                <label htmlFor="username">Username</label>
                                <input
                                    id="username"
                                    name="username"
                                    value={userData.username || ''}
                                    onChange={handleInputChange}
                                    disabled={!isEditing}
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="email">Email</label>
                                <input
                                    id="email"
                                    name="email"
                                    type="email"
                                    value={userData.email || ''}
                                    onChange={handleInputChange}
                                    disabled={!isEditing}
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="bio">Bio</label>
                                <textarea
                                    id="bio"
                                    name="bio"
                                    value={userData.bio || ''}
                                    onChange={handleInputChange}
                                    disabled={!isEditing}
                                />
                            </div>
                        </div>

                        <div className="button-group">
                            {isEditing ? (
                                <>
                                    <button type="submit" className="save-button">
                                        Save
                                    </button>
                                    <button 
                                        type="button" 
                                        className="cancel-button"
                                        onClick={() => {
                                            setIsEditing(false);
                                            fetchUserData();
                                        }}
                                    >
                                        Cancel
                                    </button>
                                </>
                            ) : (
                                <button 
                                    type="button" 
                                    className="edit-button"
                                    onClick={(e) => {
                                        e.preventDefault();
                                        setIsEditing(true);
                                    }}
                                >
                                    Edit
                                </button>
                            )}
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default ProfileCard;