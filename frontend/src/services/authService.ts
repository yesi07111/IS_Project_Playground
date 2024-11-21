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
                // Si el error es un AxiosError y tiene una respuesta
                throw error.response.data || 'Error during registration';
            } else {
                // Si el error no es un AxiosError o no tiene respuesta
                throw 'An unexpected error occurred during registration';
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
                throw error.response.data || 'Error during email verification';
            } else {
                throw 'An unexpected error occurred during email verification';
            }
        }
    },
    resendVerificationCode: async (email: string) => {
        try {
            const response = await axios.post(`${API_URL}/auth/resend-verification-email`, { email });
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error resending verification code';
            } else {
                throw 'An unexpected error occurred while resending verification code';
            }
        }
    },

    changeEmail: async (email: string) => {
        try {
            const response = await axios.post(`${API_URL}/auth/change-profile`, { email: email });
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error changing email';
            } else {
                throw 'An unexpected error occurred while changing email';
            }
        }
    }
};
