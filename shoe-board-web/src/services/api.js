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
  console.log('API response in getFriendPosts:', response.data);
  return response.data;
};

export const searchShoes = async (query) => {
  const response = await api.get(`/Shoe/Search?query=${query}`);
  return response.data;
};

export const getPopularShoes = async () => {
  const response = await api.get('/Shoe/Popular');
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

export default api;
