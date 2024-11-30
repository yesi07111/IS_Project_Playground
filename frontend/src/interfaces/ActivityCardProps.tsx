import { Activity } from "./Activity";

/**
 * Interfaz para las propiedades del componente ActivityCard.
 * 
 * Esta interfaz define las propiedades que debe recibir el componente
 * ActivityCard, incluyendo un objeto de tipo Activity que contiene
 * los detalles de la actividad a mostrar.
 */
export interface ActivityCardProps {
    activity: Activity;
}