import axios from "axios";
import { ListResourceDateResponse, ListResourceResponse, ResourceFormData, UpdateResourceFormData } from "../interfaces/Resource";
import { ResourceFilters } from "../interfaces/Filters";
import { ResourceDate } from "../interfaces/ResourceDate";

const API_URL = 'http://localhost:5117/api';

export const resourceService = {
    getAllResources: async (filters: ResourceFilters[]): Promise<ListResourceResponse> => {
        try {
            if (filters.length === 0 || !filters) {
                const response = await axios.get(`${API_URL}/resource/get-all`);
                return response.data;
            }
            const query = buildFilterQuery(filters);
            const response = await axios.get(`${API_URL}/resource/get-all?${query}`);
            return response.data;
        }
        catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al obtener los recursos.';
            } else {
                throw 'Ha ocurrido un error inesperado al obtener los recursos.';
            }
        }
    },

    getAllResourceDates: async (useCase: string): Promise<ListResourceDateResponse> => {
        try {
            const params = new URLSearchParams();
            if (useCase === 'NameDayFreq') {
                params.append('useCase', useCase);
            }
            else if (useCase === 'NameFreqMost') {
                params.append('useCase', useCase);
            }
            else if (useCase === 'NameFreqLess') {
                params.append('useCase', useCase);
            }
            const query = params.toString();
            const response = await axios.get(`${API_URL}/resourceDate/get-all?${query}`);
            return response.data;
        }
        catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al obtener las frecuencias de uso.';
            } else {
                throw 'Ha ocurrido un error inesperado al obtener las frecuencias de uso.';
            }
        }
    },

    saveUsageFrequency: async (data: ResourceDate) => {
        try {
            const response = await axios.post(`${API_URL}/resourceDate/create`, data, {
                headers: {
                    'Content-Type': 'application/json',
                },
            });
            console.log(response.data)
            return response.data;

        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw 'Ha ocurrido un error inesperado al guardar la frecuencia de uso.';
            }
        }
    },

    removeResourceDate: async (id: string, date: Date) => {
        try {
            const params = new URLSearchParams({ resourceId: id, date: date.toISOString() }).toString();
            const response = await axios.delete(`${API_URL}/resourceDate/delete?${params}`)
            return response.data;
        }
        catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw 'Ha ocurrido un error inesperado al eliminar la frecuencia de uso.';
            }
        }
    },

    createResource: async (data: ResourceFormData) => {
        try {
            const response = await axios.post(`${API_URL}/resource/create`, data);
            console.log(response.data)
            return response.data;
        }
        catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw 'Ha ocurrido un error inesperado al crear el recurso.';
            }
        }
    },

    updateResource: async (data: UpdateResourceFormData) => {
        try {
            const response = await axios.put(`${API_URL}/resource/update`, data);
            console.log(response.data)
            return response.data;
        }
        catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw 'Ha ocurrido un error inesperado al modificar el recurso.';
            }
        }
    },

    removeResource: async (id: string) => {
        try {
            const params = new URLSearchParams({ id: id }).toString();
            const response = await axios.delete(`${API_URL}/resource/delete?${params}`)
            return response.data;
        }
        catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw 'Ha ocurrido un error inesperado al eliminar el recurso.';
            }
        }
    }
}

function buildFilterQuery(filters: ResourceFilters[]) {
    const params = new URLSearchParams();

    filters.forEach(filter => {
        if (filter.type === 'AllTypes') {
            params.append('useCase', filter.type);
        }
        else if (filter.type === 'NameFreq') {
            params.append('useCase', filter.type);
        }
        else if (filter.type === 'ConditionFreq') {
            params.append('useCase', filter.type);
        }
        if (filter.type === 'Tipos de Instalaciones') {
            params.append('facilityTypes', filter.value || '');
        }
        else if (filter.type === 'Tipos de Recursos') {
            params.append('resourceTypes', filter.value || '');
        }
        else if (filter.type === 'Nombre') {
            params.append('name', filter.value || '');
        }
        else if (filter.type === 'Estado') {
            params.append('condition', filter.value || '');
        }
        else if (filter.type === 'Rango de Frecuencia de uso') {
            if (filter.minUseFrequency) {
                params.append('minUseFrequency', filter.minUseFrequency?.toString() || '');
            }
            if (filter.maxUseFrequency) {
                params.append('maxUseFrequency', filter.maxUseFrequency?.toString() || '');
            }
        }
    });

    return params.toString();
}
