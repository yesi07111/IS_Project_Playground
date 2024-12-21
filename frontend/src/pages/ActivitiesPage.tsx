import React, { useState, useEffect } from 'react';
import { Box, Button, TextField, Pagination, MenuItem, Select, FormControl, InputLabel, Collapse, Typography, IconButton } from '@mui/material';
import Grid2 from '@mui/material/Grid2';
import ActivityCard from '../components/features/ActivityCard';
import { Activity } from '../interfaces/Activity';
import { SelectChangeEvent } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import { ActivitiesFilters } from '../interfaces/ActivitiesFilters';
import { activityService } from '../services/activityService';

const ActivitiesPage: React.FC = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const [selectedFilters, setSelectedFilters] = useState<string[]>([]);
    const [filtersOpen, setFiltersOpen] = useState(false);
    const [rating, setRating] = useState<number | null>(null);
    const [startDate, setStartDate] = useState<string>('');
    const [endDate, setEndDate] = useState<string>('');
    const [startTime, setStartTime] = useState<string>('');
    const [endTime, setEndTime] = useState<string>('');
    const [educator, setEducator] = useState<string>('');
    const [type, setType] = useState<string>('');
    const [minAge, setMinAge] = useState<number | undefined>(undefined);
    const [maxAge, setMaxAge] = useState<number | undefined>(undefined);
    const [availability, setAvailability] = useState<string>('');
    const [activities, setActivities] = useState<Activity[]>([]);
    const activitiesPerPage = 6;

    useEffect(() => {
        const fetchActivities = async () => {
            try {
                const activities = await activityService.getAllActivities([]);
                console.table(activities);
                setActivities(Array.isArray(activities) ? activities : []);
            } catch (error) {
                console.error('Error fetching activities:', error);
                setActivities([]); // Asegúrate de que activities sea un array incluso en caso de error
            }
        };

        fetchActivities();
    }, []);

    const filteredActivities = activities.filter(activity =>
        activity.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        activity.description.toLowerCase().includes(searchTerm.toLowerCase())
    );

    const totalPages = Math.ceil(filteredActivities.length / activitiesPerPage);

    const currentActivities = filteredActivities.slice(
        (currentPage - 1) * activitiesPerPage,
        currentPage * activitiesPerPage
    );

    const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(event.target.value);
        setCurrentPage(1);
    };

    const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setCurrentPage(value);
    };

    const handleFilterChange = (event: SelectChangeEvent<string[]>) => {
        const value = event.target.value as string[];
        setSelectedFilters(value);
        setFiltersOpen(value.length > 0);
    };

    const toggleFiltersOpen = () => {
        setFiltersOpen(!filtersOpen);
    };

    const handleApplyFilters = async () => {
        const filters: ActivitiesFilters[] = [];

        if (rating !== null) {
            filters.push({ type: 'Calificación', value: rating.toString() });
        }
        if (startDate || endDate) {
            filters.push({ type: 'Fecha', startDate, endDate });
        }
        if (startTime || endTime) {
            filters.push({ type: 'Hora', startTime, endTime });
        }
        if (educator) {
            filters.push({ type: 'Educador', value: educator });
        }
        if (type) {
            filters.push({ type: 'Tipo', value: type });
        }
        if (minAge !== undefined || maxAge !== undefined) {
            filters.push({ type: 'Edad Recomendada', minAge, maxAge });
        }
        if (availability) {
            filters.push({ type: 'Disponibilidad', value: availability });
        }

        try {
            const activities = await activityService.getAllActivities(filters);
            setActivities(activities);
        } catch (error) {
            console.error('Error applying filters:', error);
        }
    };

    return (
        <Box sx={{ width: '100vw', minHeight: '100vh', py: 4, px: 2, backgroundColor: '#f8f9fa' }}>
            {/* Buscador con filtros */}
            <Box sx={{ display: 'flex', justifyContent: 'center', mb: 4 }}>
                <TextField
                    label="🔍 Buscar Actividades"
                    variant="outlined"
                    value={searchTerm}
                    onChange={handleSearchChange}
                    sx={{ width: '300px', mr: 2 }}
                />
                <FormControl sx={{ width: '300px', height: '56px', mr: 2 }}>
                    <InputLabel
                        sx={{
                            color: selectedFilters.length > 0 ? '#1976d2' : 'inherit',
                            position: 'absolute',
                            left: '10px',
                            top: '50%',
                            transform: 'translateY(-50%)',
                            '&.Mui-focused': {
                                color: '#1976d2',
                            },
                        }}
                    >
                        🔽 Añadir filtros
                    </InputLabel>
                    <Select
                        multiple
                        value={selectedFilters}
                        onChange={handleFilterChange}
                        renderValue={() => null}
                        sx={{
                            height: '56px',
                            '& .MuiSelect-select': {
                                paddingTop: '8px',
                                paddingBottom: '8px',
                                textAlign: 'left',
                            },
                            '&.Mui-focused .MuiSelect-select': {
                                color: '#1976d2',
                            },
                        }}
                    >
                        <MenuItem value="Calificación">Calificación</MenuItem>
                        <MenuItem value="Fecha">Fecha</MenuItem>
                        <MenuItem value="Hora">Hora</MenuItem>
                        <MenuItem value="Educador">Educador</MenuItem>
                        <MenuItem value="Tipo">Tipo</MenuItem>
                        <MenuItem value="Edad Recomendada">Edad Recomendada</MenuItem>
                        <MenuItem value="Disponibilidad">Disponibilidad</MenuItem>
                    </Select>
                </FormControl>
                {selectedFilters.length > 0 && (
                    <Button variant="contained" color="primary" onClick={handleApplyFilters}>
                        Aplicar filtros
                    </Button>
                )}
            </Box>

            {/* Sección plegable de filtros */}
            {selectedFilters.length > 0 && (
                <Box sx={{ mb: 4, p: 2, border: '1px solid #ccc', borderRadius: '4px' }}>
                    <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                        <Typography variant="h6">Filtros 🌟</Typography>
                        <IconButton onClick={toggleFiltersOpen}>
                            {filtersOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                        </IconButton>
                    </Box>
                    <Collapse in={filtersOpen}>
                        {selectedFilters.map((filter) => (
                            <Box key={filter} sx={{ mt: 2 }}>
                                {filter === "Calificación" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>⭐ Calificación ≥</Typography>
                                        <TextField
                                            type="number"
                                            onChange={(e) => {
                                                const value = parseFloat(e.target.value);
                                                setRating(Math.min(Math.max(value, 0.0), 5.0));
                                            }}
                                            slotProps={{
                                                htmlInput: {
                                                    step: 0.1,
                                                },
                                            }}
                                            sx={{
                                                ml: 2,
                                                width: '50px',
                                                textAlign: 'center',
                                                '& input::-webkit-outer-spin-button, & input::-webkit-inner-spin-button': {
                                                    WebkitAppearance: 'none',
                                                    margin: 0,
                                                },
                                                '& input[type=number]': {
                                                    MozAppearance: 'textfield',
                                                },
                                            }}
                                        />
                                    </Box>
                                )}
                                {filter === "Fecha" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>📅 Fecha desde:</Typography>
                                        <TextField
                                            type="date"
                                            onChange={(e) => setStartDate(e.target.value)}
                                            sx={{ ml: 2 }}
                                        />
                                        <Typography sx={{ ml: 2 }}>Hasta:</Typography>
                                        <TextField
                                            type="date"
                                            onChange={(e) => setEndDate(e.target.value)}
                                            sx={{ ml: 2 }}
                                        />
                                    </Box>
                                )}
                                {filter === "Hora" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>⏰ Hora desde:</Typography>
                                        <TextField
                                            type="time"
                                            onChange={(e) => setStartTime(e.target.value)}
                                            sx={{ ml: 2 }}
                                        />
                                        <Typography sx={{ ml: 2 }}>Hasta:</Typography>
                                        <TextField
                                            type="time"
                                            onChange={(e) => setEndTime(e.target.value)}
                                            sx={{ ml: 2 }}
                                        />
                                    </Box>
                                )}
                                {filter === "Educador" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>👨‍🏫 Educador:</Typography>
                                        <Select
                                            defaultValue=""
                                            onChange={(e) => setEducator(e.target.value)}
                                            sx={{ ml: 2 }}
                                        >
                                            <MenuItem value="Educador 1">Educador 1</MenuItem>
                                            <MenuItem value="Educador 2">Educador 2</MenuItem>
                                        </Select>
                                    </Box>
                                )}
                                {filter === "Tipo" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>🎢 Tipo:</Typography>
                                        <Select
                                            defaultValue=""
                                            onChange={(e) => setType(e.target.value)}
                                            sx={{ ml: 2 }}
                                        >
                                            <MenuItem value="Aire Libre">Aire Libre</MenuItem>
                                            <MenuItem value="Carrusel">Carrusel</MenuItem>
                                            <MenuItem value="Piscina">Piscina</MenuItem>
                                            <MenuItem value="Tobogán">Tobogán</MenuItem>
                                            <MenuItem value="Trencito">Trencito</MenuItem>
                                            <MenuItem value="Otro Aparato">Otro Aparato</MenuItem>
                                        </Select>
                                    </Box>
                                )}
                                {filter === "Edad Recomendada" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>👶 Edad recomendada: Mayor o igual que</Typography>
                                        <TextField
                                            type="number"
                                            onChange={(e) => setMinAge(parseInt(e.target.value, 10))}
                                            sx={{
                                                ml: 2,
                                                width: '50px',
                                                textAlign: 'center',
                                                '& input::-webkit-outer-spin-button, & input::-webkit-inner-spin-button': {
                                                    WebkitAppearance: 'none',
                                                    margin: 0,
                                                },
                                                '& input[type=number]': {
                                                    MozAppearance: 'textfield',
                                                },
                                            }}
                                        />
                                        <Typography sx={{ ml: 2 }}>y/o menor o igual que:</Typography>
                                        <TextField
                                            type="number"
                                            onChange={(e) => setMaxAge(parseInt(e.target.value, 10))}
                                            sx={{
                                                ml: 2,
                                                width: '50px',
                                                textAlign: 'center',
                                                '& input::-webkit-outer-spin-button, & input::-webkit-inner-spin-button': {
                                                    WebkitAppearance: 'none',
                                                    margin: 0,
                                                },
                                                '& input[type=number]': {
                                                    MozAppearance: 'textfield',
                                                },
                                            }}
                                        />
                                    </Box>
                                )}
                                {filter === "Disponibilidad" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>🔓 Disponibilidad:</Typography>
                                        <Select
                                            defaultValue=""
                                            onChange={(e) => setAvailability(e.target.value)}
                                            sx={{ ml: 2 }}
                                        >
                                            <MenuItem value="publico">Público</MenuItem>
                                            <MenuItem value="privado">Privado</MenuItem>
                                        </Select>
                                    </Box>
                                )}
                            </Box>
                        ))}
                    </Collapse>
                </Box>
            )}

            {/* Grid de actividades */}
            <Grid2 container spacing={4}>
                {currentActivities.map(activity => (
                    <Grid2 size={{ xs: 12, sm: 6, md: 4 }} key={activity.id}
                        sx={{
                            display: 'flex',
                            justifyContent: 'center'
                        }}
                    >
                        <ActivityCard activity={activity} />
                    </Grid2>
                ))}
            </Grid2>

            {/* Paginación */}
            <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
                <Pagination
                    count={totalPages}
                    page={currentPage}
                    onChange={handlePageChange}
                    color="primary"
                />
            </Box>
        </Box>
    );
}

export default ActivitiesPage;