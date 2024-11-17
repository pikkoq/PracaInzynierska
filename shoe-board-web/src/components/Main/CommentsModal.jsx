import React, { useState, useEffect } from 'react';
import { addComment, getComments } from '../../services/api';
import './CommentsModal.scss';

const CommentsModal = ({ postId, onClose, onCommentAdded }) => {
    const [comments, setComments] = useState([]);
    const [newComment, setNewComment] = useState('');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [submitting, setSubmitting] = useState(false);
    const MAX_COMMENT_LENGTH = 100;
    const MAX_LINES = 3;

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
            setError('Nie udało się dodać komentarza');
        } finally {
            setSubmitting(false);
        }
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
                                    <span className="comment-date">
                                        {new Date(comment.createdAt).toLocaleString()}
                                    </span>
                                </div>
                                <pre className="comment-content">{comment.content}</pre>
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