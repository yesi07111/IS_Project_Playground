import axios from 'axios';
import {
    DeleteFailUserResponse, GoogleAccessData, RegisterData, UserActionResponse, GoogleTokenValidationResponse,
    CheckEmailResponse, ReCaptchaVerificationResponse, ReCaptchaSiteKeyResponse, GoogleClientIdResponse,
} from '../interfaces/Auth';
import { UserCreationResponse } from '../interfaces/Auth';
import { GetHomePageInfoResponse } from '../interfaces/Pages';

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
    register: async (data: RegisterData): Promise<UserActionResponse> => {
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
    verifyEmail: async (Username: string, code: string): Promise<UserActionResponse> => {
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
     * Este método envía una solicitud GET al endpoint de reenvío de código de verificación
     * con el nombre de usuario. Si la solicitud es exitosa, devuelve los datos de la respuesta.
     * En caso de error, lanza un mensaje de error específico si es un error de Axios, o un mensaje
     * genérico si es otro tipo de error.
     * 
     * @param {string} userName - Nombre de usuario del usuario al que se le reenviará el código.
     * @returns {Promise<any>} Los datos de la respuesta del servidor.
     * @throws {Error} Si ocurre un error al reenviar el código de verificación.
     */
    resendVerificationCode: async (userName: string): Promise<UserCreationResponse> => {
        try {
            // Construir la URL con los parámetros de consulta
            const queryParams = new URLSearchParams({ userName }).toString();
            const response = await axios.get(`${API_URL}/auth/resend-verification-email?${queryParams}`);
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
     * Este método envía una solicitud DELETE al endpoint de eliminación de usuario no verificado
     * con los datos del usuario como parámetros de consulta. Si la solicitud es exitosa, no devuelve nada.
     * En caso de error, imprime un mensaje de error en la consola.
     * 
     * @param {string} id - ID del usuario.
     * @param {string} firstName - Nombre del usuario.
     * @param {string} lastName - Apellido del usuario.
     * @param {string} userName - Nombre de usuario.
     * @param {string} email - Correo electrónico del usuario.
     * @param {string} userType - Tipo de usuario.
     */
    deleteUserFromDB: async (id: string, firstName: string, lastName: string, userName: string, email: string, userType: string): Promise<DeleteFailUserResponse> => {
        try {
            // Construir la URL con los parámetros de consulta
            const queryParams = new URLSearchParams({
                id,
                firstName,
                lastName,
                userName,
                email,
                userType
            }).toString();

            // Hacer la solicitud DELETE con los parámetros de consulta
            const response = await axios.delete(`${API_URL}/auth/delete-fail-user?${queryParams}`);
            return response.data;

        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error eliminando al usuario.';
            } else {
                throw 'Ha ocurrido un error inesperado al intentar eliminar al usuario.';
            }
        }
    },
    /**
     * Inicia sesión en el sistema.
     * 
     * Este método envía una solicitud GET al endpoint de inicio de sesión con el identificador
     * y la contraseña del usuario. Si la solicitud es exitosa, devuelve los datos de la respuesta.
     * En caso de error, lanza un mensaje de error específico si es un error de Axios, o un mensaje
     * genérico si es otro tipo de error.
     * 
     * @param {string} Identifier - Identificador del usuario (puede ser nombre de usuario o correo electrónico).
     * @param {string} Password - Contraseña del usuario.
     * @returns {Promise<UserResponse>} Los datos de la respuesta del servidor.
     * @throws {Error} Si ocurre un error durante el inicio de sesión.
     */
    login: async (Identifier: string, Password: string): Promise<UserActionResponse> => {
        try {
            // Construir la URL con los parámetros de consulta
            const queryParams = new URLSearchParams({
                Identifier,
                Password
            }).toString();

            const response = await axios.get(`${API_URL}/auth/login?${queryParams}`);
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
    * Este método envía una solicitud GET al endpoint de verificación de estado de correo
    * con el token, el ID y el nombre de usuario como parámetros de consulta. Si la solicitud
    * es exitosa, devuelve los datos de la respuesta. En caso de error, lanza un mensaje de error
    * específico si es un error de Axios, o un mensaje genérico si es otro tipo de error.
    * 
    * @param {string} Token - Token de autenticación del usuario.
    * @param {string} Id - ID del usuario.
    * @param {string} Username - Nombre de usuario.
    * @returns {Promise<any>} Los datos de la respuesta del servidor.
    * @throws {Error} Si ocurre un error al verificar el estado del correo.
    */
    checkVerifiedEmail: async (Token: string, Id: string, Username: string): Promise<CheckEmailResponse> => {
        try {
            // Construir la URL con los parámetros de consulta
            const queryParams = new URLSearchParams({
                Token,
                Id,
                Username
            }).toString();

            // Hacer la solicitud GET con los parámetros de consulta
            const response = await axios.get(`${API_URL}/auth/check-email?${queryParams}`);
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data;
            } else {
                throw new Error('Ha ocurrido un error inesperado al verificar el estado del correo del usuario actual.');
            }
        }
    },

    resetPassword: async (identifier: string, reducedCode: string, fullCode: string, newPassword: string): Promise<UserCreationResponse> => {
        try {
            const response = await axios.post(`${API_URL}/auth/reset-password`, { identifier: identifier, reducedCode: reducedCode, fullCode: fullCode, newPassword: newPassword });
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al verificar el email.';
            } else {
                throw 'Ha ocurrido un error inesperado al verificar el email.';
            }
        }
    },

    sendResetPasswordCode: async (identifier: string): Promise<UserActionResponse> => {
        try {
            const queryParams = new URLSearchParams({ identifier }).toString();

            const response = await axios.get(`${API_URL}/auth/send-reset-password-email?${queryParams}`);
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error reenviando el código de verificación.';
            } else {
                throw 'Ha ocurrido un error inesperado al reenviar el código de verificación.';
            }
        }
    },

    verifyCaptcha: async (token: string): Promise<ReCaptchaVerificationResponse> => {
        try {
            const params = new URLSearchParams({ token }).toString();
            const response = await axios.get(`${API_URL}/auth/verify-captcha?${params}`);
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al verificar el captcha.';
            } else {
                throw 'Ha ocurrido un error inesperado al verificar el captcha.';
            }
        }
    },
    getReCaptchaSiteKey: async (): Promise<ReCaptchaSiteKeyResponse> => {
        try {
            const response = await axios.get(`${API_URL}/auth/get-captcha-site-key`);
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al verificar el captcha.';
            } else {
                throw 'Ha ocurrido un error inesperado al verificar el captcha.';
            }
        }
    },
    getGoogleClientId: async (): Promise<GoogleClientIdResponse> => {
        try {
            const response = await axios.get(`${API_URL}/auth/get-google-client-id`);
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al obtener el clientId de Google.';
            } else {
                throw 'Ha ocurrido un error inesperado al obtener el clientId de Google.';
            }
        }
    },
    googleAccess: async (data: GoogleAccessData): Promise<UserActionResponse> => {
        try {
            const response = await axios.post(`${API_URL}/auth/google-access`, data)
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al intentar acceder con Google.';
            } else {
                throw 'Ha ocurrido un error inesperado al intentar acceder con Google.';
            }
        }
    },
    checkGoogleToken: async (token: string): Promise<GoogleTokenValidationResponse> => {
        try {
            const response = await axios.post(`${API_URL}/auth/check-google-token`, { 'token': token })
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al chequear la validez del token con Google.';
            } else {
                throw 'Ha ocurrido un error inesperado al chequear la validez del token con Google.';
            }
        }
    },
    homePageDataRetrieve: async (): Promise<GetHomePageInfoResponse> => {
        try {
            const response = await axios.get(`${API_URL}/get/homepage`)
            console.log("response homepage")
            console.table(response)
            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error) && error.response) {
                throw error.response.data || 'Error al obtener los datos de la página de inicio.';
            } else {
                throw 'Ha ocurrido un error inesperado al obtener los datos de la página de inicio.';
            }
        }
    }
};