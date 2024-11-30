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