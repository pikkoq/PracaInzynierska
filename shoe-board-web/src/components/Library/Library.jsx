import React from "react";
import TopNavbar from '../Navigation/TopNavbar';
import Navigation from "../Navigation/Navigation";
import ShoeLibrary from "./ShoeLibrary";
import './Library.scss';

const Library = () => {
    return(
        <div className="library-container">
            <TopNavbar />
            <div className="main-content">
                <div className="left-nav">
                    <Navigation />
                </div>
                <div className="main-content-container">
                    <ShoeLibrary />
                </div>
            </div>
        </div>
    );
};

export default Library; 