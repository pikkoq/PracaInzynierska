import React, { useState, useEffect } from 'react';
import { FaEllipsisV } from 'react-icons/fa';
import { 
    addComment, 
    getComments, 
    deleteComment, 
    deleteCommentAdmin, 
    editCommentAdmin 
} from '../../services/api';
import './CommentsModal.scss';

const CommentsModal = ({ postId, onClose, onCommentAdded }) => {
    const [comments, setComments] = useState([]);
    const [newComment, setNewComment] = useState('');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [submitting, setSubmitting] = useState(false);
    const [activeDropdown, setActiveDropdown] = useState(null);
    const [isAdmin, setIsAdmin] = useState(false);
    const [editingComment, setEditingComment] = useState(null);
    const [editContent, setEditContent] = useState('');
    const MAX_COMMENT_LENGTH = 100;
    const MAX_LINES = 3;

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const userRole = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
            setIsAdmin(userRole === "Admin");
        }
    }, []);

    const fetchComments = async () => {
        try {
            const response = await getComments(postId);
            if (response.success) {
                setComments(response.data);
            }
        } catch (error) {
            setError('Failed to fetch comments');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchComments();
    }, [postId]);

    const handleCommentChange = (e) => {
        const newContent = e.target.value;
        const lines = newContent.split('\n');
        
        if (lines.length <= MAX_LINES) {
            if (newContent.length <= MAX_COMMENT_LENGTH) {
                setNewComment(newContent);
            }
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!newComment.trim()) return;

        setSubmitting(true);
        try {
            const response = await addComment(postId, newComment.trim());
            if (response.success) {
                setNewComment('');
                onCommentAdded();
                await fetchComments();
            }
        } catch (error) {
            setError('Failed to add comment');
        } finally {
            setSubmitting(false);
        }
    };

    const handleDeleteComment = async (commentId) => {
        try {
            const response = isAdmin 
                ? await deleteCommentAdmin(commentId)
                : await deleteComment(commentId);

            if (response.success) {
                await fetchComments();
            } else {
                setError('Failed to delete comment');
            }
        } catch (error) {
            setError('Error deleting comment');
        }
        setActiveDropdown(null);
    };

    const handleEditComment = async (commentId) => {
        try {
            const response = await editCommentAdmin(commentId, editContent);
            if (response.success) {
                await fetchComments();
                setEditingComment(null);
                setEditContent('');
            } else {
                setError('Failed to edit comment');
            }
        } catch (error) {
            setError('Error editing comment');
        }
        setActiveDropdown(null);
    };

    const getCurrentUsername = () => {
        const token = localStorage.getItem('token');
        if (token) {
            const payload = JSON.parse(atob(token.split('.')[1]));
            return payload.username;
        }
        return null;
    };

    return (
        <div className="comments-modal" onClick={onClose}>
            <div className="comments-content" onClick={e => e.stopPropagation()}>
                <div className="comments-header">
                    <h3>Comments</h3>
                    <button className="close-button" onClick={onClose}>&times;</button>
                </div>

                <div className="comments-list">
                    {loading ? (
                        <div className="loading">Loading comments...</div>
                    ) : comments.length === 0 ? (
                        <div className="no-comments">No comments yet</div>
                    ) : (
                        comments.map((comment) => (
                            <div key={comment.id} className="comment">
                                <div className="comment-header">
                                    <span className="comment-author">{comment.username}</span>
                                    <div className="comment-actions">
                                        <span className="comment-date">
                                            {new Date(comment.createdAt).toLocaleString()}
                                        </span>
                                        {(getCurrentUsername() === comment.username || isAdmin) && (
                                            <div className="dropdown-container">
                                                <button 
                                                    className="action-button"
                                                    onClick={(e) => {
                                                        e.stopPropagation();
                                                        setActiveDropdown(activeDropdown === comment.id ? null : comment.id);
                                                    }}
                                                >
                                                    <FaEllipsisV />
                                                </button>
                                                {activeDropdown === comment.id && (
                                                    <div className="dropdown-menu">
                                                        {isAdmin && (
                                                            <button 
                                                                onClick={() => {
                                                                    setEditingComment(comment.id);
                                                                    setEditContent(comment.content);
                                                                }}
                                                            >
                                                                Edit
                                                            </button>
                                                        )}
                                                        <button onClick={() => handleDeleteComment(comment.id)}>
                                                            Delete
                                                        </button>
                                                    </div>
                                                )}
                                            </div>
                                        )}
                                    </div>
                                </div>
                                {editingComment === comment.id ? (
                                    <div className="edit-comment-form">
                                        <textarea
                                            value={editContent}
                                            onChange={(e) => setEditContent(e.target.value)}
                                            maxLength={MAX_COMMENT_LENGTH}
                                            rows={3}
                                        />
                                        <div className="edit-actions">
                                            <button onClick={() => handleEditComment(comment.id)}>Save</button>
                                            <button onClick={() => {
                                                setEditingComment(null);
                                                setEditContent('');
                                            }}>Cancel</button>
                                        </div>
                                    </div>
                                ) : (
                                    <pre className="comment-content">{comment.content}</pre>
                                )}
                            </div>
                        ))
                    )}
                </div>

                <form onSubmit={handleSubmit} className="comment-form">
                    {error && <div className="error-message">{error}</div>}
                    <div className="comment-input-group">
                        <label htmlFor="comment">
                            <span className="comment-limit">
                                ({newComment.length || 0}/{MAX_COMMENT_LENGTH})
                            </span>
                        </label>
                        <textarea
                            id="comment"
                            value={newComment}
                            onChange={handleCommentChange}
                            placeholder="Write a comment..."
                            disabled={submitting}
                            maxLength={MAX_COMMENT_LENGTH}
                            rows={3}
                        />
                    </div>
                    <button 
                        type="submit" 
                        disabled={submitting || !newComment.trim()}
                    >
                        {submitting ? 'Sending...' : 'Send'}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default CommentsModal; 