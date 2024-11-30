import React, { createContext, useContext, useState, useEffect } from 'react';
import { authService } from '../../services/authService';
import { AuthContextType } from '../../interfaces/AuthContextType';
import { AuthProviderProps } from '../../interfaces/AuthProviderProps';

/**
 * Contexto de autenticación que proporciona el estado de autenticación y funciones relacionadas.
 * 
 * Este contexto se utiliza para compartir el estado de autenticación y las funciones de inicio
 * y cierre de sesión en toda la aplicación. Se inicializa como indefinido y se debe usar dentro
 * de un `AuthProvider`.
 */
const AuthContext = createContext<AuthContextType | undefined>(undefined);

/**
 * Proveedor de contexto de autenticación.
 * 
 * Este componente envuelve a los componentes hijos y proporciona el contexto de autenticación,
 * que incluye el estado de autenticación, funciones para iniciar y cerrar sesión, y verificación
 * de correo electrónico.
 * 
 * @param {AuthProviderProps} props - Propiedades del proveedor, incluyendo los componentes hijos.
 * @returns {JSX.Element} El proveedor de contexto de autenticación.
 */
export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [canAccessUserType, setCanAccessUserType] = useState(false);
    const [canAccessVerifyEmail, setCanAccessVerifyEmail] = useState(false);
    const [isEmailVerified, setIsEmailVerified] = useState(false);

    /**
     * Función para iniciar sesión.
     * 
     * Establece el estado de autenticación como verdadero.
     */
    const login = () => setIsAuthenticated(true);

    /**
     * Función para cerrar sesión.
     * 
     * Establece el estado de autenticación como falso.
     */
    const logout = () => {
        setIsAuthenticated(false);
    };

    /**
     * Verifica el estado de verificación del correo electrónico del usuario.
     * 
     * Esta función obtiene el token de autenticación, el ID y el nombre de usuario
     * del almacenamiento local y utiliza el servicio de autenticación para verificar
     * si el correo electrónico está verificado. Actualiza el estado de verificación
     * del correo electrónico en consecuencia.
     */
    const checkEmailVerification = async () => {
        try {
            const token = localStorage.getItem('authToken');
            const id = localStorage.getItem('authId');
            const username = localStorage.getItem('authUserName');

            if (token && id && username) {
                const response = await authService.checkVerifiedEmail(token, id, username);
                setIsEmailVerified(response.IsVerified);
                setCanAccessVerifyEmail(!response.IsVerified);
            }
        } catch (error) {
            console.error('Error checking email verification:', error);
        }
    };

    /**
     * Efecto que verifica el estado de verificación del correo electrónico cuando el usuario está autenticado.
     * 
     * Este efecto se ejecuta cada vez que cambia el estado de autenticación. Si el usuario está autenticado,
     * llama a la función `checkEmailVerification`.
     */
    useEffect(() => {
        if (isAuthenticated) {
            checkEmailVerification();
        }
    }, [isAuthenticated]);

    return (
        <AuthContext.Provider value={{
            isAuthenticated,
            login,
            logout,
            canAccessUserType,
            setCanAccessUserType,
            canAccessVerifyEmail,
            setCanAccessVerifyEmail,
            isEmailVerified,
            checkEmailVerification
        }}>
            {children}
        </AuthContext.Provider>
    );
};

/**
 * Hook personalizado para acceder al contexto de autenticación.
 * 
 * Este hook proporciona acceso al contexto de autenticación y debe ser utilizado
 * dentro de un `AuthProvider`. Lanza un error si se usa fuera de un `AuthProvider`.
 * 
 * @returns {AuthContextType} El contexto de autenticación.
 * @throws {Error} Si se usa fuera de un `AuthProvider`.
 */
// eslint-disable-next-line react-refresh/only-export-components
export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};