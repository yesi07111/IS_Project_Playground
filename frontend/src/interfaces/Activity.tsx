/**
 * Interfaz que define la estructura de una actividad.
 * 
 * @property {number} id - Identificador único de la actividad.
 * @property {string} name - Nombre de la actividad.
 * @property {string} description - Descripción de la actividad.
 * @property {string} image - URL de la imagen representativa de la actividad.
 * @property {number} rating - Calificación de la actividad.
 * @property {string} color - Color asociado a la actividad para temas de diseño.
 */
export interface Activity {
    id: number;
    name: string;
    description: string;
    image: string;
    rating: number;
    color: string;
}