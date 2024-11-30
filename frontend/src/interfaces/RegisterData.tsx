/**
 * Interfaz que define la estructura de los datos necesarios para registrar un nuevo usuario.
 * 
 * Esta interfaz se utiliza para tipar los datos que se envían al endpoint de registro de usuario.
 * Incluye los siguientes campos:
 * 
 * - **FirstName**: Nombre del usuario.
 * - **LastName**: Apellido del usuario.
 * - **Username**: Nombre de usuario único.
 * - **Password**: Contraseña del usuario.
 * - **Email**: Correo electrónico del usuario.
 * - **Roles**: Lista de roles asignados al usuario.
 */
export interface RegisterData {
    FirstName: string;
    LastName: string;
    Username: string;
    Password: string;
    Email: string;
    Roles: string[];
}
