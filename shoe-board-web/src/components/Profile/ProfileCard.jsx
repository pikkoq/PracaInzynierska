import React, { useState, useEffect } from "react";
import { FaPen } from 'react-icons/fa';
import { getUserData, updateUserData, changeUserPassword } from '../../services/api';
import './ProfileCard.scss';

const ProfileCard = ({ onClose }) => {
    const [isEditing, setIsEditing] = useState(false);
    const [userData, setUserData] = useState({
        username: '',
        email: '',
        bio: '',
        profilePicturePath: ''
    });
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(true);
    const MAX_BIO_LENGTH = 500;
    const MAX_LINES = 5;
    const [showPasswordChange, setShowPasswordChange] = useState(false);
    const [passwordData, setPasswordData] = useState({
        currentPassword: '',
        newPassword: '',
        confirmNewPassword: ''
    });
    const [passwordError, setPasswordError] = useState('');
    const [passwordSuccess, setPasswordSuccess] = useState('');
    const [isChangingPassword, setIsChangingPassword] = useState(false);

    useEffect(() => {
        fetchUserData();
    }, []);

    const fetchUserData = async () => {
        try {
            setIsLoading(true);
            const response = await getUserData();
            if (response.success) {
                setUserData(response.data);
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

    const handleBioChange = (e) => {
        const content = e.target.value;
        const lines = content.split('\n');
        
        if (lines.length <= MAX_LINES) {
            if (content.length <= MAX_BIO_LENGTH) {
                setUserData(prev => ({
                    ...prev,
                    bio: content
                }));
            }
        }
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
                setError(response.message || 'Failed to update data.');
            }
        } catch (error) {
            setError(error.response?.data?.message || 'Error while saving update.');
            console.error('Error updating user data:', error);
        }
    };

    const handlePasswordChange = (e) => {
        const { name, value } = e.target;
        setPasswordData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handlePasswordSubmit = async (e) => {
        e.preventDefault();
        setPasswordError('');
        setPasswordSuccess('');

        if (!passwordData.currentPassword || !passwordData.newPassword || !passwordData.confirmNewPassword) {
            setPasswordError('All fields are required');
            return;
        }

        if (passwordData.newPassword !== passwordData.confirmNewPassword) {
            setPasswordError('New passwords do not match');
            return;
        }

        if (passwordData.newPassword.length < 6) {
            setPasswordError('New password must be at least 6 characters long');
            return;
        }

        setIsChangingPassword(true);
        try {
            const response = await changeUserPassword({
                currentPassword: passwordData.currentPassword,
                newPassword: passwordData.newPassword
            });

            if (response.success) {
                setPasswordSuccess('Password changed successfully');
                setPasswordData({
                    currentPassword: '',
                    newPassword: '',
                    confirmNewPassword: ''
                });
                setTimeout(() => {
                    setShowPasswordChange(false);
                    setPasswordSuccess('');
                }, 2000);
            } else {
                setPasswordError(response.message || 'Failed to change password');
            }
        } catch (error) {
            setPasswordError('Current password is incorrect or server error occurred');
            console.error('Error changing password:', error);
        } finally {
            setIsChangingPassword(false);
        }
    };

    if (isLoading) {
        return <div className="profile-card">Loading...</div>;
    }

    return (
        <div className="profile-card">
            <div className="profile-main-content">
                <div className="profile-left-section">
                    <img 
                        src={userData.profilePicturePath || require('../../assets/DefaultUser.png')} 
                        alt={userData.username}
                        className="profile-avatar"
                    />
                    <div className="profile-info">
                        <h2 className="username">{userData.username}</h2>
                        <pre className="bio" >{userData.bio || 'There is no bio yet...'}</pre>
                    </div>
                </div>
                <button 
                    className="edit-profile-button"
                    onClick={() => {
                        setError('');
                        setIsEditing(true);
                    }}
                >
                    Edit Profile
                </button>
            </div>

            {isEditing && (
                <div className="edit-modal-overlay">
                    <div className="edit-modal">
                        <h3>Edit Profile</h3>
                        {error && <div className="modal-error-message">{error}</div>}
                        <form onSubmit={handleSubmit}>
                            <div className="form-group">
                                <label htmlFor="username">Username</label>
                                <input
                                    id="username"
                                    name="username"
                                    value={userData.username || ''}
                                    onChange={handleInputChange}
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
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="bio">
                                    Bio 
                                    <span className="bio-limit">
                                        ({userData.bio?.length || 0}/{MAX_BIO_LENGTH})
                                    </span>
                                </label>
                                <textarea
                                    id="bio"
                                    name="bio"
                                    value={userData.bio || ''}
                                    onChange={handleBioChange}
                                    maxLength={MAX_BIO_LENGTH}
                                    rows={5}
                                    className="bio-textarea"
                                    placeholder="Write something about yourself..."
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="profilePicturePath">Avatar URL</label>
                                <input
                                    id="profilePicturePath"
                                    name="profilePicturePath"
                                    value={userData.profilePicturePath || ''}
                                    onChange={handleInputChange}
                                />
                            </div>

                            <div className="modal-buttons">
                                <button 
                                    type="button" 
                                    className="change-password-button"
                                    onClick={() => setShowPasswordChange(true)}
                                >
                                    Change Password
                                </button>
                                <button type="submit" className="save-button">
                                    Save Changes
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
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {showPasswordChange && (
                <div className="password-modal-overlay">
                    <div className="password-modal">
                        <h3>Change Password</h3>
                        {passwordError && <div className="modal-error-message">{passwordError}</div>}
                        {passwordSuccess && <div className="modal-success-message">{passwordSuccess}</div>}
                        <form onSubmit={handlePasswordSubmit}>
                            <div className="form-group">
                                <label htmlFor="currentPassword">Current Password</label>
                                <input
                                    type="password"
                                    id="currentPassword"
                                    name="currentPassword"
                                    value={passwordData.currentPassword}
                                    onChange={handlePasswordChange}
                                    disabled={isChangingPassword}
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="newPassword">New Password</label>
                                <input
                                    type="password"
                                    id="newPassword"
                                    name="newPassword"
                                    value={passwordData.newPassword}
                                    onChange={handlePasswordChange}
                                    disabled={isChangingPassword}
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="confirmNewPassword">Confirm New Password</label>
                                <input
                                    type="password"
                                    id="confirmNewPassword"
                                    name="confirmNewPassword"
                                    value={passwordData.confirmNewPassword}
                                    onChange={handlePasswordChange}
                                    disabled={isChangingPassword}
                                />
                            </div>

                            <div className="modal-buttons">
                                <button 
                                    type="submit" 
                                    className="save-button"
                                    disabled={isChangingPassword}
                                >
                                    {isChangingPassword ? 'Changing...' : 'Change Password'}
                                </button>
                                <button 
                                    type="button" 
                                    className="cancel-button"
                                    onClick={() => {
                                        setShowPasswordChange(false);
                                        setPasswordError('');
                                        setPasswordData({
                                            currentPassword: '',
                                            newPassword: '',
                                            confirmNewPassword: ''
                                        });
                                    }}
                                    disabled={isChangingPassword}
                                >
                                    Cancel
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ProfileCard;