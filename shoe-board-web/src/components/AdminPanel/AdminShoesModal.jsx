import React from 'react';
import './AdminShoesModals.scss';

const AdminShoesModal = ({ onClose }) => {
    return (
        <div className="admin-modal-overlay" onClick={onClose}>
            <div className="admin-modal-content" onClick={e => e.stopPropagation()}>
                <div className="admin-modal-header">
                    <h2>Shoes for Approval</h2>
                    <button className="close-button" onClick={onClose}>&times;</button>
                </div>
                <div className="admin-modal-body">
                </div>
            </div>
        </div>
    );
};

export default AdminShoesModal; 