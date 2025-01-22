import { SelectChangeEvent } from "@mui/material/Select";

export interface FilterSelectProps {
    selectedFilters: string[];
    handleFilterChange: (event: SelectChangeEvent<string[]>) => void;
    handleApplyFilters: () => void;
}

export interface ActivitiesFilters {
    type: string;
    useCase?: string;
    value?: string;
    startDate?: string;
    endDate?: string;
    startTime?: string;
    endTime?: string;
    minAge?: number;
    maxAge?: number;
}

export interface FacilityFilters {
    name?: string; // Nombre de la instalación
    location?: string; // Ubicación de la instalación
    type?: string; // Tipo de instalación
    maximumCapacity?: number; // Capacidad máxima de la instalación
    usagePolicy?: string; // Política de uso de la instalación
    useCase: string;
}

export interface UserFilters {
    userName?: string;
    email?: string;
    emailConfirmed?: string;
    firstName?: string;
    lastName?: string;
    rol?: string;
    markDeleted?: boolean;
    useCase: string;
}

export interface DaySelectorProps {
    daysOfWeek: string[];
    setDaysOfWeek: React.Dispatch<React.SetStateAction<string[]>>;
}

export interface SearchBarProps {
    searchTerm: string;
    handleSearchChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    labelText: string;
}

