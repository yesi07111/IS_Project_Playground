import axios from 'axios';
import { UserFilters } from '../interfaces/UserFilters';

const API_URL = 'http://localhost:5117/api';

export const userService = {

    getAllUsers: async (filters: UserFilters) => {
        try {
            if (!filters) {
                const response = await axios.get(`${API_URL}/user/get-all`);
                return response.data;
            }
            const query = buildFilterQuery(filters);
            const response = await axios.get(`${API_URL}/user/get-all?${query}`);
            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al obtener los usuarios.';
            } else {
                throw 'Ha ocurrido un error inesperado al obtener los usuarios.';
            }
        }
    },

};

function buildFilterQuery(filters: UserFilters) {
    const params = new URLSearchParams();
    if (filters.useCase) {
        params.append('useCase', filters.useCase)
    }
    if (filters.userName) {
        params.append('userName', filters.userName);
    }
    if (filters.email) {
        params.append('email', filters.email);
    }
    if (filters.emailConfirmed) {
        params.append('emailConfirmed', filters.emailConfirmed);
    }
    if (filters.firstName) {
        params.append('firstName', filters.firstName);
    }
    if (filters.lastName) {
        params.append('lastName', filters.lastName);
    }
    if (filters.rol) {
        params.append('rol', filters.rol);
    }
    if (filters.markDeleted !== undefined) {
        params.append('markDeleted', filters.markDeleted.toString());
    }
    return params.toString();
}