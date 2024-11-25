import React, { ReactNode } from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from './authContext';

interface ProtectedRouteProps {
    children: ReactNode;
    redirectTo: string;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, redirectTo }) => {
    const { isAuthenticated, canAccessUserType, canAccessVerifyEmail } = useAuth();

    if (redirectTo === '/user-type' && !canAccessUserType) {
        return <Navigate to="/login" />;
    }

    if (redirectTo === '/verify-email' && !canAccessVerifyEmail) {
        return <Navigate to="/register" />;
    }

    if (!isAuthenticated && !canAccessUserType && !canAccessVerifyEmail) {
        return <Navigate to={redirectTo} />;
    }

    return <>{children}</>;
};

export default ProtectedRoute;