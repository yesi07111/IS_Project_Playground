import axios from 'axios';
import { ListReservationResponse, ReservationCreationResponse, ReservationFormData } from '../interfaces/Reservation';
import { UserActionResponse } from '../interfaces/Auth';

const API_URL = 'http://localhost:5117/api';

export const reservationService = {

    getAllReservations: async (id: string): Promise<ListReservationResponse> => {
        try {
            const params = new URLSearchParams({ id }).toString();
            const response = await axios.get(`${API_URL}/reservation/get-all?${params}`);
            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al obtener las reservas.';
            } else {
                throw 'Ha ocurrido un error inesperado al obtener las reservas.';
            }
        }
    },
    reserveActivityDate: async (data: ReservationFormData): Promise<ReservationCreationResponse> => {
        try {
            const response = await axios.post(`${API_URL}/reserve/activity`, data);
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw new Error('Ha ocurrido un error inesperado al reservar para esta actividad.');
            }
        }
    },
    cancelReservation: async (activityId: string, userId: string): Promise<UserActionResponse> => {
        try {
            const response = await axios.put(`${API_URL}/reservation/cancel`, { activityId, userId });
            return response.data;
        } catch (error) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al cancelar la reserva.';
            } else {
                throw 'Ha ocurrido un error inesperado al cancelar la reserva.';
            }
        }
    }

};