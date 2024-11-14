import React, { useState, useEffect } from 'react';
import { getYourPosts, deletePost } from '../../services/api';
import { FaTrash } from 'react-icons/fa';
import CommentsModal from '../Main/CommentsModal';
import './ProfilePosts.scss';

const ProfilePosts = () => {
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const [showComments, setShowComments] = useState(false);
    const [selectedPostId, setSelectedPostId] = useState(null);

    const fetchPosts = async () => {
        try {
            const response = await getYourPosts();
            if (response.success) {
                setPosts(response.data);
            }
        } catch (error) {
            console.error('Error fetching your posts:', error);
            setError('Error fetching your posts.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPosts();
    }, []);

    const handleDeletePost = async (postId) => {
        if (window.confirm('Are you sure you want to delete this post?')) {
            try {
                const response = await deletePost(postId);
                if (response.success) {
                    setPosts(posts.filter(post => post.id !== postId));
                    setSuccessMessage('Post deleted successfully!');
                    setTimeout(() => {
                        setSuccessMessage('');
                    }, 3000);
                }
            } catch (error) {
                console.error('Error deleting post:', error);
                setError('Error deleting post.');
                setTimeout(() => {
                    setError('');
                }, 3000);
            }
        }
    };

    const handleCommentAdded = (postId) => {
        setPosts(posts.map(post => {
            if (post.id === postId) {
                return { ...post, commentsCount: (post.commentsCount || 0) + 1 };
            }
            return post;
        }));
    };

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <>
            <div className="profile-posts-container">
                <h2>Your posts</h2>
                {error && <div className="error-message">{error}</div>}
                {successMessage && <div className="success-message">{successMessage}</div>}
                {posts.length === 0 ? (
                    <div className="no-posts">You don't have any posts yet.</div>
                ) : (
                    <div className="profile-posts">
                        {posts.map((post) => (
                            <div key={post.id} className="profile-post">
                                <div className="post-content">
                                    <div className="post-header">
                                        <p className="post-date">
                                            {new Date(post.datePosted).toLocaleString()}
                                        </p>
                                    </div>
                                    <h3>{post.title}</h3>
                                    <p className="post-text">{post.content}</p>
                                    <div className="shoe-details">
                                        <div className="details-box">
                                            <p>Size: {post.size}</p>
                                            <p>Comfort: {post.comfortRating} ⭐</p>
                                            <p>Style: {post.styleRating} ⭐</p>
                                            <p>Season: {post.season}</p>
                                            <p>Review: {post.review}</p>
                                        </div>
                                    </div>
                                    <div className="post-stats">
                                        <div className="like-section">
                                            <span>❤️ {post.likeCount}</span>
                                        </div>
                                        <button 
                                            className="comment-button"
                                            onClick={() => {
                                                setSelectedPostId(post.id);
                                                setShowComments(true);
                                            }}
                                        >
                                            💬 {post.commentsCount || 0}
                                        </button>
                                    </div>
                                </div>
                                <div className="post-right-section">
                                    <div className="post-image">
                                        <img src={post.image_Url} alt={post.title} />
                                    </div>
                                    <button
                                        onClick={() => handleDeletePost(post.id)}
                                        className="delete-button"
                                        title="Delete post"
                                    >
                                        <FaTrash /> Delete
                                    </button>
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
        </>
    );
};

export default ProfilePosts;