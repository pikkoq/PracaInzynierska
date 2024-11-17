import React, { useState, useEffect } from 'react';
import TopNavbar from '../Navigation/TopNavbar';
import Navigation from '../Navigation/Navigation';
import PopularShoes from './PopularShoes';
import PostFeed from './PostFeed';
import AddPost from './AddPost';
import { getFriendPosts } from '../../services/api';
import './Home.scss';

const Home = () => {
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [pageNumber, setPageNumber] = useState(1);
    const [hasMore, setHasMore] = useState(true);

    const fetchPosts = async (page) => {
        try {
            setLoading(true);
            const response = await getFriendPosts(page);
            
            if (response.success) {
                if (page === 1) {
                    setPosts(response.data);
                } else {
                    setPosts(prevPosts => [...prevPosts, ...response.data]);
                }
                setHasMore(response.data.length === 10);
            }
        } catch (error) {
            console.error('Error fetching posts:', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPosts(1);
    }, []);

    const loadMore = () => {
        if (!loading && hasMore) {
            const nextPage = pageNumber + 1;
            setPageNumber(nextPage);
            fetchPosts(nextPage);
        }
    };

    return (
        <div className="home-container">
            <TopNavbar />
            <div className="main-content">
                <div className="left-nav">
                    <Navigation />
                </div>
                <div className="scrollable-content">
                    <AddPost />
                    <PostFeed 
                        posts={posts} 
                        loading={loading} 
                        hasMore={hasMore}
                        loadMore={loadMore}
                    />
                </div>
                <div className="right-sidebar">
                    <PopularShoes />
                </div>
            </div>
        </div>
    );
};

export default Home;
