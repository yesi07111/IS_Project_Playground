import { ReactNode } from "react";

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