export interface FacilityFilters {
    name?: string; // Nombre de la instalación
    location?: string; // Ubicación de la instalación
    type?: string; // Tipo de instalación
    maximumCapacity?: number; // Capacidad máxima de la instalación
    usagePolicy?: string; // Política de uso de la instalación
    useCase: string;
}
