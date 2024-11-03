import React, { useState, useEffect } from 'react';
import TopNavbar from '../Navigation/TopNavbar';
import Navigation from '../Navigation/Navigation';
import PopularShoes from './PopularShoes';
import PostFeed from './PostFeed';
import { getFriendPosts } from '../../services/api';
import './Home.css';

const Home = () => {
    const [posts, setPosts] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchPosts = async () => {
            try {
                const response = await getFriendPosts();
                setPosts(response.data);
            } catch (error) {
                console.error('Error fetching posts:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchPosts();
    }, []);

    return (
        <div className="home-container">
            <TopNavbar />
            <div className="main-content">
                <div className="left-nav">
                    <Navigation />
                </div>
                <div className="scrollable-content">
                    <PostFeed initialPosts={posts} loading={loading} />
                </div>
                <div className="right-sidebar">
                    <PopularShoes />
                </div>
            </div>
        </div>
    );
};

export default Home;
