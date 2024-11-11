import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5000/api', // Cambiar esto por la baseURL correcta
});

export const fetchActivities = async () => {
    const response = await api.get('/activities'); // Asegurar que la API tenga esta ruta
    return response.data;
};

export const fetchResources = async () => {
    const response = await api.get('/resources'); // Asegurar que la API tenga esta ruta
    return response.data;
};