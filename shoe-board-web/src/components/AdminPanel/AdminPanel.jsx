import React, { useState, useEffect } from 'react';
import { Navigate } from 'react-router-dom';
import TopNavbar from '../Navigation/TopNavbar';
import Navigation from '../Navigation/Navigation';
import AdminShoesModal from './AdminShoesModal';
import './AdminPanel.scss';

const AdminPanel = () => {
    const [showShoesModal, setShowShoesModal] = useState(false);
    const [showUsersModal, setShowUsersModal] = useState(false);
    const [showPostsModal, setShowPostsModal] = useState(false);
    const [isAdmin, setIsAdmin] = useState(true);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const userRole = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
            setIsAdmin(userRole === "Admin");
        }
    }, []);

    if (!isAdmin) {
        return <Navigate to="/home" replace />;
    }

    const adminActions = [
        {
            title: 'Shoes for Approval',
            description: 'Review and manage shoes waiting for approval',
            onClick: () => setShowShoesModal(true),
            icon: '👟'
        },
        {
            title: 'Users Management',
            description: 'View and manage all users',
            onClick: () => setShowUsersModal(true),
            icon: '👥'
        },
        {
            title: 'Posts Management',
            description: 'View and manage all posts',
            onClick: () => setShowPostsModal(true),
            icon: '📝'
        }
    ];

    return (
        <div className="admin-panel-container">
            <TopNavbar />
            <div className="main-content">
                <div className="left-nav">
                    <Navigation />
                </div>
                <div className="admin-content">
                    <h1>Admin Panel</h1>
                    <div className="admin-actions-grid">
                        {adminActions.map((action, index) => (
                            <div 
                                key={index} 
                                className="admin-action-card"
                                onClick={action.onClick}
                            >
                                <div className="action-icon">{action.icon}</div>
                                <h2>{action.title}</h2>
                                <p>{action.description}</p>
                            </div>
                        ))}
                    </div>

                    {showShoesModal && (
                        <AdminShoesModal onClose={() => setShowShoesModal(false)} />
                    )}
                    {showUsersModal && (
                        <AdminShoesModal onClose={() => setShowUsersModal(false)} />
                    )}
                    {showPostsModal && (
                        <AdminShoesModal onClose={() => setShowPostsModal(false)} />
                    )}
                </div>
            </div>
        </div>
    );
};

export default AdminPanel; 