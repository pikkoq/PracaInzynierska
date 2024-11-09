import React from "react";
import TopNavbar from '../Navigation/TopNavbar';
import Navigation from "../Navigation/Navigation";
import ProfileCard from "./ProfileCard";
import ProfilePosts from "./ProfilePosts";
import './Profile.scss';

const Profile = () => {
    return(
        <div className="profile-container">
            <TopNavbar />
            <div className="main-content">
                <div className="left-nav">
                    <Navigation />
                </div>
                <div className="main-content-container">
                    <ProfileCard />
                    <ProfilePosts />
                </div>
            </div>
        </div>
    );
};

export default Profile;