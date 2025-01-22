import React, { createContext, useContext, useState, useEffect } from 'react';
import { authService } from '../../services/authService';
import { AuthContextType, AuthProviderProps } from '../../interfaces/Auth';
import { tokenService } from '../../services/tokenService';

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [canAccessPasswordReset, setCanAccessPasswordReset] = useState(false);
    const [canAccessVerifyEmail, setCanAccessVerifyEmail] = useState(false);
    const [isEmailVerified, setIsEmailVerified] = useState(false);

    // Restaurar el estado de autenticación desde localStorage al cargar
    useEffect(() => {
        const storedAuth = localStorage.getItem('isAuthenticated');
        if (storedAuth === 'true') {
            setIsAuthenticated(true);
            tokenService.startTokenRefreshCheck();
        }
    }, []);

    const login = () => {
        setIsAuthenticated(true);
        tokenService.startTokenRefreshCheck();
        localStorage.setItem('isAuthenticated', 'true'); // Guardar en localStorage
    };

    const logout = () => {
        setIsAuthenticated(false);
        localStorage.removeItem('isAuthenticated'); // Eliminar de localStorage
        localStorage.removeItem('authToken'); // Limpiar otros datos de sesión si es necesario
        localStorage.removeItem('authId');
        localStorage.removeItem('authUsername');
        tokenService.stopTokenRefreshCheck();
    };

    const checkEmailVerification = async () => {
        try {
            const token = localStorage.getItem('authToken');
            const id = localStorage.getItem('authId');
            const username = localStorage.getItem('authUsername');

            if (token && id && username) {
                const response = await authService.checkVerifiedEmail(token, id, username);
                setIsEmailVerified(response.isVerified);
                setCanAccessVerifyEmail(!response.isVerified);
                console.log('Email verificado:', response.isVerified);
            }
        } catch (error) {
            console.error('Error verificando el email:', error);
        }
    };

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
            canAccessPasswordReset,
            setCanAccessPasswordReset,
            canAccessVerifyEmail,
            setCanAccessVerifyEmail,
            isEmailVerified,
            checkEmailVerification
        }}>
            {children}
        </AuthContext.Provider>
    );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth debe ser usado dentro de AuthProvider');
    }
    return context;
};