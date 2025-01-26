import { ReviewsFilters } from "../interfaces/Filters";
import axios from 'axios';
import { ListReviewResponse, ReviewData, } from "../interfaces/Review";

const API_URL = 'http://localhost:5117/api';

export const reviewService = {
    getAllReviews: async (filters: ReviewsFilters[]): Promise<ListReviewResponse> => {
        try {
            const query = buildFilterQuery(filters);
            const response = await axios.get(`${API_URL}/review/get-all?${query}`);
            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al obtener actividades.';
            } else {
                throw 'Ha ocurrido un error inesperado al obtener actividades.';
            }
        }
    },
    createReview: async (data: ReviewData): Promise<{ Response: string }> => {
        try {
            const response = await axios.post(`${API_URL}/review/create`, data)
            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al crear la actividad.';
            } else {
                throw 'Ha ocurrido un error inesperado al crear la actividad.';
            }
        }
    },
    updateReview: async (data: ReviewData): Promise<{ Response: string }> => {
        try {
            const response = await axios.put(`${API_URL}/review/update`, data)
            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al actualizar la actividad.';
            } else {
                throw 'Ha ocurrido un error inesperado al actualizar la actividad.';
            }
        }
    },
    deleteReview: async (reviewId: string, useCase: string): Promise<{ Response: string }> => {
        try {
            const params = new URLSearchParams({ reviewId: reviewId, useCase: useCase }).toString();
            const response = await axios.delete(`${API_URL}/review/delete?${params}`)
            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al eliminar la actividad.';
            } else {
                throw 'Ha ocurrido un error inesperado al eliminar la actividad.';
            }
        }
    },
}


function buildFilterQuery(filters: ReviewsFilters[]) {
    const params = new URLSearchParams();

    filters.forEach(filter => {
        if (filter.type === 'Casos de Uso') {
            params.append('UseCase', filter.useCase || '')
        }
        if (filter.type === 'UserId') {
            params.append('UserId', filter.value || '')
        }
    })
    return params.toString()
}