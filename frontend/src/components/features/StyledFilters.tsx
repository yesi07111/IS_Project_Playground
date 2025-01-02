import React from 'react';
import { FormControl, InputLabel, Select, MenuItem, Button } from '@mui/material';
import { FilterSelectProps } from '../../interfaces/FilterSelectProps';
import { useLocation } from 'react-router-dom';

export const FilterSelect: React.FC<FilterSelectProps> = ({ selectedFilters, handleFilterChange, handleApplyFilters }) => {
    const location = useLocation();

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
                    {location.pathname === '/activities' ? (
                        <MenuItem value="De Esta Semana">De Esta Semana</MenuItem>
                    ) : (
                        <MenuItem value="Calificación">Calificación</MenuItem>
                    )}
                    <MenuItem value="Rango de Fecha">Rango de Fecha</MenuItem>
                    <MenuItem value="Rango de Hora">Rango de Hora</MenuItem>
                    <MenuItem value="Educadores">Por Educadores</MenuItem>
                    <MenuItem value="Tipos de Instalaciones">Tipos de Instalaciones</MenuItem>
                    <MenuItem value="Tipos de Actividades">Tipos de Actividades</MenuItem>
                    <MenuItem value="Edad Recomendada">Edad Recomendada</MenuItem>
                    <MenuItem value="Disponibilidad">Pública o Privada</MenuItem>
                    {location.pathname === '/activities' && (
                        <MenuItem value="Nueva">Nueva o No</MenuItem>
                    )}
                    <MenuItem value="Capacidad Disponible">Capacidad Disponible</MenuItem>
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
