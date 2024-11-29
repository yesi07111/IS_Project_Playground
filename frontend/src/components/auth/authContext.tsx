import React, { createContext, useContext, useState, ReactNode, useEffect } from 'react';
import { authService } from '../../services/authService';

interface AuthContextType {
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

interface AuthProviderProps {
    children: ReactNode;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [canAccessUserType, setCanAccessUserType] = useState(false);
    const [canAccessVerifyEmail, setCanAccessVerifyEmail] = useState(false);
    const [isEmailVerified, setIsEmailVerified] = useState(false);

    const login = () => setIsAuthenticated(true);
    const logout = () => {
        setIsAuthenticated(false);
    };

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

// eslint-disable-next-line react-refresh/only-export-components
export const useAuth = (): AuthContextType => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};