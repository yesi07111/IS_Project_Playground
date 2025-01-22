import axios from 'axios';
import { UserImageUploadResponse } from '../interfaces/User';

const API_URL = 'http://localhost:5117/api';

export const imageUploadService = {
    saveUserImage: async (username: string, image: File): Promise<UserImageUploadResponse> => {
        try {
            const formData = new FormData();
            formData.append('Username', username);
            formData.append('Image', image);

            const response = await axios.post(`${API_URL}/auth/upload-user-image`, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            });
            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al guardar la imagen.';
            } else {
                throw 'Ha ocurrido un error inesperado al guardar la imagen.';
            }
        }
    }
};