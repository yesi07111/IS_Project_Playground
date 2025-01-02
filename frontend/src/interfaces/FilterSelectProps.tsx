import { SelectChangeEvent } from "@mui/material/Select";

export interface FilterSelectProps {
    selectedFilters: string[];
    handleFilterChange: (event: SelectChangeEvent<string[]>) => void;
    handleApplyFilters: () => void;
}