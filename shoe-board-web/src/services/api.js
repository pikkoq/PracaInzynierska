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

export const loginUser = async (login, password) => {
  const response = await api.post('/User/loginUser', { login, password });
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

export const getFriendPosts = async (pageNumber = 1) => {
  const response = await api.get(`/Post/GetFriendPosts?pageNumber=${pageNumber}`);
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

export const getFriends = async () => {
    try {
        const response = await api.get('/Friend/GetFreinds');
        return response.data;
    } catch (error) {
        console.error('Error fetching friends:', error);
        return { success: false, error: error.response?.data || 'Failed to fetch friends' };
    }
};

export const getFriendRequests = async () => {
    try {
        const response = await api.get('/Friend/GetFriendRequests');
        return response.data;
    } catch (error) {
        console.error('Error fetching friend requests:', error);
        return { success: false, error: error.response?.data || 'Failed to fetch friend requests' };
    }
};

export const getSentFriendRequests = async () => {
    try {
        const response = await api.get('/Friend/GetSentFriendRequests');
        return response.data;
    } catch (error) {
        console.error('Error fetching sent friend requests:', error);
        return { success: false, error: error.response?.data || 'Failed to fetch sent friend requests' };
    }
};

export const acceptFriendRequest = async (requestId) => {
    try {
        const response = await api.post(`/Friend/AcceptFriendRequest?requestId=${requestId}`);
        return response.data;
    } catch (error) {
        console.error('Error accepting friend request:', error);
        return { success: false, error: error.response?.data || 'Failed to accept friend request' };
    }
};

export const searchFriends = async (searchTerm) => {
    try {
        const response = await api.get(`/Friend/SearchFriends?searchTerm=${encodeURIComponent(searchTerm)}`);
        return response.data;
    } catch (error) {
        console.error('Error searching friends:', error);
        return { success: false, error: error.response?.data || 'Failed to search friends' };
    }
};

export const sendFriendRequest = async (receiverId) => {
    try {
        const response = await api.post(`/Friend/SendFriendRequest?receiverId=${receiverId}`);
        return response.data;
    } catch (error) {
        console.error('Error sending friend request:', error);
        return { success: false, error: error.response?.data || 'Failed to send friend request' };
    }
};

export const declineFriendRequest = async (requestId) => {
    try {
        const response = await api.delete(`/Friend/CancelFriendRequest?requestId=${requestId}`);
        return response.data;
    } catch (error) {
        console.error('Error declining friend request:', error);
        return { success: false, error: error.response?.data || 'Failed to decline friend request' };
    }
};

export const deleteFriend = async (friendId) => {
    try {
        const response = await api.delete(`/Friend/DeleteFriend?friendId=${friendId}`);
        return response.data;
    } catch (error) {
        console.error('Error deleting friend:', error);
        return { success: false, error: error.response?.data || 'Failed to delete friend' };
    }
};

export const rejectFriendRequest = async (requestId) => {
    try {
        const response = await api.delete(`/Friend/DeclineFriendRequest?requestId=${requestId}`);
        return response.data;
    } catch (error) {
        console.error('Error rejecting friend request:', error);
        return { success: false, error: error.response?.data || 'Failed to reject friend request' };
    }
};

export const changeUserPassword = async (passwordData) => {
  try {
    const response = await api.patch('/User/changeUserPassword', {
      currentPassword: passwordData.currentPassword,
      newPassword: passwordData.newPassword
    });
    return response.data;
  } catch (error) {
    console.error('Error changing password:', error);
    throw error;
  }
};

export const getUserProfile = async (userName) => {
  try {
    const response = await api.get(`/User/getUserProfile?userName=${encodeURIComponent(userName)}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching user profile:', error);
    throw error;
  }
};

export const getShoesToAccept = async (pageNumber = 1) => {
  try {
    const response = await api.get(`/Admin/GetShoesToAccept?pageNumber=${pageNumber}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching shoes to accept:', error);
    throw error;
  }
};

export const acceptShoe = async (shoeId) => {
  try {
    const response = await api.post(`/Admin/AcceptNewAddedShoes?shoeId=${shoeId}`);
    return response.data;
  } catch (error) {
    console.error('Error accepting shoe:', error);
    throw error;
  }
};

export const declineShoe = async (shoeId) => {
  try {
    const response = await api.delete(`/Admin/DeclineNewAddedShoes?shoeId=${shoeId}`);
    return response.data;
  } catch (error) {
    console.error('Error declining shoe:', error);
    throw error;
  }
};

export const editNewShoe = async (shoeId, shoeData) => {
  try {
    const response = await api.patch(`/Admin/EditNewAddedShoes?shoeId=${shoeId}`, shoeData);
    return response.data;
  } catch (error) {
    console.error('Error editing shoe:', error);
    throw error;
  }
};

export const getAllUsers = async (pageNumber = 1) => {
  try {
    const response = await api.get(`/Admin/GetAllUsers?pageNumber=${pageNumber}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching users:', error);
    throw error;
  }
};

export const deleteUser = async (userId) => {
  try {
    const response = await api.delete(`/Admin/DeleteUserAccount?userId=${userId}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting user:', error);
    throw error;
  }
};

export const editUser = async (userId, userData) => {
  try {
    const response = await api.patch(`/Admin/EditUserAccount?userId=${userId}`, userData);
    return response.data;
  } catch (error) {
    console.error('Error editing user:', error);
    throw error;
  }
};

export const getAllPosts = async (pageNumber = 1) => {
  try {
    const response = await api.get(`/Admin/GetAllUsersPosts?pageNumber=${pageNumber}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching all posts:', error);
    throw error;
  }
};

export const editPost = async (postId, content) => {
  try {
    const response = await api.patch(`/Admin/EditUserPost?postId=${postId}`, JSON.stringify(content), {
      headers: {
        'Content-Type': 'application/json'
      }
    });
    return response.data;
  } catch (error) {
    console.error('Error editing post:', error);
    throw error;
  }
};

export const deletePostAdmin = async (postId) => {
  try {
    const response = await api.delete(`/Admin/DeleteUserPost?postId=${postId}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting post:', error);
    throw error;
  }
};

export const registerNewShoe = async (shoeData) => {
  try {
    const formData = new FormData();
    formData.append('Gender', shoeData.gender);
    formData.append('Release_Date', shoeData.releaseDate);
    formData.append('Brand', shoeData.brand);
    formData.append('Price', shoeData.price);
    formData.append('Colorway', shoeData.colorway);
    formData.append('Nickname', shoeData.nickname);
    formData.append('ImageFile', shoeData.imageFile);
    formData.append('Series', shoeData.series);
    formData.append('Model_No', shoeData.modelNo);
    formData.append('Title', shoeData.title);
    formData.append('ShopUrl', shoeData.shopUrl);
    formData.append('Main_Color', shoeData.mainColor);

    const response = await api.post('/Shoe/RegisterNewShoe', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data;
  } catch (error) {
    console.error('Error registering new shoe:', error);
    throw error;
  }
};

export default api;
