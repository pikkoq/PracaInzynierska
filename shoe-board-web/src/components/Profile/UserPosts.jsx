import React, { useState, useEffect } from 'react';
import { getYourPosts, deletePost } from '../../services/api';
import { FaTrash } from 'react-icons/fa';
import './UserPosts.scss';

const UserPosts = () => {
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [successMessage, setSuccessMessage] = useState('');

    const fetchPosts = async () => {
        try {
            const response = await getYourPosts();
            if (response.success) {
                setPosts(response.data);
            }
        } catch (error) {
            console.error('Error fetching your posts:', error);
            setError('Error fetching your posts');
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
                    setSuccessMessage('The post was removed successfully!');
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

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="user-posts-container">
            <h2>Twoje posty</h2>
            {error && <div className="error-message">{error}</div>}
            {successMessage && <div className="success-message">{successMessage}</div>}
            {posts.length === 0 ? (
                <div className="no-posts">Nie masz jeszcze żadnych postów</div>
            ) : (
                <div className="posts-grid">
                    {posts.map((post) => (
                        <div key={post.id} className="post-card">
                            <div className="post-header">
                                <p className="post-date">
                                    {new Date(post.datePosted).toLocaleString()}
                                </p>
                                <button
                                    onClick={() => handleDeletePost(post.id)}
                                    className="delete-button"
                                    title="Usuń post"
                                >
                                    <FaTrash />
                                </button>
                            </div>
                            <div className="post-image">
                                <img src={post.image_Url} alt={post.title} />
                            </div>
                            <div className="post-content">
                                <h3>{post.title}</h3>
                                <p>{post.content}</p>
                                <div className="post-details">
                                    <p>Size: {post.size}</p>
                                    <p>Comfort: {post.comfortRating}/5</p>
                                    <p>Style: {post.styleRating}/5</p>
                                    <p>Season: {post.season}</p>
                                </div>
                                <div className="post-stats">
                                    <span>❤️ {post.likeCount}</span>
                                    <span>💬 {post.comments?.length || 0}</span>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default UserPosts; 