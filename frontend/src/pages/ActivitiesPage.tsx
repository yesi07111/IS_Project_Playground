import React, { useState, useEffect, useCallback } from 'react';
import { Box, TextField, Pagination, MenuItem, Select, Collapse, Typography, IconButton, FormControlLabel, Checkbox, FormControl, InputLabel, Input } from '@mui/material';
import Grid2 from '@mui/material/Grid2';
import ActivityCard from '../components/features/ActivityCard';
import { Activity, ListActivityResponse } from '../interfaces/Activity';
import { SelectChangeEvent } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import { ActivitiesFilters } from '../interfaces/Filters';
import { activityService } from '../services/activityService';
import { userService } from '../services/userService';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { facilityService } from '../services/facilityService';
import { DaySelector } from '../components/features/DaySelector';
import { FilterSelect } from '../components/features/StyledFilters';
import { SearchBar } from '../components/features/StyledSearchBar';
import { cacheService } from '../services/cacheService';
import CreateActivityButton from '../components/features/CreateActivityButton';
import { useAuth } from '../components/auth/authContext';
import { DataPagesProps } from '../interfaces/Pages';
import { dateService } from '../services/dateService';

const ActivitiesPage: React.FC<DataPagesProps> = ({ reload }) => {
    const [searchTerm, setSearchTerm] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const [selectedFilters, setSelectedFilters] = useState<string[]>([]);
    const [filtersOpen, setFiltersOpen] = useState(false);
    const [startDate, setStartDate] = useState<Date | null>(null);
    const [endDate, setEndDate] = useState<Date | null>(null);
    const [startTime, setStartTime] = useState<string>('');
    const [endTime, setEndTime] = useState<string>('');
    const [selectedEducators, setSelectedEducators] = useState<string[]>([]);
    const [selectedFacilityTypes, setSelectedFacilityTypes] = useState<string[]>([]);
    const [selectedActivityTypes, setSelectedActivityTypes] = useState<string[]>([]);
    const [minAge, setMinAge] = useState<number | null>(null);
    const [maxAge, setMaxAge] = useState<number | null>(null);
    const [availability, setAvailability] = useState<string>('');
    const [activities, setActivities] = useState<Activity[]>([]);
    const [educators, setEducators] = useState<{ id: string, displayName: string }[]>([]);
    const [minAgeError, setMinAgeError] = useState<string>('');
    const [maxAgeError, setMaxAgeError] = useState<string>('');
    const [facilityTypes, setFacilityTypes] = useState<string[]>([]);
    const [activityTypes, setActivityTypes] = useState<string[]>([]);
    const [activityImages, setActivityImages] = useState<{ [id: string]: string }>({});
    const [today, setToday] = useState(false);
    const [tomorrow, setTomorrow] = useState(false);
    const [thisWeek, setThisWeek] = useState(false);
    const [daysOfWeek, setDaysOfWeek] = useState<string[]>([]);
    const [capacity, setCapacity] = useState<number | null>(null);
    const [isNew, setIsNew] = React.useState(false);
    const { isAuthenticated } = useAuth();

    const rol = localStorage.getItem('authUserRole');

    const activitiesPerPage = 6;

    const fetchAllFacilityTypes = useCallback(async () => {
        try {
            const response = await facilityService.getAllFacilities({ useCase: 'AllTypes' });
            const typesArray: string[] = Array.isArray(response.result) ? response.result as unknown as string[] : Array.from(response.result);
            setFacilityTypes(typesArray);
            cacheService.saveFacilityTypes(typesArray);
        } catch (error) {
            console.error('Error fetching facility types:', error);
            const cachedFacilityTypes = cacheService.loadFacilityTypes();
            setFacilityTypes(cachedFacilityTypes);
        }
    }, []);

    const fetchAllActivityTypes = useCallback(async () => {
        try {
            const response = await activityService.getAllActivities([{ type: 'Casos de Uso', useCase: 'AllTypes' }]);
            const typesArray: string[] = Array.isArray(response.result) ? response.result as unknown as string[] : [];
            setActivityTypes(typesArray);
            cacheService.saveActivityTypes(typesArray);
        } catch (error) {
            console.error('Error obteniendo los tipos de actividades:', error);
            const cachedActivityTypes = cacheService.loadActivityTypes();
            setActivityTypes(cachedActivityTypes);
        }
    }, []);

    const fetchAllEducators = useCallback(async () => {
        try {
            const users = await userService.getAllUsers({ useCase: 'AsFilter', rol: 'Educator' });
            const usersArray = Array.isArray(users) ? users : Array.from(users.users);
            const formattedEducators = usersArray.map(user => ({
                id: user.id,
                displayName: `${user.firstName} ${user.lastName} @${user.userName}`
            }));
            setEducators(formattedEducators);
            cacheService.saveEducators(formattedEducators);
        } catch (error) {
            console.error('Error obteniendo los educadores:', error);
            const cachedEducators = cacheService.loadEducators();
            setEducators(cachedEducators);
        }
    }, []);

    const cacheActivityImages = useCallback((activitiesArray: Activity[]) => {
        setActivityImages((prevImagesMap) => {
            const newImagesMap = { ...prevImagesMap };
            activitiesArray.forEach((activity: Activity) => {
                if (!newImagesMap[activity.id]) {
                    newImagesMap[activity.id] = activity.image;
                }
            });
            return newImagesMap;
        });
    }, []);

    const fetchAllActivities = useCallback(async (filters: ActivitiesFilters[] = [{ type: 'Casos de Uso', useCase: 'ActivityView' }]) => {
        try {
            const response: ListActivityResponse = await activityService.getAllActivities(filters);
            const activitiesArrayR: Activity[] = Array.isArray(response.result)
                ? response.result as Activity[]
                : [];
            //asegurarme de que las actividades mostradas del filtrado no sean pendientes
            const activitiesArray: Activity[] = activitiesArrayR.filter(activity => !activity.pending)

            cacheActivityImages(activitiesArray);
            setActivities(activitiesArray.map(activity => ({
                ...activity,
                date: new Date(activity.date),
                image: activity.image,
            })));
            setActivityImages(prevImagesMap => {
                const newImagesMap = { ...prevImagesMap };
                activitiesArray.forEach(activity => {
                    if (!newImagesMap[activity.id]) {
                        newImagesMap[activity.id] = activity.image;
                    }
                });
                cacheService.saveImages(newImagesMap);
                return newImagesMap;
            });
            cacheService.saveActivities(activitiesArray);
        } catch (error) {
            console.error('Error obteniendo actividades:', error);
            const cachedActivities = cacheService.loadActivities();
            const cachedImages = cacheService.loadImages();
            setActivityImages(cachedImages);
            setActivities(cachedActivities.map((activity: Activity) => ({
                ...activity,
                date: new Date(activity.date),
                image: cachedImages[activity.id] || activity.image,
            })));
        }
    }, [cacheActivityImages, activityImages]);

    const fetchInitialData = useCallback(async () => {
        await Promise.all([fetchAllFacilityTypes(), fetchAllActivityTypes(), fetchAllEducators(), fetchAllActivities()]);
    }, [fetchAllFacilityTypes, fetchAllActivityTypes, fetchAllEducators, fetchAllActivities]);

    useEffect(() => {
        fetchInitialData();
    }, []);

    useEffect(() => {
        if (selectedFilters.length === 0) {
            fetchAllActivities();
        }
    }, [selectedFilters]);

    useEffect(() => {
        if (reload) {
            cacheService.clearCache()
            fetchInitialData();
        }
    }, [reload]);

    useEffect(() => {
        const cachedActivities = cacheService.loadActivities();
        const cachedImages = cacheService.loadImages();
        const cachedFacilityTypes = cacheService.loadFacilityTypes();
        const cachedActivityTypes = cacheService.loadActivityTypes();
        const cachedEducators = cacheService.loadEducators();
        if (cachedActivities.length > 0) {
            setActivities(cachedActivities);
            setActivityImages(cachedImages);
        }
        setFacilityTypes(cachedFacilityTypes);
        setActivityTypes(cachedActivityTypes);
        setEducators(cachedEducators);
    }, []);

    const filteredActivities = activities.filter(activity => {
        const { formattedDate, formattedTime } = dateService.parseDate(activity.date);
        const participants = `${activity.currentCapacity}/${activity.maximumCapacity}`;

        return activity.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
            formattedDate.toLowerCase().includes(searchTerm.toLowerCase()) ||
            formattedTime.toLowerCase().includes(searchTerm.toLowerCase()) ||
            participants.includes(searchTerm);
    });

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

        const removedFilters = selectedFilters.filter(filter => !value.includes(filter));

        removedFilters.forEach(filter => {
            switch (filter) {
                case "Rango de Fecha":
                    setStartDate(null);
                    setEndDate(null);
                    setStartTime(null!);
                    setEndTime(null!);
                    break;
                case "Rango de Hora":
                    setStartTime(null!);
                    setEndTime(null!);
                    break;
                case "Nueva":
                    setIsNew(null!);
                    break;
                case "Educadores":
                    setSelectedEducators(null!);
                    break;
                case "Tipo de Instalación":
                    setSelectedFacilityTypes(null!);
                    break;
                case "Tipo de Actividad":
                    setSelectedActivityTypes(null!);
                    break;
                case "Edad Recomendada":
                    setMinAge(null);
                    setMaxAge(null);
                    break;
                case "Disponibilidad":
                    setAvailability(null!);
                    break;
                case "Esta Semana":
                    setToday(null!);
                    setTomorrow(null!);
                    setThisWeek(null!);
                    setDaysOfWeek(null!);
                    break;
                default:
                    break;
            }
        });

        setSelectedFilters(value);
        setFiltersOpen(value.length > 0);
    };

    const toggleFiltersOpen = () => {
        setFiltersOpen(!filtersOpen);
    };

    const handleApplyFilters = async () => {
        let valid = true;
        if (minAge !== null && (minAge < 2 || minAge > 16)) {
            setMinAgeError('La edad mínima debe estar entre 2 y 16 años.');
            valid = false;
        } else {
            setMinAgeError('');
        }
        if (maxAge !== null && (maxAge < 3 || maxAge > 17)) {
            setMaxAgeError('La edad máxima debe estar entre 3 y 17 años.');
            valid = false;
        } else {
            setMaxAgeError('');
        }

        if (!valid) return;

        const filters: ActivitiesFilters[] = [];

        filters.push({ type: 'Casos de Uso', useCase: 'ActivityView' })

        if (capacity !== null) {
            filters.push({ type: 'Capacidad', value: capacity.toString() })
        }
        if (startDate || endDate) {
            const startDateTime = startDate ? new Date(startDate) : null;
            const endDateTime = endDate ? new Date(endDate) : null;

            if (startDateTime && startTime) {
                const [hours, minutes] = startTime.split(':');
                startDateTime.setHours(parseInt(hours, 10), parseInt(minutes, 10));
            }

            if (endDateTime && endTime) {
                const [hours, minutes] = endTime.split(':');
                endDateTime.setHours(parseInt(hours, 10), parseInt(minutes, 10));
            }

            filters.push({
                type: 'Rango de Fecha',
                startDate: startDateTime ? startDateTime.toISOString() : undefined,
                endDate: endDateTime ? endDateTime.toISOString() : undefined
            });
        }
        if (startTime || endTime) {
            filters.push({
                type: 'Rango de Hora',
                startTime: startTime ? startTime.toString() : undefined,
                endTime: endTime ? endTime.toString() : undefined
            })
        }
        if (selectedEducators) {
            filters.push({ type: 'Educadores', value: selectedEducators.join(',') });
        }
        if (selectedFacilityTypes.length > 0) {
            filters.push({ type: 'Tipos de Instalaciones', value: selectedFacilityTypes.join(',') });
        }
        if (selectedActivityTypes.length > 0) {
            filters.push({ type: 'Tipos de Actividades', value: selectedActivityTypes.join(',') });
        }
        if (selectedFilters.includes("Edad Recomendada")) {
            filters.push({
                type: 'Edad Recomendada',
                minAge: minAge !== null ? minAge : 2,
                maxAge: maxAge !== null ? maxAge : 17
            });
        }
        if (availability) {
            filters.push({ type: 'Disponibilidad', value: availability });
        }
        if (selectedFilters.includes("De Esta Semana")) {
            if (today) filters.push({ type: 'Hoy', value: 'true' });
            if (tomorrow) filters.push({ type: 'Mañana', value: 'true' });
            if (thisWeek) filters.push({ type: 'Esta Semana', value: 'true' });
            if (daysOfWeek.length > 0) filters.push({ type: 'Días de la Semana', value: daysOfWeek.join(',') });
        }

        if (selectedFilters.includes("Nueva")) {
            if (isNew) {
                filters.push({ type: 'Nueva', value: 'true' })
            }
            else {
                filters.push({ type: 'Nueva', value: 'false' })
            }
        }

        try {
            const response: ListActivityResponse = await activityService.getAllActivities(filters);
            const activitiesArrayR: Activity[] = Array.isArray(response.result)
                ? response.result as Activity[]
                : Array.from(response.result);
            //asegurarme de que las actividades mostradas del filtrado no sean pendientes
            const activitiesArray: Activity[] = activitiesArrayR.filter(activity => !activity.pending)

            cacheActivityImages(activitiesArray);
            setActivities(activitiesArray);
            cacheService.saveActivities(activitiesArray);
            cacheService.saveImages(activityImages);
        } catch (error) {
            console.error('Error applying filters:', error);
            const cachedActivities = cacheService.loadActivities();
            const cachedImages = cacheService.loadImages();
            setActivities(cachedActivities);
            setActivityImages(cachedImages);
        }
    };

    const menuItems = [
        { label: "De Esta Semana", value: "De Esta Semana" },
        { label: "Calificación", value: "Calificación" },
        { label: "Rango de Fecha", value: "Rango de Fecha" },
        { label: "Rango de Hora", value: "Rango de Hora" },
        { label: "Por Educadores", value: "Educadores" },
        { label: "Tipos de Instalaciones", value: "Tipos de Instalaciones" },
        { label: "Tipos de Actividades", value: "Tipos de Actividades" },
        { label: "Edad Recomendada", value: "Edad Recomendada" },
        { label: "Pública o Privada", value: "Disponibilidad" },
        { label: "Nueva o No", value: "Nueva" },
        { label: "Capacidad Disponible", value: "Capacidad Disponible" },
    ];

    return (
        <LocalizationProvider dateAdapter={AdapterDateFns}>
            <Box sx={{ width: '100vw', minHeight: '100vh', py: 4, px: 2, backgroundColor: '#f8f9fa' }}>
                {/* Buscador con filtros */}
                <Box sx={{ display: 'flex', justifyContent: 'center', mb: 4 }}>
                    <SearchBar searchTerm={searchTerm} handleSearchChange={handleSearchChange} labelText='Actividades' />
                    <FilterSelect
                        selectedFilters={selectedFilters}
                        handleFilterChange={handleFilterChange}
                        handleApplyFilters={handleApplyFilters}
                        menuItems={menuItems}
                    />
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
                                    {filter === "De Esta Semana" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center', mt: 2 }}>
                                            <Typography>🗓️ De esta semana:</Typography>
                                            <Box sx={{ display: 'flex', alignItems: 'center', ml: 2 }}>
                                                <FormControlLabel
                                                    control={<Checkbox checked={today} onChange={(e) => setToday(e.target.checked)} />}
                                                    label="Hoy"
                                                />
                                                <FormControlLabel
                                                    control={<Checkbox checked={tomorrow} onChange={(e) => setTomorrow(e.target.checked)} />}
                                                    label="Mañana"
                                                />
                                                <FormControlLabel
                                                    control={<Checkbox checked={thisWeek} onChange={(e) => setThisWeek(e.target.checked)} />}
                                                    label="Esta Semana"
                                                />
                                                <DaySelector daysOfWeek={daysOfWeek} setDaysOfWeek={setDaysOfWeek} />
                                            </Box>
                                        </Box>
                                    )}
                                    {filter === "Nueva" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center', mt: 2 }}>
                                            <Typography>✨ Es Nueva:</Typography>
                                            <Box sx={{ display: 'flex', alignItems: 'center', ml: 2 }}>
                                                <FormControlLabel
                                                    control={<Checkbox checked={isNew} onChange={(e) => setIsNew(e.target.checked)} />}
                                                    label=""
                                                />
                                            </Box>
                                        </Box>
                                    )}
                                    {filter === "Rango de Fecha" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography>📅 Fecha desde:</Typography>
                                            <DatePicker
                                                value={startDate}
                                                onChange={(newValue: Date | null) => setStartDate(newValue)}
                                                slotProps={{
                                                    textField: {
                                                        sx: { ml: 2 },
                                                    },
                                                }}
                                            />
                                            <Typography sx={{ ml: 2 }}>Hasta:</Typography>
                                            <DatePicker
                                                value={endDate}
                                                onChange={(newValue: Date | null) => setEndDate(newValue)}
                                                slotProps={{
                                                    textField: {
                                                        sx: { ml: 2 },
                                                    },
                                                }}
                                            />
                                        </Box>
                                    )}
                                    {filter === "Rango de Hora" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography>⏰ Hora desde:</Typography>
                                            <TextField
                                                type="time"
                                                value={startTime}
                                                onChange={(e) => setStartTime(e.target.value)}
                                                sx={{ ml: 2 }}
                                            />
                                            <Typography sx={{ ml: 2 }}>Hasta:</Typography>
                                            <TextField
                                                type="time"
                                                value={endTime}
                                                onChange={(e) => setEndTime(e.target.value)}
                                                sx={{ ml: 2 }}
                                            />
                                        </Box>
                                    )}

                                    {filter === "Educadores" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography>👨‍🏫 Educadores:</Typography>
                                            <FormControl sx={{ ml: 2, minWidth: 350 }} variant="outlined">
                                                <InputLabel id="educator-select-label">📏 Escoger Educadores</InputLabel>
                                                <Select
                                                    labelId="educator-select-label"
                                                    multiple
                                                    value={selectedEducators}
                                                    onChange={(e) => setSelectedEducators(e.target.value as string[])}
                                                    label="📏 Escoger Educadores"
                                                    renderValue={(selected) =>
                                                        selected.map(id => {
                                                            const educator = educators.find(e => e.id === id);
                                                            return educator ? `${educator.displayName}` : id;
                                                        }).join(', ') // Separar por comas
                                                    }
                                                    sx={{
                                                        minHeight: '56px',
                                                        maxHeight: '100vh',
                                                        width: 'auto', // Ancho fijo por defecto
                                                        '& .MuiSelect-select': {
                                                            paddingTop: '8px',
                                                            paddingBottom: '8px',
                                                            textAlign: 'left',
                                                            whiteSpace: 'nowrap', // No permite el salto de línea
                                                            overflow: 'hidden',
                                                            textOverflow: 'ellipsis', // Añade puntos suspensivos
                                                            display: 'block',
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
                                                    {educators.map((educator) => (
                                                        <MenuItem key={educator.id} value={educator.id}>
                                                            {educator.displayName}
                                                        </MenuItem>
                                                    ))}
                                                </Select>
                                            </FormControl>
                                        </Box>
                                    )}

                                    {filter === "Tipos de Instalaciones" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography>🏟️ Tipos de Instalaciones:</Typography>
                                            <FormControl sx={{ ml: 2, minWidth: 200 }} variant="outlined">
                                                <InputLabel id="facility-type-select-label">🏟️ Escoger Tipos</InputLabel>
                                                <Select
                                                    labelId="facility-type-select-label"
                                                    multiple
                                                    value={selectedFacilityTypes}
                                                    onChange={(e) => setSelectedFacilityTypes(e.target.value as string[])}
                                                    label="🏟️ Escoger Tipos"
                                                    renderValue={(selected) => selected.join(', ')}
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
                                                    {facilityTypes.map((typeOption) => (
                                                        <MenuItem key={typeOption} value={typeOption}>
                                                            {typeOption}
                                                        </MenuItem>
                                                    ))}
                                                </Select>
                                            </FormControl>
                                        </Box>
                                    )}
                                    {filter === "Tipos de Actividades" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography>🎢 Tipos de Actividades:</Typography>
                                            <FormControl sx={{ ml: 2, minWidth: 200 }} variant="outlined">
                                                <InputLabel id="activity-type-select-label">🎢 Escoger Tipos</InputLabel>
                                                <Select
                                                    labelId="activity-type-select-label"
                                                    multiple
                                                    value={selectedActivityTypes}
                                                    onChange={(e) => setSelectedActivityTypes(e.target.value as string[])}
                                                    label="🎢 Escoger Tipos"
                                                    renderValue={(selected) => selected.join(', ')}
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
                                                    {activityTypes.map((typeOption) => (
                                                        <MenuItem key={typeOption} value={typeOption}>
                                                            {typeOption}
                                                        </MenuItem>
                                                    ))}
                                                </Select>
                                            </FormControl>
                                        </Box>
                                    )}
                                    {filter === "Edad Recomendada" && (
                                        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-start' }}>
                                            <Typography>👶 Edad recomendada:</Typography>
                                            <Box sx={{ display: 'flex', alignItems: 'center', mt: 1 }}>
                                                <Typography>Mayor o igual que</Typography>
                                                <TextField
                                                    type="number"
                                                    value={minAge !== null ? minAge : ''}
                                                    onChange={(e) => {
                                                        const value = e.target.value;
                                                        setMinAge(value ? parseInt(value, 10) : null);
                                                    }}
                                                    slotProps={{
                                                        htmlInput: {
                                                            min: 2,
                                                            max: 16,
                                                            style: { textAlign: 'center' },
                                                        }
                                                    }}
                                                    error={Boolean(minAgeError)}
                                                    sx={{
                                                        ml: 2,
                                                        width: '50px',
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
                                                    value={maxAge !== null ? maxAge : ''}
                                                    onChange={(e) => {
                                                        const value = e.target.value;
                                                        setMaxAge(value ? parseInt(value, 10) : null);
                                                    }}
                                                    slotProps={{
                                                        htmlInput: {
                                                            min: 3,
                                                            max: 17,
                                                            style: { textAlign: 'center' },
                                                        }
                                                    }}
                                                    error={Boolean(maxAgeError)}
                                                    sx={{
                                                        ml: 2,
                                                        width: '50px',
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
                                            {minAgeError && <Typography color="error" sx={{ mt: 1 }}>{minAgeError}</Typography>}
                                            {maxAgeError && <Typography color="error" sx={{ mt: 1 }}>{maxAgeError}</Typography>}
                                        </Box>
                                    )}
                                    {filter === "Disponibilidad" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography>🔓 Disponibilidad:</Typography>
                                            <FormControl sx={{ ml: 2, minWidth: 250 }} variant="outlined">
                                                <InputLabel id="availability-select-label">🔓 Escoger Disponibilidad</InputLabel>
                                                <Select
                                                    labelId="availability-select-label"
                                                    value={availability.toString()}
                                                    onChange={(e) => setAvailability(e.target.value)}
                                                    label="🔓 Escoger Disponibilidad"
                                                >
                                                    <MenuItem value="true">Público</MenuItem>
                                                    <MenuItem value="false">Privado</MenuItem>
                                                </Select>
                                            </FormControl>
                                        </Box>
                                    )}
                                    {filter === "Capacidad Disponible" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography>🎒 Capacidad disponible de:</Typography>
                                            <FormControl sx={{ ml: 2, width: 40 }} variant="outlined">
                                                <Input
                                                    type="number"
                                                    value={capacity !== null ? capacity : ''}
                                                    onChange={(e) => {
                                                        const value = e.target.value;
                                                        setCapacity(value ? parseInt(value, 10) : null);
                                                    }}
                                                    slotProps={{
                                                        input: {
                                                            min: 0,
                                                            style: { MozAppearance: 'textfield', textAlign: 'center' },
                                                        },
                                                    }}
                                                    sx={{
                                                        '& input[type=number]': {
                                                            MozAppearance: 'textfield',
                                                            textAlign: 'center',
                                                            '&::-webkit-outer-spin-button, &::-webkit-inner-spin-button': {
                                                                WebkitAppearance: 'none',
                                                                margin: 0,
                                                            },
                                                        },
                                                    }}
                                                />
                                            </FormControl>
                                            <Typography sx={{ ml: 1 }}>niños.</Typography>
                                        </Box>
                                    )}
                                </Box>
                            ))}
                        </Collapse>
                    </Box>
                )}

                {/* Botón de "Crear Nueva Actividad" visible para Educadores */}
                {rol === 'Educator' && isAuthenticated && (
                    <Box sx={{ display: 'flex', justifyContent: 'center', mb: 4 }}>
                        <CreateActivityButton />
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
                            <ActivityCard activity={{ ...activity, image: activityImages[activity.id] }} />
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
        </LocalizationProvider>
    );
}

export default ActivitiesPage;
