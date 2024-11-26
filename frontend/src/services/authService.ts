import axios from 'axios';

const API_URL = 'http://localhost:5117/api';

interface RegisterData {
    FirstName: string;
    LastName: string;
    Username: string;
    Password: string;
    Email: string;
    Roles: string[];
}

export const authService = {
    register: async (data: RegisterData) => {
        try {
            const response = await axios.post(`${API_URL}/auth/register`, data);
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw new Error('Ha ocurrido un error inesperado durante el registro.');
            }
        }
    },

    verifyEmail: async (Username: string, code: string) => {
        try {
            const response = await axios.get(`${API_URL}/auth/confirm-email`, {
                params: { username: Username, code }
            });
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al verificar el email.';
            } else {
                throw 'Ha ocurrido un error inesperado al verificar el email.';
            }
        }
    },
    resendVerificationCode: async (userName: string) => {
        try {
            const response = await axios.post(`${API_URL}/auth/resend-verification-email`, { userName });
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error reenviando el código de verificación.';
            } else {
                throw 'Ha ocurrido un error inesperado al reenviar el código de verificación.';
            }
        }
    },

    deleteUserFromDB: async (firstName: string, lastName: string, userName: string, email: string, userType: string, deleteToken: string) => {
        try {
            await axios.post(`${API_URL}/auth/delete-unverified-user`, {
                firstName,
                lastName,
                userName,
                email,
                userType,
                deleteToken
            })
        } catch (error) {
            console.error('Error al borrar al usuario:', error);
        }
    },

    login: async (Identifier: string, Password: string) => {
        try {
            const response = await axios.post(`${API_URL}/auth/login`, { Identifier, Password });
            return response.data;;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw new Error('Ha ocurrido un error inesperado al iniciar sesión.');
            }
        }
    }
};
