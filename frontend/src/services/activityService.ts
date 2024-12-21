import axios from 'axios';
import { ActivitiesFilters } from '../interfaces/ActivitiesFilters';

const API_URL = 'http://localhost:5117/api';

export const activityService = {

    getAllActivities: async (filters: ActivitiesFilters[]) => {
        try {
            const query = buildFilterQuery(filters);
            const response = await axios.get(`${API_URL}/activity/get-all?${query}`);
            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al obtener las actividades.';
            } else {
                throw 'Ha ocurrido un error inesperado al obtener las actividades.';
            }
        }
    },

};

function buildFilterQuery(filters: ActivitiesFilters[]) {
    const params = new URLSearchParams();
    filters.forEach(filter => {
        if (filter.type === 'Calificación') {
            params.append('rating', filter.value || '');
        } else if (filter.type === 'Fecha') {
            params.append('startDate', filter.startDate || '');
            params.append('endDate', filter.endDate || '');
        } else if (filter.type === 'Hora') {
            params.append('startTime', filter.startTime || '');
            params.append('endTime', filter.endTime || '');
        } else if (filter.type === 'Educador') {
            params.append('educator', filter.value || '');
        } else if (filter.type === 'Tipo') {
            params.append('type', filter.value || '');
        } else if (filter.type === 'Edad Recomendada') {
            params.append('minAge', filter.minAge?.toString() || '');
            params.append('maxAge', filter.maxAge?.toString() || '');
        } else if (filter.type === 'Disponibilidad') {
            params.append('availability', filter.value || '');
        }
    });
    return params.toString();
}

