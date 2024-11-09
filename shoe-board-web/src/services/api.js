import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7117/api',
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers['Authorization'] = `Bearer ${token}`;
  }
  return config;
}, (error) => {
  return Promise.reject(error);
});

export const login = async (email, password) => {
  const response = await api.post('/User/loginUser', { email, password });
  if (response.data.success) {
    localStorage.setItem('token', response.data.data);
    const payload = JSON.parse(atob(response.data.data.split('.')[1]));
    localStorage.setItem('userId', payload.id);
    localStorage.setItem('tokenExpiration', payload.exp * 1000);
  }
  
  return response.data;
};

export const register = async (username, email, password) => {
  const response = await api.post('/User/registerUser', { username, email, password });
  return response.data;
};

export const getFriendPosts = async () => {
  const response = await api.get('/Post/GetFriendPosts');
  return response.data;
};

export const searchShoes = async (searchTerm, pageNumber = 1) => {
  try {
    const encodedSearchTerm = encodeURIComponent(searchTerm);
    const response = await api.get(`/Shoe/SearchShoe?searchTerm=${encodedSearchTerm}&pageNumber=${pageNumber}`);
    return response.data;
  } catch (error) {
    console.error('Error searching shoes:', error);
    throw error;
  }
};

export const getPopularShoes = async () => {
  const response = await api.get('/Shoe/GetPopularShoes');
  return response.data;
};

export const logout = () => {
  localStorage.removeItem('token');
  localStorage.removeItem('userId');
  localStorage.removeItem('tokenExpiration');
};

export const likePost = async (postId) => {
  try {
    const response = await api.post(`/Post/LikePost?postId=${postId}`);
    console.log('Like response:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error liking post:', error);
    throw error;
  }
};

export const unlikePost = async (postId) => {
  try {
    const response = await api.delete(`/Post/UnlikePost?postId=${postId}`);
    console.log('Unlike response:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error unliking post:', error);
    throw error;
  }
};

export const getUserData = async () => {
  try {
    const userId = localStorage.getItem('userId');
    if (!userId) {
      throw new Error('User ID not found');
    }
    const response = await api.get(`/User/getUser/${userId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching user data:', error);
    throw error;
  }
};

export const updateUserData = async (userData) => {
  try {
    const response = await api.patch('/User/editUserData', userData);
    return response.data;
  } catch (error) {
    console.error('Error updating user data:', error);
    throw error;
  }
};

export const getUserShoes = async () => {
  try {
    const userId = localStorage.getItem('userId');
    if (!userId) {
      throw new Error('User ID not found');
    }
    const response = await api.get(`/Shoe/GetAllUserShoes?userId=${userId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching user shoes:', error);
    throw error;
  }
};

export const deleteUserShoe = async (shoeId) => {
  try {
    const response = await api.delete(`/Shoe/DeleteUserShoe?shoeId=${shoeId}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting user shoe:', error);
    throw error;
  }
};

export const updateUserShoe = async (shoeId, shoeData) => {
  try {
    console.log('Sending update request with data:', {
      shoeId,
      shoeData
    });
    const response = await api.patch(`/Shoe/EditUserShoe?shoeId=${shoeId}`, {
      size: shoeData.size,
      comfortRating: parseInt(shoeData.comfortRating),
      styleRating: parseInt(shoeData.styleRating),
      season: shoeData.season,
      review: shoeData.review
    });
    console.log('Server response:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error updating user shoe:', error.response?.data || error);
    throw error;
  }
};

export const getShoeDetails = async (shoeId) => {
  try {
    const response = await api.get(`/Shoe/GetShoeDetails?shoeId=${shoeId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching shoe details:', error);
    throw error;
  }
};

export const addShoeToUserCollection = async (shoeCatalogId, shoeData) => {
  try {
    const response = await api.post(`/Shoe/SignShoeToUser?shoeCatalogId=${shoeCatalogId}`, {
      size: shoeData.size,
      comfortRating: parseInt(shoeData.comfortRating),
      styleRating: parseInt(shoeData.styleRating),
      season: shoeData.season,
      review: shoeData.review
    });
    return response.data;
  } catch (error) {
    console.error('Error adding shoe to collection:', error);
    throw error;
  }
};

export const getCatalogShoeDetails = async (catalogShoeId) => {
  try {
    const response = await api.get(`/Shoe/GetCatalogShoeDetails?catalogShoeId=${catalogShoeId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching catalog shoe details:', error);
    throw error;
  }
};

export const addPost = async (postData) => {
  try {
    const response = await api.post('/Post/AddPost', postData);
    return response.data;
  } catch (error) {
    console.error('Error adding post:', error);
    throw error;
  }
};

export const getYourPosts = async () => {
  try {
    const response = await api.get('/Post/GetYoursPosts');
    return response.data;
  } catch (error) {
    console.error('Error fetching your posts:', error);
    throw error;
  }
};

export const deletePost = async (postId) => {
  try {
    const response = await api.delete(`/Post/DeletePost?postId=${postId}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting post:', error);
    throw error;
  }
};

export const getComments = async (postId) => {
  try {
    const response = await api.get(`/Post/GetComments?postId=${postId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching comments:', error);
    throw error;
  }
};

export const addComment = async (postId, content) => {
  try {
    const response = await api.post(`/Post/AddComment`, {
      postId: postId,
      content: content
    });
    return response.data;
  } catch (error) {
    console.error('Error adding comment:', error);
    throw error;
  }
};

export default api;
