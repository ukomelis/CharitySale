import axios from 'axios';

const API_BASE_URL = 'http://localhost:8080/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
});

export const getItems = async () => {
  try {
    const response = await apiClient.get('/items');
    return response.data;
  } catch (error) {
    console.error('Error fetching items:', error);
    throw error;
  }
};

export const createSale = async (saleData) => {
  try {
    const response = await apiClient.post('/sales', saleData);
    return response.data;
  } catch (error) {
    console.error('Error creating sale:', error);
    throw error;
  }
};

export const updateItemQuantity = async (itemId, quantity) => {
  try {
    const response = await apiClient.patch(`/items/${itemId}/quantity`, { quantity });
    return response.data;
  } catch (error) {
    console.error('Error updating item quantity:', error);
    throw error;
  }
};  