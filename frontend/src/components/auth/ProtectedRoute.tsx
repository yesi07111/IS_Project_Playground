import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from './authContext';
import { ProtectedRouteProps } from '../../interfaces/ProtectedRouteProps';

/**
 * Componente de ruta protegida que controla el acceso a rutas específicas basándose en el estado de autenticación.
 * 
 * Este componente utiliza el contexto de autenticación para determinar si el usuario tiene permiso para acceder
 * a ciertas rutas. Redirige a diferentes rutas según el estado de autenticación y los permisos de acceso.
 * 
 * @param {ProtectedRouteProps} props - Propiedades del componente, incluyendo los componentes hijos y la ruta de redirección.
 * @returns {JSX.Element} Los componentes hijos si el acceso está permitido, o un componente `Navigate` para redirigir si no lo está.
 */
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