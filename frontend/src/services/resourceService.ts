import axios from "axios";
import { ResourceFilters } from "../interfaces/ResourceFilters";
import { ListResourceResponse } from "../interfaces/Resource";

const API_URL = 'http://localhost:5117/api';

export const resourceService = {
    getAllResources: async (filters: ResourceFilters[]): Promise<ListResourceResponse> => {
        try {
            if (filters.length === 0 || !filters) {
                const response = await axios.get(`${API_URL}/resource/get-all`);
                return response.data;
            }
            const query = buildFilterQuery(filters);
            console.log('!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!', query);
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
    }
}

function buildFilterQuery(filters: ResourceFilters[]) {
    const params = new URLSearchParams();

    filters.forEach(filter => {
        if (filter.type === 'AllTypes') {
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
