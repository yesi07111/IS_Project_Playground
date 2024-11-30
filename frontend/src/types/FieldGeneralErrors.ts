/**
 * Tipo que representa los errores de validación recibidos desde el backend.
 * 
 * Este tipo se utiliza para manejar los errores de validación que provienen
 * de un validador de comandos o de consultas en el backend. Incluye un código
 * de estado, un mensaje descriptivo y un objeto de errores que contiene listas
 * de errores específicos por campo, así como errores generales.
 * 
 * - **statusCode**: Código de estado HTTP asociado al error.
 * - **message**: Mensaje descriptivo del error.
 * - **errors**: Objeto que contiene:
 *   - Errores específicos por campo, donde la clave es el nombre del campo y el valor es una lista de mensajes de error.
 *   - **generalErrors**: Lista de errores generales que no están asociados a un campo específico.
 */
export type FieldGeneralErrors = {
    statusCode: number;
    message: string;
    errors: {
        [key: string]: string[];
        generalErrors: string[];
    };
};