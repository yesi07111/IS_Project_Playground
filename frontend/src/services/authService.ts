import axios from 'axios';

const API_URL = 'http://localhost:5117/api'; // Ajusta según tu configuración

interface RegisterData {
    firstName: string;
    lastName: string;
    email: string;
    userName: string;
    password: string;
}

interface LoginData {
    userNameOrEmail: string;
    password: string;
}

interface AuthResponse {
    id: string;
    userName: string;
    token?: string;
}

export const authService = {
    async register(data: RegisterData): Promise<AuthResponse> {
        try {
            const response = await axios.post(`${API_URL}/auth/register`, data);
            return response.data;
        } catch (error: any) {
            throw this.handleError(error);
        }
    },

    async login(data: LoginData): Promise<AuthResponse> {
        try {
            const response = await axios.post(`${API_URL}/login`, data);
            if (response.data.token) {
                localStorage.setItem('token', response.data.token);
                localStorage.setItem('user', JSON.stringify(response.data));
            }
            return response.data;
        } catch (error: any) {
            throw this.handleError(error);
        }
    },

    async verifyEmail(username: string, code: string): Promise<AuthResponse> {
        try {
            const response = await axios.get(
                `${API_URL}/auth/confirm-email?username=${username}&code=${code}`
            );
            if (response.data.token) {
                localStorage.setItem('token', response.data.token);
                localStorage.setItem('user', JSON.stringify(response.data));
            }
            return response.data;
        } catch (error: any) {
            throw this.handleError(error);
        }
    },

    logout(): void {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
    },

    getCurrentUser(): AuthResponse | null {
        const userStr = localStorage.getItem('user');
        if (userStr) {
            return JSON.parse(userStr);
        }
        return null;
    },

    isAuthenticated(): boolean {
        return !!localStorage.getItem('token');
    },

    handleError(error: any): Error {
        if (error.response?.data?.detail) {
            return new Error(error.response.data.detail);
        }
        return new Error('An unexpected error occurred');
    }
};