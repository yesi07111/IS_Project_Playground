import React from 'react';
import { FormControl, InputLabel, Select, MenuItem, Button } from '@mui/material';
import { FilterSelectProps } from '../../interfaces/Filters';

export const FilterSelect: React.FC<FilterSelectProps> = ({ selectedFilters, handleFilterChange, handleApplyFilters, menuItems }) => {
    return (
        <div style={{ display: 'flex', alignItems: 'center' }}>
            <FormControl sx={{ width: '300px', mr: 2 }} variant="outlined">
                <InputLabel id="filtros-label">🔽 Añadir filtros</InputLabel>
                <Select
                    labelId="filtros-label"
                    label="🔽 Añadir filtros"
                    multiple
                    value={selectedFilters}
                    onChange={handleFilterChange}
                    renderValue={(selected) =>
                        // Renderizar los textos de los filtros seleccionados
                        (selected as string[])
                            .map((selectedValue) => {
                                const item = menuItems.find((menuItem) => menuItem.value === selectedValue);
                                return item ? item.label : selectedValue;
                            })
                            .join(', ')
                    }
                    sx={{
                        minHeight: '56px',
                        maxHeight: '100vh',
                        '& .MuiSelect-select': {
                            paddingTop: '8px',
                            paddingBottom: '8px',
                            textAlign: 'left',
                            whiteSpace: 'normal',
                            display: 'block',
                            overflow: 'hidden',
                        },
                    }}
                    MenuProps={{
                        PaperProps: {
                            style: {
                                maxHeight: 224,
                                width: 250,
                            },
                        },
                    }}
                >
                    {menuItems.map((item, index) => (
                        <MenuItem key={index} value={item.value}>
                            {item.label}
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>
            {selectedFilters.length > 0 && (
                <Button variant="contained" color="primary" onClick={handleApplyFilters} sx={{ height: '56px' }}>
                    Aplicar filtros
                </Button>
            )}
        </div>
    );
};
