import React, { useState } from 'react';
import { likePost, unlikePost, getCatalogShoeDetails } from '../../services/api';
import './Post.css';
import ShoeDetailsModal from '../Navigation/ShoeDetailsModal';

const Post = ({ post, onPostUpdate }) => {
  const [isLiked, setIsLiked] = useState(post.isLiked);
  const [likeCount, setLikeCount] = useState(post.likeCount);
  const [showDetails, setShowDetails] = useState(false);
  const [shoeDetails, setShoeDetails] = useState(null);
  const [isLoadingDetails, setIsLoadingDetails] = useState(false);

  const handleLikeClick = async () => {
    try {
      const newIsLiked = !isLiked;
      const newLikeCount = newIsLiked ? likeCount + 1 : likeCount - 1;
      
      setIsLiked(newIsLiked);
      setLikeCount(newLikeCount);

      const response = newIsLiked ? await likePost(post.id) : await unlikePost(post.id);
      
      if (!response.success) {
        setIsLiked(!newIsLiked);
        setLikeCount(likeCount);
        console.error('Like/Unlike operation failed:', response.message);
      } else {
        onPostUpdate(post.id, { isLiked: newIsLiked, likeCount: newLikeCount });
      }
    } catch (error) {
      console.error('Error toggling like:', error);
      setIsLiked(!isLiked);
      setLikeCount(likeCount);
    }
  };

  const handleImageClick = async () => {
    try {
      setIsLoadingDetails(true);
      const response = await getCatalogShoeDetails(post.shoeCatalogId);
      if (response.success) {
        setShoeDetails(response.data);
        setShowDetails(true);
      }
    } catch (error) {
      console.error('Error fetching shoe details:', error);
    } finally {
      setIsLoadingDetails(false);
    }
  };

  return (
    <>
      <div className="post">
        <div className="post-content">
          <div className="post-header">
            <h3>{post.username}</h3>
            <p>Posted on: {new Date(post.datePosted).toLocaleString()}</p>
          </div>
          <h2 className="post-title">{post.title}</h2>
          <p className="post-text">{post.content}</p>
          <div className="post-details">
            <p>Size: {post.size}</p>
            <p>Comfort Rating: {post.comfortRating}</p>
            <p>Style Rating: {post.styleRating}</p>
            <p>Season: {post.season}</p>
            <p>Review: {post.review}</p>
          </div>
          <div className="post-footer">
            <div className="like-section">
              <button onClick={handleLikeClick} className="like-button">
                <i className={`bx ${isLiked ? 'bxs-heart' : 'bx-heart'}`}></i>
              </button>
              <span>{likeCount}</span>
            </div>
            <span>Comments: {post.comments.length}</span>
          </div>
        </div>
        <div className="post-image" onClick={handleImageClick} style={{ cursor: 'pointer' }}>
          <img src={post.image_Url} alt={post.title} />
        </div>
      </div>

      {showDetails && (
        <ShoeDetailsModal 
          shoe={shoeDetails}
          isLoading={isLoadingDetails}
          onClose={() => setShowDetails(false)}
        />
      )}
    </>
  );
};

export default Post;
