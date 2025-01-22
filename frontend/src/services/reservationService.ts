import axios from 'axios';
import { ListReservationResponse } from '../interfaces/Reservation';

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

};