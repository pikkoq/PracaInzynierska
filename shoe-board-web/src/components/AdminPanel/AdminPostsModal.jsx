import React, { useState, useEffect, useRef } from 'react';
import { FaEdit, FaTrash, FaChevronLeft, FaChevronRight } from 'react-icons/fa';
import { getAllPosts, editPost, deletePostAdmin } from '../../services/api';
import CommentsModal from '../Main/CommentsModal';
import './AdminPostsModal.scss';
import ShoeImage from '../common/ShoeImage';

const AdminPostsModal = ({ onClose }) => {
    const [posts, setPosts] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [editingPost, setEditingPost] = useState(null);
    const [editContent, setEditContent] = useState('');
    const [isPageLoading, setIsPageLoading] = useState(false);
    const [showComments, setShowComments] = useState(false);
    const [selectedPostId, setSelectedPostId] = useState(null);
    const modalBodyRef = useRef(null);

    useEffect(() => {
        fetchPosts();
    }, [currentPage]);

    const fetchPosts = async () => {
        try {
            setIsPageLoading(true);
            const response = await getAllPosts(currentPage);
            if (response.success) {
                setPosts(response.data);
                setTotalPages(response.totalPages);
            }
        } catch (error) {
            setError('Failed to fetch posts');
        } finally {
            setIsPageLoading(false);
        }
    };

    const handleEdit = (post) => {
        setEditingPost(post);
        setEditContent(post.content);
    };

    const handleEditSubmit = async (e) => {
        e.preventDefault();
        try {
            setLoading(true);
            const response = await editPost(editingPost.postId, editContent);
            if (response.success) {
                await fetchPosts();
                setEditingPost(null);
            }
        } catch (error) {
            setError('Failed to edit post');
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (postId) => {
        if (window.confirm('Are you sure you want to delete this post?')) {
            try {
                const response = await deletePostAdmin(postId);
                if (response.success) {
                    await fetchPosts();
                }
            } catch (error) {
                setError('Failed to delete post');
            }
        }
    };

    const getImageUrl = (imageUrl) => {
        if (!imageUrl) return '';

        if (imageUrl.startsWith('https')) {
            return imageUrl;
        }

        if (imageUrl.startsWith('/uploads')) {
            return `https://localhost:7117${imageUrl}`;
        }

        return imageUrl;
    };

    const handleViewProfile = (userName) => {
        window.open(`/profile/${userName}`, '_blank');
    };

    const handleCommentClick = (postId) => {
        setSelectedPostId(postId);
        setShowComments(true);
    };

    const handleCommentAdded = () => {
        fetchPosts();
    };

    const handlePageChange = (newPage) => {
        setCurrentPage(newPage);
        if (modalBodyRef.current) {
            modalBodyRef.current.scrollTop = 0;
        }
    };

    return (
        <div className="admin-modal-overlay" onClick={onClose}>
            <div className="admin-modal-content" onClick={e => e.stopPropagation()}>
                <div className="admin-modal-header">
                    <h2>Posts Management</h2>
                    <button className="close-button" onClick={onClose}>&times;</button>
                </div>
                <div className="admin-modal-body" ref={modalBodyRef}>
                    {error && <div className="admin-error-message">{error}</div>}
                    <div className={`posts-grid ${isPageLoading ? 'loading' : ''}`}>
                        {posts.map(post => (
                            <div key={post.postId} className="post-card">
                                <div className="post-content-wrapper">
                                    <div className="post-header">
                                        <div 
                                            className="user-info"
                                            onClick={() => handleViewProfile(post.username)}
                                            style={{ cursor: 'pointer' }}
                                        >
                                            <img 
                                                src={post.profilePicture} 
                                                alt={post.username} 
                                                className="user-avatar"
                                            />
                                            <span className="username">{post.username}</span>
                                        </div>
                                        <span className="post-date">
                                            {new Date(post.datePosted).toLocaleDateString()}
                                        </span>
                                    </div>
                                    <div className="post-content">
                                        <h3>{post.title}</h3>
                                        <pre className="content-text">{post.content}</pre>
                                        <div className="shoe-details">
                                            <p>Size: {post.size}</p>
                                            <p>Comfort: {post.comfortRating}⭐</p>
                                            <p>Style: {post.styleRating}⭐</p>
                                            <p>Season: {post.season}</p>
                                            <p>Review: {post.review}</p>
                                        </div>
                                        <div className="post-stats">
                                            <span>❤️ {post.likeCount}</span>
                                            <button 
                                                className="comments-button"
                                                onClick={() => handleCommentClick(post.postId)}
                                            >
                                                💬 {post.commentsCount}
                                            </button>
                                        </div>
                                        <div className="post-actions">
                                            <button 
                                                className="action-button edit"
                                                onClick={() => handleEdit(post)}
                                            >
                                                <FaEdit /> Edit
                                            </button>
                                            <button 
                                                className="action-button delete"
                                                onClick={() => handleDelete(post.postId)}
                                            >
                                                <FaTrash /> Delete
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div className="post-image">
                                    <ShoeImage src={getImageUrl(post.shoePhoto)} alt={post.title} />
                                </div>
                            </div>
                        ))}
                    </div>

                    {showComments && (
                        <CommentsModal 
                            postId={selectedPostId}
                            onClose={() => setShowComments(false)}
                            onCommentAdded={handleCommentAdded}
                        />
                    )}

                    {editingPost && (
                        <div className="edit-form-overlay">
                            <div className="edit-form">
                                <h3>Edit Post</h3>
                                <form onSubmit={handleEditSubmit}>
                                    <textarea
                                        value={editContent}
                                        onChange={(e) => setEditContent(e.target.value)}
                                        rows="4"
                                    />
                                    <div className="form-actions">
                                        <button type="submit" className="save-button" disabled={loading}>
                                            {loading ? 'Saving...' : 'Save'}
                                        </button>
                                        <button 
                                            type="button" 
                                            className="cancel-button"
                                            onClick={() => setEditingPost(null)}
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

export default AdminPostsModal;
