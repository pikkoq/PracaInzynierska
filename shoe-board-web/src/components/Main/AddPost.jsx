import React, { useState, useEffect } from 'react';
import { getUserShoes, addPost } from '../../services/api';
import './AddPost.scss';

const AddPost = () => {
    const [content, setContent] = useState('');
    const [shoeId, setShoeId] = useState('');
    const [userShoes, setUserShoes] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const MAX_CONTENT_LENGTH = 500;
    const MAX_LINES = 5;

    useEffect(() => {
        const fetchUserShoes = async () => {
            try {
                const response = await getUserShoes();
                if (response.success) {
                    setUserShoes(response.data);
                }
            } catch (error) {
                console.error('Error fetching user shoes:', error);
                setError('Nie udało się pobrać butów z kolekcji');
            }
        };

        fetchUserShoes();
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!shoeId || !content.trim()) {
            setError('Wybierz buta i napisz treść posta');
            return;
        }

        setLoading(true);
        setError('');
        setSuccessMessage('');

        try {
            const response = await addPost({
                shoeId: parseInt(shoeId),
                content: content.trim()
            });

            if (response.success) {
                setContent('');
                setShoeId('');
                setSuccessMessage('Post został dodany pomyślnie!');
                setTimeout(() => {
                    setSuccessMessage('');
                }, 3000);
            } else {
                setError('Nie udało się dodać posta');
            }
        } catch (error) {
            setError('Wystąpił błąd podczas dodawania posta');
            console.error('Error adding post:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleContentChange = (e) => {
        const newContent = e.target.value;
        const lines = newContent.split('\n');
        
        if (lines.length <= MAX_LINES) {
            if (newContent.length <= MAX_CONTENT_LENGTH) {
                setContent(newContent);
            }
        }
    };

    return (
        <div className="add-post-container">
            <h3>Add new post</h3>
            {error && <div className="error-message">{error}</div>}
            {successMessage && <div className="success-message">{successMessage}</div>}
            <form onSubmit={handleSubmit}>
                <select
                    value={shoeId}
                    onChange={(e) => setShoeId(e.target.value)}
                    disabled={loading}
                    className="shoe-select"
                >
                    <option value="">Select shoe</option>
                    {userShoes.map((shoe) => (
                        <option key={shoe.id} value={shoe.id}>
                            {shoe.title}
                        </option>
                    ))}
                </select>
                <div className="form-group">
                    <label htmlFor="content">
                        Content
                        <span className="content-limit">
                            ({content.length || 0}/{MAX_CONTENT_LENGTH})
                        </span>
                    </label>
                    <textarea
                        id="content"
                        value={content}
                        onChange={handleContentChange}
                        maxLength={MAX_CONTENT_LENGTH}
                        rows={5}
                        placeholder="What is on your mind?"
                        disabled={loading}
                        className="add-post-content"
                    />
                </div>
                <button 
                    type="submit" 
                    disabled={loading || !shoeId || !content.trim()}
                    className="submit-button"
                >
                    {loading ? 'Adding...' : 'Add post'}
                </button>
            </form>
        </div>
    );
};

export default AddPost; 