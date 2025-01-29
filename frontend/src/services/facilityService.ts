import axios from 'axios';
import { FacilityFilters } from '../interfaces/Filters';
import { Facility, FacilityFormData } from '../interfaces/Facility';
import { responsiveFontSizes } from '@mui/material';

const API_URL = 'http://localhost:5117/api';

export const facilityService = {

    getAllFacilities: async (filters: FacilityFilters) => {
        try {
            if (!filters) {
                const response = await axios.get(`${API_URL}/facility/get-all`);
                return response.data;
            }
            const query = buildFilterQuery(filters);
            const response = await axios.get(`${API_URL}/facility/get-all?${query}`);
            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al obtener las instalaciones.';
            } else {
                throw 'Ha ocurrido un error inesperado al obtener las instalaciones.';
            }
        }
    },

    getFacility: async (id: string) => {
        try {
            const query = new URLSearchParams({ Id: id }).toString();
            const response = await axios.get(`${API_URL}/facility/get?${query}`);
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw new Error('Ha ocurrido un error inesperado al obtener los datos de la instalación.');
            }
        }
    },

    createFacility: async (data: FacilityFormData) => {
        try {
            const response = await axios.post(`${API_URL}/facility/create`, data, {
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            return response.data;
        }
        catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw 'Ha ocurrido un error inesperado al crear la instalación.';
            }
        }
    },


};

function buildFilterQuery(filters: FacilityFilters) {
    const params = new URLSearchParams();
    if (filters.useCase) {
        params.append('useCase', filters.useCase)
    }
    if (filters.name) {
        params.append('name', filters.name);
    }
    if (filters.location) {
        params.append('location', filters.location);
    }
    if (filters.type) {
        params.append('type', filters.type);
    }
    if (filters.maximumCapacity !== undefined) {
        params.append('maximumCapacity', filters.maximumCapacity.toString());
    }
    if (filters.usagePolicy) {
        params.append('usagePolicy', filters.usagePolicy);
    }
    return params.toString();
}
