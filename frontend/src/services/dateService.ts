export function parseDate(date: unknown): { formattedDate: string, formattedTime: string } {
    // Verificar si date es una instancia de Date
    if (!(date instanceof Date)) {
        // Intentar convertir el string a Date si es un string
        if (typeof date === 'string') {
            const parsedDate = new Date(date);
            if (!isNaN(parsedDate.getTime())) {
                date = parsedDate;
            } else {
                // Si no se puede convertir, retornar valores vacíos
                return { formattedDate: '', formattedTime: '' };
            }
        } else {
            // Si no es ni Date ni string, retornar valores vacíos
            return { formattedDate: '', formattedTime: '' };
        }
    }

    if (date instanceof Date) {
        // Convertir la fecha de UTC a la hora local
        const localDate = new Date(date.getTime() + date.getTimezoneOffset() * 60000);

        const daysOfWeek = ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'];
        const months = ['enero', 'febrero', 'marzo', 'abril', 'mayo', 'junio', 'julio', 'agosto', 'septiembre', 'octubre', 'noviembre', 'diciembre'];

        const dayOfWeek = daysOfWeek[localDate.getDay()];
        const day = localDate.getDate();
        const month = months[localDate.getMonth()];

        const hours = localDate.getHours();
        const minutes = String(localDate.getMinutes()).padStart(2, '0');
        const ampm = hours >= 12 ? 'PM' : 'AM';
        const formattedHours = hours % 12 || 12; // Convertir a formato de 12 horas

        const formattedDate = `${dayOfWeek} ${day} de ${month}`;
        const formattedTime = `${formattedHours}:${minutes} ${ampm}`;

        return { formattedDate, formattedTime };
    }

    return { formattedDate: '', formattedTime: '' };
}