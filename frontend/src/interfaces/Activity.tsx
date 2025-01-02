/**
 * Interfaz que define la estructura de una actividad.
 * 
 * @property {string} id - Identificador único de la actividad.
 * @property {string} name - Nombre de la actividad.
 * @property {Date} date - Fecha de la actividad.
 * @property {string} image - URL de la imagen representativa de la actividad.
 * @property {number} rating - Calificación de la actividad.
 * @property {string} color - Color asociado a la actividad para temas de diseño.
 * @property {number} maximumCapacity - Capacidad máxima de la actividad.
 * @property {number} currentCapacity - Capacidad ocupada de la actividad.
 * @property {string} location - Ubicación asociado a la actividad.
 *
 * 
 */
export interface Activity {
    id: string;
    name: string;
    date: Date;
    image: string;
    rating: number;
    color: string;
    maximumCapacity: number;
    currentCapacity: number;
    isPublic: string;
    isNew: string;
}
/**
 * Interfaz que define la estructura de la respuesta de la lista de actividades.
 * 
 * @property {Activity[]} activities - Lista de actividades.
 */
export interface ListActivityResponse {
    result: object[];
}

/**
 * Interfaz que define la estructura de una actividad detallada.
 * 
 * @property {string} id - Identificador único de la actividad.
 * @property {string} name - Nombre de la actividad.
 * @property {string} description - Descripción de la actividad.
 * @property {string} image - URL de la imagen representativa de la actividad.
 * @property {number} rating - Calificación de la actividad.
 * @property {string} color - Color asociado a la actividad para temas de diseño.
 * @property {number} maximumCapacity - Capacidad máxima de la actividad.
 * @property {number} currentCapacity - Capacidad ocupada de la actividad.
 * @property {string} educatorFullName - Nombre completo del educador.
 * @property {string} educatorUsername - Nombre de usuario del educador.
 * @property {string} facilityName - Nombre de la instalación.
 * @property {string} facilityLocation - Ubicación de la instalación.
 * @property {string} facilityType - Tipo de la instalación.
 * @property {string} activityType - Tipo de la actividad.
 * @property {string} usagePolicy - Política de uso de la actividad.
 * @property {string} recommendedAge - Edad recomendada para la actividad.
 * @property {string[]} comments - Comentarios sobre la actividad.
 * @property {string[]} resources - Recursos disponibles para la actividad.
 * @property {Date} date - Fecha de la actividad.
 */
export interface ActivityDetail {
    id: string;
    name: string;
    description: string;
    image: string;
    rating: number;
    color: string;
    maximumCapacity: number;
    currentCapacity: number;
    educatorFullName: string;
    educatorUsername: string;
    facilityName: string;
    facilityLocation: string;
    facilityType: string;
    activityType: string;
    usagePolicy: string;
    recommendedAge: string;
    comments: string[];
    resources: string[];
    date: Date;
}
export interface ActivityDetailArray {
    result: ActivityDetail
}
