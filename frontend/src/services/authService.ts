import axios from 'axios';
import { RegisterData } from '../interfaces/RegisterData';

const API_URL = 'http://localhost:5117/api';


export const authService = {
    /**
     * Registra un nuevo usuario en el sistema.
     * 
     * Este método envía una solicitud POST al endpoint de registro con los datos del usuario.
     * Si la solicitud es exitosa, devuelve los datos de la respuesta. En caso de error, lanza
     * un mensaje de error específico si es un error de Axios, o un mensaje genérico si es otro
     * tipo de error.
     * 
     * @param {RegisterData} data - Datos del usuario a registrar, incluyendo nombre, apellido, 
     * nombre de usuario, contraseña, correo electrónico y roles.
     * @returns {Promise<any>} Los datos de la respuesta del servidor.
     * @throws {Error} Si ocurre un error durante el registro.
     */
    register: async (data: RegisterData) => {
        try {
            const response = await axios.post(`${API_URL}/auth/register`, data);
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw new Error('Ha ocurrido un error inesperado durante el registro.');
            }
        }
    },

    /**
     * Verifica el correo electrónico de un usuario.
     * 
     * Este método envía una solicitud GET al endpoint de confirmación de correo electrónico
     * con el nombre de usuario y el código de verificación. Si la solicitud es exitosa, devuelve
     * los datos de la respuesta. En caso de error, lanza un mensaje de error específico si es un
     * error de Axios, o un mensaje genérico si es otro tipo de error.
     * 
     * @param {string} Username - Nombre de usuario del usuario a verificar.
     * @param {string} code - Código de verificación enviado al correo del usuario.
     * @returns {Promise<any>} Los datos de la respuesta del servidor.
     * @throws {Error} Si ocurre un error durante la verificación del correo.
     */
    verifyEmail: async (Username: string, code: string) => {
        try {
            const response = await axios.get(`${API_URL}/auth/confirm-email`, {
                params: { username: Username, code }
            });
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al verificar el email.';
            } else {
                throw 'Ha ocurrido un error inesperado al verificar el email.';
            }
        }
    },

    /**
     * Reenvía el código de verificación al correo electrónico del usuario.
     * 
     * Este método envía una solicitud POST al endpoint de reenvío de código de verificación
     * con el nombre de usuario. Si la solicitud es exitosa, devuelve los datos de la respuesta.
     * En caso de error, lanza un mensaje de error específico si es un error de Axios, o un mensaje
     * genérico si es otro tipo de error.
     * 
     * @param {string} userName - Nombre de usuario del usuario al que se le reenviará el código.
     * @returns {Promise<any>} Los datos de la respuesta del servidor.
     * @throws {Error} Si ocurre un error al reenviar el código de verificación.
     */
    resendVerificationCode: async (userName: string) => {
        try {
            const response = await axios.post(`${API_URL}/auth/resend-verification-email`, { userName });
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error reenviando el código de verificación.';
            } else {
                throw 'Ha ocurrido un error inesperado al reenviar el código de verificación.';
            }
        }
    },

    /**
     * Elimina un usuario no verificado de la base de datos.
     * 
     * Este método envía una solicitud POST al endpoint de eliminación de usuario no verificado
     * con los datos del usuario. Si la solicitud es exitosa, no devuelve nada. En caso de error,
     * imprime un mensaje de error en la consola.lo devuelve.
     * 
     * @param {string} firstName - Nombre del usuario.
     * @param {string} lastName - Apellido del usuario.
     * @param {string} userName - Nombre de usuario.
     * @param {string} email - Correo electrónico del usuario.
     * @param {string} userType - Tipo de usuario.
     * @param {string} deleteToken - Token de eliminación.
     */
    deleteUserFromDB: async (deleteToken: string, firstName: string, lastName: string, userName: string, email: string, userType: string) => {
        try {
            await axios.delete(`${API_URL}/auth/delete-fail-user/${deleteToken}/${firstName}/${lastName}/${userName}/${email}/${userType}`);
        } catch (error) {
            console.error('Error al borrar al usuario:', error);
        }
    },
    /**
     * Inicia sesión en el sistema.
     * 
     * Este método envía una solicitud POST al endpoint de inicio de sesión con el identificador
     * y la contraseña del usuario. Si la solicitud es exitosa, devuelve los datos de la respuesta.
     * En caso de error, lanza un mensaje de error específico si es un error de Axios, o un mensaje
     * genérico si es otro tipo de error.
     * 
     * @param {string} Identifier - Identificador del usuario (puede ser nombre de usuario o correo electrónico).
     * @param {string} Password - Contraseña del usuario.
     * @returns {Promise<any>} Los datos de la respuesta del servidor.
     * @throws {Error} Si ocurre un error durante el inicio de sesión.
     */
    login: async (Identifier: string, Password: string) => {
        try {
            const response = await axios.post(`${API_URL}/auth/login`, { Identifier, Password });
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw new Error('Ha ocurrido un error inesperado al iniciar sesión.');
            }
        }
    },

    /**
     * Verifica el estado del correo electrónico del usuario actual.
     * 
     * Este método envía una solicitud POST al endpoint de verificación de estado de correo
     * con el token, el ID y el nombre de usuario. Si la solicitud es exitosa, devuelve los
     * datos de la respuesta. En caso de error, lanza un mensaje de error específico si es un
     * error de Axios, o un mensaje genérico si es otro tipo de error.
     * 
     * @param {string} Token - Token de autenticación del usuario.
     * @param {string} Id - ID del usuario.
     * @param {string} UserName - Nombre de usuario.
     * @returns {Promise<any>} Los datos de la respuesta del servidor.
     * @throws {Error} Si ocurre un error al verificar el estado del correo.
     */
    checkVerifiedEmail: async (Token: string, Id: string, UserName: string) => {
        try {
            const response = await axios.post(`${API_URL}/auth/check-email`, { Token, Id, UserName });
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw new Error('Ha ocurrido un error inesperado al verificar el estado del correo del usuario actual.');
            }
        }
    }
};
