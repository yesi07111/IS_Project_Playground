import React from 'react';
import { TextField } from '@mui/material';
import { SearchBarProps } from '../../interfaces/Filters';

export const SearchBar: React.FC<SearchBarProps> = ({ searchTerm, handleSearchChange, labelText }) => {
    return (
        <TextField
            label={`🔍 Buscar ${labelText}`}
            variant="outlined"
            value={searchTerm}
            onChange={handleSearchChange}
            sx={{ width: '300px', mr: 2 }}
        />
    );
};