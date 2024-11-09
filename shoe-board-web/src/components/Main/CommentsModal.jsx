import React, { useState, useEffect } from 'react';
import { addComment, getComments } from '../../services/api';
import './CommentsModal.scss';

const CommentsModal = ({ postId, onClose, onCommentAdded }) => {
    const [comments, setComments] = useState([]);
    const [newComment, setNewComment] = useState('');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [submitting, setSubmitting] = useState(false);

    const fetchComments = async () => {
        try {
            const response = await getComments(postId);
            if (response.success) {
                setComments(response.data);
            }
        } catch (error) {
            setError('Nie udało się pobrać komentarzy');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchComments();
    }, [postId]);

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
            setError('Nie udało się dodać komentarza');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="comments-modal">
            <div className="comments-content">
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
                                    <span className="comment-date">
                                        {new Date(comment.createdAt).toLocaleString()}
                                    </span>
                                </div>
                                <p className="comment-content">{comment.content}</p>
                            </div>
                        ))
                    )}
                </div>

                <form onSubmit={handleSubmit} className="comment-form">
                    {error && <div className="error-message">{error}</div>}
                    <textarea
                        value={newComment}
                        onChange={(e) => setNewComment(e.target.value)}
                        placeholder="Write a comment..."
                        disabled={submitting}
                    />
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