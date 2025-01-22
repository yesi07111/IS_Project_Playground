import axios from 'axios';
import { DecodedtokenResponse } from '../interfaces/Auth';

const API_URL = 'http://localhost:5117/api';

export const tokenService = {
    getTokenExpirationDate: async (token: string): Promise<Date | null> => {
        try {
            const queryParams = new URLSearchParams({ token }).toString();
            const response = await axios.get(`${API_URL}/auth/decode-token?${queryParams}`);
            const decoded = response.data;

            const expString = decoded.claims.exp;
            if (!expString) {
                return null;
            }

            const exp = parseInt(expString, 10);
            if (isNaN(exp)) {
                return null;
            }

            const date = new Date(0);
            date.setUTCSeconds(exp);
            return date;
        } catch (error) {
            console.error('Error al decodificar el token:', error);
            return null;
        }
    },

    isTokenExpired: async (token: string, thresholdInMinutes: number = 5): Promise<boolean> => {
        const expirationDate = await tokenService.getTokenExpirationDate(token);
        if (!expirationDate) {
            return true;
        }
        const now = new Date();
        const threshold = new Date(now.getTime() + thresholdInMinutes * 60000);
        return expirationDate <= threshold;
    },

    refreshTokenIfNeeded: async (thresholdInMinutes: number = 5) => {
        const token = localStorage.getItem('authToken');
        if (!token) {
            console.error('No se encontró el token de autenticación');
            return;
        }

        if (await tokenService.isTokenExpired(token, thresholdInMinutes)) {
            try {
                const userId = localStorage.getItem('authId');
                const response = await axios.post(`${API_URL}/auth/refresh-token`, { Token: token, UserId: userId });
                const newToken = response.data.token;
                localStorage.setItem('authToken', newToken);
                console.log('Token actualizado exitosamente');
            } catch (error) {
                console.error('Error al actualizar el token:', error);
            }
        }
    },

    startTokenRefreshCheck: (intervalInMinutes: number = 1, thresholdInMinutes: number = 5) => {
        if (refreshIntervalId) {
            clearInterval(refreshIntervalId);
        }
        refreshIntervalId = setInterval(() => {
            tokenService.refreshTokenIfNeeded(thresholdInMinutes);
        }, intervalInMinutes * 60000);
    },

    stopTokenRefreshCheck: () => {
        if (refreshIntervalId) {
            clearInterval(refreshIntervalId);
            refreshIntervalId = null;
        }
    },

    decodeToken: async (token: string): Promise<DecodedtokenResponse | null> => {
        try {
            const queryParams = new URLSearchParams({ token }).toString();
            const response = await axios.get(`${API_URL}/auth/decode-token?${queryParams}`);
            return response.data;
        } catch (error) {
            console.error('Error al decodificar el token:', error);
            return null;
        }
    }
};

let refreshIntervalId: NodeJS.Timeout | null = null;