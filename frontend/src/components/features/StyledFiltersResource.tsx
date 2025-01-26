import React from 'react';
import { FormControl, InputLabel, Select, MenuItem, Button } from '@mui/material';
import { FilterSelectProps } from '../../interfaces/Filters';
import { useLocation } from 'react-router-dom';

export const FilterSelect: React.FC<FilterSelectProps> = ({ selectedFilters, handleFilterChange, handleApplyFilters }) => {
    const location = useLocation();

    return (
        <div style={{ display: 'flex', alignItems: 'center' }}>
            {location.pathname === '/resources' && (
                <FormControl sx={{ width: '300px', mr: 2 }} variant="outlined">
                    <InputLabel id="filtros-label">🔽 Añadir filtros</InputLabel>
                    <Select
                        labelId="filtros-label"
                        label="🔽 Añadir filtros"
                        multiple
                        value={selectedFilters}
                        onChange={handleFilterChange}
                        renderValue={(selected) => selected.join(', ')}
                        sx={{
                            minHeight: '56px', // Altura mínima
                            maxHeight: '100vh', // Altura máxima
                            '& .MuiSelect-select': {
                                paddingTop: '8px',
                                paddingBottom: '8px',
                                textAlign: 'left',
                                whiteSpace: 'normal', // Permite el ajuste de línea
                                display: 'block', // Cambia a block para permitir el ajuste de línea
                                overflow: 'hidden', // Oculta el desbordamiento
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
                        <MenuItem value="Tipo de Instalación">Tipo de Instalación</MenuItem>
                        <MenuItem value="Tipo de Recurso">Tipo de Recurso</MenuItem>
                        <MenuItem value="Nombre">Nombre</MenuItem>
                        <MenuItem value="Estado del Recurso">Estado del Recurso</MenuItem>
                        <MenuItem value="Frecuencia de Uso">Frecuencia de Uso</MenuItem>
                    </Select>
                </FormControl>
            )}
            {selectedFilters.length > 0 && (
                <Button variant="contained" color="primary" onClick={handleApplyFilters} sx={{ height: '56px' }}>
                    Aplicar filtros
                </Button>
            )}
        </div>
    );
};
