import React, { createContext, useContext, useState, ReactNode } from 'react';

interface AuthContextType {
    isAuthenticated: boolean;
    login: () => void;
    logout: () => void;
    canAccessUserType: boolean;
    setCanAccessUserType: (status: boolean) => void;
    canAccessVerifyEmail: boolean;
    setCanAccessVerifyEmail: (status: boolean) => void;
}

interface AuthProviderProps {
    children: ReactNode;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [canAccessUserType, setCanAccessUserType] = useState(false);
    const [canAccessVerifyEmail, setCanAccessVerifyEmail] = useState(false);

    const login = () => setIsAuthenticated(true);
    const logout = () => setIsAuthenticated(false);

    return (
        <AuthContext.Provider value={{
            isAuthenticated,
            login,
            logout,
            canAccessUserType,
            setCanAccessUserType,
            canAccessVerifyEmail,
            setCanAccessVerifyEmail
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