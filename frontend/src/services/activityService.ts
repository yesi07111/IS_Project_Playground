import axios from 'axios';
import { ActivitiesFilters } from '../interfaces/Filters';
import { ActivityData, GetActivityResponse, ListActivityResponse } from '../interfaces/Activity';

const API_URL = 'http://localhost:5117/api';

export const activityService = {
    getAllActivities: async (filters: ActivitiesFilters[]): Promise<ListActivityResponse> => {
        try {
            if (filters.some(filter => filter.type === 'Casos de Uso' && filter.useCase === 'ActivityView')) {
                const query = buildFilterQuery(filters);
                const response = await axios.get(`${API_URL}/activity/get-all?${query}`);
                return response.data;
            }
            const query = buildFilterQuery(filters);
            const response = await axios.get(`${API_URL}/activity/get-all?${query}`);
            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al obtener actividades.';
            } else {
                throw 'Ha ocurrido un error inesperado al obtener actividades.';
            }
        }
    },

    getActivity: async (id: string, useCase: string): Promise<GetActivityResponse> => {
        try {
            const query = new URLSearchParams({ Id: id, UseCase: useCase }).toString();
            const response = await axios.get(`${API_URL}/activity/get?${query}`);

            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al obtener los detalles de la actividad.';
            } else {
                throw 'Ha ocurrido un error inesperado al obtener los detalles de la actividad.';
            }
        }
    },

    createActivity: async (activityData: ActivityData) => {
        try {
            console.table(activityData);
            const response = await axios.post(`${API_URL}/activity/post`, activityData);
            console.log(response);
        }
        catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw new Error('Ha ocurrido un error inesperado durante la creacion de actividad.');
            }
        }
    }
};

function buildFilterQuery(filters: ActivitiesFilters[]) {
    const params = new URLSearchParams();
    let startDateTime = '';
    let endDateTime = '';
    let startTime = '';
    let endTime = '';

    filters.forEach(filter => {
        if (filter.type === 'Casos de Uso') {
            params.append('UseCase', filter.useCase || '')
        }
        if (filter.type === 'UserId') {
            params.append('UserId', filter.value || '')
        }
        if (filter.type === 'Nueva') {
            params.append('IsNew', filter.value || '')
        }
        if (filter.type === 'Calificación') {
            params.append('rating', filter.value || '');
        } else if (filter.type === 'Rango de Fecha') {
            // Combina fecha y hora si ambas están presentes
            if (filter.startDate) {
                const startDate = new Date(filter.startDate);
                if (filter.startTime) {
                    const [hours, minutes] = filter.startTime.split(':');
                    startDate.setHours(parseInt(hours, 10), parseInt(minutes, 10));
                }
                startDateTime = `${startDate.getFullYear()}-${(startDate.getMonth() + 1).toString().padStart(2, '0')}-${startDate.getDate().toString().padStart(2, '0')}T${startDate.getHours().toString().padStart(2, '0')}:${startDate.getMinutes().toString().padStart(2, '0')}:00`;
            }
            if (filter.endDate) {
                const endDate = new Date(filter.endDate);
                if (filter.endTime) {
                    const [hours, minutes] = filter.endTime.split(':');
                    endDate.setHours(parseInt(hours, 10), parseInt(minutes, 10));
                }
                endDateTime = `${endDate.getFullYear()}-${(endDate.getMonth() + 1).toString().padStart(2, '0')}-${endDate.getDate().toString().padStart(2, '0')}T${endDate.getHours().toString().padStart(2, '0')}:${endDate.getMinutes().toString().padStart(2, '0')}:00`;
            }
        } else if (filter.type === 'Rango de Hora') {
            if ((!startDateTime || startDateTime == '') && filter.startTime) {
                const [hours, minutes] = filter.startTime.split(':');
                startTime = `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}:00`;
            }
            if ((!endDateTime || endDateTime == '') && filter.endTime) {
                const [hours, minutes] = filter.endTime.split(':');
                endTime = `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}:00`;
            }
        } else if (filter.type === 'Educadores') {
            params.append('educators', filter.value || '');
        } else if (filter.type === 'Tipos de Instalaciones') {
            params.append('facilityTypes', filter.value || '');
        } else if (filter.type === 'Tipos de Actividades') {
            params.append('activityTypes', filter.value || '');
        } else if (filter.type === 'Edad Recomendada') {
            params.append('minAge', filter.minAge?.toString() || '');
            params.append('maxAge', filter.maxAge?.toString() || '');
        } else if (filter.type === 'Disponibilidad') {
            params.append('availability', filter.value || '');
        } else if (filter.type === 'Hoy' && filter.value) {
            params.append('Today', 'true');
        } else if (filter.type === 'Mañana' && filter.value) {
            params.append('tomorrow', 'true');
        } else if (filter.type === 'Esta Semana' && filter.value) {
            params.append('thisWeek', 'true');
        } else if (filter.type === 'Días de la Semana' && filter.value) {
            params.append('daysOfWeek', filter.value);
        } else if (filter.type === 'Capacidad' && filter.value) {
            params.append('capacity', filter.value)
        }
    });

    if (startDateTime) {
        params.append('startDateTime', startDateTime);
    }
    if (endDateTime) {
        params.append('endDateTime', endDateTime);
    }
    if (startTime) {
        params.append('StartTime', startTime);
    }
    if (endTime) {
        params.append('EndTime', endTime);
    }

    return params.toString();
}