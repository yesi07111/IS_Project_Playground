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
    canAccessUserType: boolean;
    setCanAccessUserType: (status: boolean) => void;
    canAccessVerifyEmail: boolean;
    setCanAccessVerifyEmail: (status: boolean) => void;
    isEmailVerified: boolean;
    checkEmailVerification: () => Promise<void>;
}