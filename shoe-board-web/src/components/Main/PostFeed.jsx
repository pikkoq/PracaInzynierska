import React, { useCallback, useRef } from 'react';
import Post from './Post';
import './PostFeed.scss';

const PostFeed = ({ posts, loading, hasMore, loadMore }) => {
    const observer = useRef();
    
    const lastPostRef = useCallback(node => {
        if (loading) return;
        
        if (observer.current) {
            observer.current.disconnect();
        }
        
        observer.current = new IntersectionObserver(entries => {
            if (entries[0].isIntersecting && hasMore) {
                loadMore();
            }
        });
        
        if (node) {
            observer.current.observe(node);
        }
    }, [loading, hasMore, loadMore]);

    if (!Array.isArray(posts)) {
        return <div className="no-posts">No posts available</div>;
    }

    return (
        <div className="post-feed">
            {posts.map((post, index) => {
                if (posts.length === index + 1) {
                    // Ostatni post
                    return (
                        <div ref={lastPostRef} key={post.id}>
                            <Post post={post} />
                        </div>
                    );
                } else {
                    return <Post key={post.id} post={post} />;
                }
            })}
            {loading && (
                <div className="loading-more">Loading more posts...</div>
            )}
            {!hasMore && posts.length > 0 && (
                <div className="no-more-posts">No more posts to load</div>
            )}
        </div>
    );
};

export default PostFeed;
