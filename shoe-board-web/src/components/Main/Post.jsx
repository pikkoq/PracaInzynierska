import React, { useState } from 'react';
import { likePost, unlikePost, getCatalogShoeDetails } from '../../services/api';
import { useNavigate } from 'react-router-dom';
import './Post.scss';
import ShoeImage from '../common/ShoeImage';
import ShoeDetailsModal from '../Navigation/ShoeDetailsModal';
import CommentsModal from './CommentsModal';

const Post = ({ post, onPostUpdate }) => {
  const navigate = useNavigate();
  const [isLiked, setIsLiked] = useState(post.isLiked);
  const [likeCount, setLikeCount] = useState(post.likeCount);
  const [showDetails, setShowDetails] = useState(false);
  const [shoeDetails, setShoeDetails] = useState(null);
  const [isLoadingDetails, setIsLoadingDetails] = useState(false);
  const [showComments, setShowComments] = useState(false);
  const [commentsCount, setCommentsCount] = useState(post.commentsCount || 0);
  const [isProcessingLike, setIsProcessingLike] = useState(false);

  const handleLikeClick = async () => {
    if (isProcessingLike) return;
    
    setIsProcessingLike(true);
    try {
      const response = isLiked ? 
        await unlikePost(post.id) : 
        await likePost(post.id);
      
      if (response.success) {
        const newIsLiked = !isLiked;
        const newLikeCount = newIsLiked ? likeCount + 1 : likeCount - 1;
        
        setIsLiked(newIsLiked);
        setLikeCount(newLikeCount);
        onPostUpdate(post.id, { isLiked: newIsLiked, likeCount: newLikeCount });
      }
    } catch (error) {
      console.error('Error toggling like:', error);
    } finally {
      setIsProcessingLike(false);
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

  const handleCommentAdded = () => {
    const newCommentsCount = commentsCount + 1;
    setCommentsCount(newCommentsCount);
    if (onPostUpdate) {
      try {
        onPostUpdate(post.id, { commentsCount: newCommentsCount });
      } catch (error) {
        setCommentsCount(commentsCount);
        console.error('Error updating comment count:', error);
      }
    }
  };

  const handleViewProfile = (username) => {
    navigate(`/profile/${username}`);
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

  return (
    <>
      <div className="post">
        <div className="post-content">
          <div className="post-header">
            <div className="user-info">
              <div className="avatar">
                <img src={getImageUrl(post.profilePictureUrl) || 'https://icons.veryicon.com/png/o/miscellaneous/common-icons-31/default-avatar-2.png'} alt="User avatar" />
              </div>
              <div className="user-details">
                <h3 
                  onClick={() => handleViewProfile(post.username)}
                  style={{ cursor: 'pointer' }}
                >
                  {post.username}
                </h3>
                <p>Posted on: {new Date(post.datePosted).toLocaleString()}</p>
              </div>
            </div>
          </div>
          <h2 className="post-title">{post.title}</h2>
          <pre className="post-text">{post.content}</pre>
          <div className="post-details">
            <p>Size: {post.size}</p>
            <p>Comfort Rating: {post.comfortRating} ⭐</p>
            <p>Style Rating: {post.styleRating} ⭐</p>
            <p>Season: {post.season}</p>
            <p>Review: {post.review}</p>
          </div>
          <div className="post-footer">
            <div className="like-section">
              <button 
                onClick={handleLikeClick} 
                className="like-button"
                disabled={isProcessingLike}
              >
                <i className={`bx ${isLiked ? 'bxs-heart' : 'bx-heart'}`}></i>
              </button>
              <span>{likeCount}</span>
            </div>
            <button 
              className="comment-button"
              onClick={() => setShowComments(true)}
            >
              💬 {commentsCount || 0}
            </button>
          </div>
        </div>
        <div className="post-image" onClick={handleImageClick} style={{ cursor: 'pointer' }}>
          <ShoeImage src={getImageUrl(post.image_Url)} alt={post.title} />
        </div>
      </div>

      {showComments && (
        <CommentsModal 
          postId={post.id}
          onClose={() => setShowComments(false)}
          onCommentAdded={handleCommentAdded}
        />
      )}

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
