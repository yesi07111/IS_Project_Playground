// Define una interfaz para los filtros
export interface ActivitiesFilters {
    type: string;
    value?: string;
    startDate?: string;
    endDate?: string;
    startTime?: string;
    endTime?: string;
    minAge?: number;
    maxAge?: number;
}