import { ReactNode } from "react";

/**
 * Interfaz para las propiedades de la ruta protegida.
 * 
 * Esta interfaz define las propiedades que debe recibir el componente
 * ProtectedRoute, incluyendo los componentes hijos que se mostrarán
 * si el acceso está permitido y la ruta de redirección en caso contrario.
 */
export interface ProtectedRouteProps {
    children: ReactNode;
    redirectTo: string;
}
/**
 * Interfaz para el contexto de autenticación.
 * 
 * Esta interfaz define el tipo de datos y funciones que se proporcionan
 * a través del contexto de autenticación, incluyendo el estado de autenticación,
 * funciones para iniciar y cerrar sesión, y verificación de correo electrónico.
 */
export interface AuthContextType {
    isAuthenticated: boolean;
    login: () => void;
    logout: () => void;
    canAccessPasswordReset: boolean;
    setCanAccessPasswordReset: (status: boolean) => void;
    canAccessVerifyEmail: boolean;
    setCanAccessVerifyEmail: (status: boolean) => void;
    isEmailVerified: boolean;
    checkEmailVerification: () => Promise<void>;
}

/**
 * Interfaz para las propiedades del proveedor de autenticación.
 * 
 * Esta interfaz define las propiedades que debe recibir el componente
 * AuthProvider, incluyendo los componentes hijos que se envolverán
 * dentro del proveedor de contexto de autenticación.
 */
export interface AuthProviderProps {
    children: ReactNode;
}

export interface ReCaptchaSiteKeyResponse {

    isValid: boolean;
    message: string;
    siteKey: string;
}

export interface GoogleClientIdResponse {
    clientId: string;
}

export interface GoogleTokenValidationResponse {
    isValid: boolean;
    claims?: { [key: string]: string } | null;
}

export interface DecodedtokenResponse {
    claims: {
        exp: string; // Fecha de expiración del token en formato UNIX timestamp
        iat?: string; // Fecha de emisión del token en formato UNIX timestamp
        iss?: string; // Emisor del token
        sub?: string; // identificador del sujeto (usuario)
        aud?: string; // Audiencia del token
        nbf?: string; // Fecha antes de la cual el token no es válido
        jti?: string; // identificador único del token
        email?: string; // Correo electrónico del usuario
        email_verified?: boolean; // Indica si el correo electrónico ha sido verificado
        name?: string; // Nombre completo del usuario
        given_name?: string; // Nombre de pila del usuario
        family_name?: string; // Apellido del usuario
        picture?: string; // URL de la imagen de perfil del usuario
        locale?: string; // Configuración regional del usuario
        // Otros campos personalizados que pueda tener el token
    };
    // Otros campos que pueda tener el token
    access_token?: string; // token de acceso
    refresh_token?: string; // token de actualización
    token_type?: string; // Tipo de token (por ejemplo, "Bearer")
    scope?: string; // Alcance del token
    expires_in?: string; // Tiempo en segundos hasta que el token expire
}

/**
 * Interfaz que define la estructura de los datos necesarios para registrar un nuevo usuario.
 * 
 * Esta interfaz se utiliza para tipar los datos que se envían al endpoint de registro de usuario.
 * Incluye los siguientes campos:
 * 
 * - **firstName**: Nombre del usuario.
 * - **lastName**: Apellido del usuario.
 * - **username**: Nombre de usuario único.
 * - **Password**: Contraseña del usuario.
 * - **email**: Correo electrónico del usuario.
 * - **rol**: Rol asignado al usuario.
 */
export interface RegisterData {
    firstName: string;
    lastName: string;
    username: string;
    password: string;
    email: string;
    rol: string;
}

export interface GoogleAccessData {
    firstName: string;
    lastName: string;
    username: string;
    imageUrl: string;
    email: string;
    isConfirmed: string;
    rol: string;
    action: string;
}

export interface UserCreationResponse {
    id: string;
    username: string;
}

export interface UserActionResponse {
    id: string;
    username: string;
    token: string;
}

export interface CheckEmailResponse {
    isVerified: boolean;
}

export interface ResetPasswordResponse {
    success: boolean;
    message: string;
}

export interface SendResetPasswordCodeResponse {
    success: boolean;
    message: string;
}

export interface ReCaptchaVerificationResponse {
    isValid: boolean;
    message: string;
}

export interface DeleteFailUserResponse {
    success: boolean;
    message: string;
}
