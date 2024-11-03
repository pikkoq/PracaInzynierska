import React from "react";
import TopNavbar from '../Navigation/TopNavbar';
import Navigation from "../Navigation/Navigation";
import ProfileCard from "./ProfileCard";
import './Profile.css';

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
                </div>
            </div>
        </div>
    );
};

export default Profile;