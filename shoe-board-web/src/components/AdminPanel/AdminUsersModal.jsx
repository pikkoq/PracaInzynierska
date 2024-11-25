import React, { useState, useEffect, useRef } from 'react';
import { FaEdit, FaTrash, FaChevronLeft, FaChevronRight } from 'react-icons/fa';
import { getAllUsers, deleteUser, editUser } from '../../services/api';
import './AdminUsersModal.scss';

const AdminUsersModal = ({ onClose }) => {
    const [users, setUsers] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [editingUser, setEditingUser] = useState(null);
    const [editFormData, setEditFormData] = useState({
        bio: '',
        profilePicture: ''
    });
    const [isPageLoading, setIsPageLoading] = useState(false);
    const modalBodyRef = useRef(null);

    useEffect(() => {
        fetchUsers();
    }, [currentPage]);

    const fetchUsers = async () => {
        try {
            setIsPageLoading(true);
            const response = await getAllUsers(currentPage);
            if (response.success) {
                setUsers(response.data);
                setTotalPages(response.totalPages);
            }
        } catch (error) {
            setError('Failed to fetch users');
        } finally {
            setIsPageLoading(false);
        }
    };

    const handleDelete = async (userId) => {
        if (window.confirm('Are you sure you want to delete this user? This action cannot be undone.')) {
            try {
                const response = await deleteUser(userId);
                if (response.success) {
                    await fetchUsers();
                }
            } catch (error) {
                setError('Failed to delete user');
            }
        }
    };

    const handleEdit = (user) => {
        setEditingUser(user);
        setEditFormData({
            bio: user.bio,
            profilePicture: user.profilePicturePath
        });
    };

    const handleEditSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await editUser(editingUser.id, editFormData);
            if (response.success) {
                setEditingUser(null);
                await fetchUsers();
            }
        } catch (error) {
            setError('Failed to edit user');
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setEditFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleViewProfile = (userName) => {
        window.open(`/profile/${userName}`, '_blank');
    };

    const handlePageChange = (newPage) => {
        setCurrentPage(newPage);
        if (modalBodyRef.current) {
            modalBodyRef.current.scrollTop = 0;
        }
    };

    if (loading) return null;

    return (
        <div className="admin-modal-overlay" onClick={onClose}>
            <div className="admin-modal-content" onClick={e => e.stopPropagation()}>
                <div className="admin-modal-header">
                    <h2>User Management</h2>
                    <button className="close-button" onClick={onClose}>&times;</button>
                </div>
                <div className="admin-modal-body" ref={modalBodyRef}>
                    {error && <div className="admin-error-message">{error}</div>}
                    <div className={`users-grid ${isPageLoading ? 'loading' : ''}`}>
                        {users.map(user => (
                            <div key={user.id} className="user-card">
                                <div 
                                    className="user-avatar"
                                    onClick={() => handleViewProfile(user.userName)}
                                    style={{ cursor: 'pointer' }}
                                    title="Open profile in new tab"
                                >
                                    <img src={user.profilePicturePath} alt={user.userName} />
                                </div>
                                <div className="user-info">
                                    <h3 
                                        onClick={() => handleViewProfile(user.userName)}
                                        style={{ cursor: 'pointer' }}
                                        title="Open profile in new tab"
                                    >
                                        {user.userName}
                                    </h3>
                                    <div className="user-details">
                                        <p>Email: {user.email}</p>
                                        <p>Joined: {new Date(user.dateCreated).toLocaleDateString()}</p>
                                        <div className="user-actions">
                                            <button 
                                                className="action-button edit"
                                                onClick={() => handleEdit(user)}
                                            >
                                                <FaEdit /> Edit
                                            </button>
                                            <button 
                                                className="action-button delete"
                                                onClick={() => handleDelete(user.id)}
                                            >
                                                <FaTrash /> Delete
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                    {isPageLoading && <div className="page-loading-overlay">Loading...</div>}

                    {editingUser && (
                        <div className="edit-form-overlay">
                            <div className="edit-form">
                                <h3>Edit User: {editingUser.userName}</h3>
                                <form onSubmit={handleEditSubmit}>
                                    <div className="form-group">
                                        <label>Bio</label>
                                        <textarea
                                            name="bio"
                                            value={editFormData.bio}
                                            onChange={handleInputChange}
                                            rows="4"
                                        />
                                    </div>
                                    <div className="form-group">
                                        <label>Profile Picture URL</label>
                                        <input
                                            type="text"
                                            name="profilePicture"
                                            value={editFormData.profilePicture}
                                            onChange={handleInputChange}
                                        />
                                    </div>
                                    <div className="form-actions">
                                        <button type="submit" className="save-button">Save</button>
                                        <button 
                                            type="button" 
                                            className="cancel-button"
                                            onClick={() => setEditingUser(null)}
                                        >
                                            Cancel
                                        </button>
                                    </div>
                                </form>
                            </div>
                        </div>
                    )}

                    <div className="pagination">
                        <button 
                            onClick={() => handlePageChange(currentPage - 1)}
                            disabled={currentPage === 1 || isPageLoading}
                        >
                            <FaChevronLeft />
                        </button>
                        <span>Page {currentPage} of {totalPages}</span>
                        <button 
                            onClick={() => handlePageChange(currentPage + 1)}
                            disabled={currentPage === totalPages || isPageLoading}
                        >
                            <FaChevronRight />
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AdminUsersModal;
