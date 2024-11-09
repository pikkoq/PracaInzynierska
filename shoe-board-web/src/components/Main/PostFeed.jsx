import React, { useState, useEffect } from 'react';
import Post from './Post';
import './PostFeed.scss';

const PostFeed = ({ initialPosts, loading }) => {
  const [posts, setPosts] = useState(initialPosts);

  useEffect(() => {
    setPosts(initialPosts);
  }, [initialPosts]);

  const handlePostUpdate = (postId, updates) => {
    setPosts(prevPosts =>
      prevPosts.map(post =>
        post.id === postId ? { ...post, ...updates } : post
      )
    );
  };

  if (loading) {
    return <div className="loading">Loading posts...</div>;
  }

  if (!Array.isArray(posts) || posts.length === 0) {
    return <div className="no-posts">No posts available</div>;
  }

  return (
    <div className="post-feed">
      {posts.map((post) => (
        <Post key={post.id} post={post} onPostUpdate={handlePostUpdate} />
      ))}
    </div>
  );
};

export default PostFeed;
