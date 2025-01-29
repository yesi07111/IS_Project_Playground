import { useCallback, useEffect, useState } from "react";
import { Facility, ListFacilityResponse } from "../interfaces/Facility";
import { facilityService } from "../services/facilityService";
import { cacheService } from "../services/cacheService";
import { Box, Button, Collapse, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, FormControl, Grid2, IconButton, Input, InputLabel, MenuItem, Pagination, Select, SelectChangeEvent, Typography } from "@mui/material";
import { FacilityFilters } from "../interfaces/Filters";
import { SearchBar } from "../components/features/StyledSearchBar";
import { FilterSelect } from "../components/features/StyledFilters";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import { useNavigate } from "react-router-dom";
import { useAuth } from "../components/auth/authContext";
import GenericCard from "../components/features/GenericCard";

const FacilitiesPage: React.FC<{ reload: boolean }> = ({ reload }) => {
    const [searchTerm, setSearchTerm] = useState(''); //termino de busqueda ingresado por el admin
    const [currentPage, setCurrentPage] = useState(1); //pagina actual en paginacion
    const [selectedFilters, setSelectedFilters] = useState<string[]>([]); //filtros seleccionados por el admin
    const [filtersOpen, setFiltersOpen] = useState(false); //seccion de filtros abierta o no
    const [name, setName] = useState(''); //nombre del usuario elegido
    const [location, setLocation] = useState(''); //apellido del usuario elegido
    const [type, setType] = useState(''); //nombre de usuario elegido
    const [maximumCapacity, setMaximumCapacity] = useState<number | null>(null); //email del usuario elegido
    const [usagePolicy, setUsagePolicy] = useState(''); //rol elegido
    const [facilities, setFacilities] = useState<Facility[]>([]); //lista de recursos cargados desd ela API
    const [facilityTypes, setFacilityTypes] = useState<string[]>([]); //lista de tipos de instalaciones disponibles
    const [facilityLocations, setFacilityLocations] = useState<string[]>([]);
    const myRole = localStorage.getItem('authUserRole');
    const { isAuthenticated } = useAuth();
    const navigate = useNavigate();
    const [openDialog, setOpenDialog] = useState(false);
    const [selectedFacility, setSelectedFacility] = useState<string | null>(null);

    const facilitiesPerPage = 9;

    const fetchAllFacilityTypes = useCallback(async () => {
        try {
            const response = await facilityService.getAllFacilities({ useCase: 'AllTypes' });
            const typesArray: string[] = Array.isArray(response.result) ? response.result as unknown as string[] : Array.from(response.result);
            setFacilityTypes(typesArray);
            cacheService.saveFacilityTypes(typesArray);
        }
        catch (error) {
            console.error('Error fetching facility types: ', error);
            const catchedFacilityTypes = cacheService.loadFacilityTypes();
            setFacilityTypes(catchedFacilityTypes);
        }
    }, []);

    const fetchAllLocations = useCallback(async () => {
        try {
            const response = await facilityService.getAllFacilities({ useCase: 'AllLocations' });
            const typesArray: string[] = Array.isArray(response.result) ? response.result as unknown as string[] : Array.from(response.result);
            setFacilityLocations(typesArray);
            cacheService.saveFacilityLocations(typesArray);
        } catch (error) {
            console.error('Error fetching facility types:', error);
            const cachedFacilityTypes = cacheService.loadFacilityLocations();
            setFacilityLocations(cachedFacilityTypes);
        }
    }, []);

    const fetchAllFacilities = useCallback(async () => {
        try {
            const facilities: ListFacilityResponse = await facilityService.getAllFacilities({ useCase: 'AdminEducatorView' });
            const facilitiesArray: Facility[] = Array.isArray(facilities.result)
                ? facilities.result as Facility[]
                : [];

            setFacilities(facilitiesArray);
            cacheService.saveFacilities(facilitiesArray);
        }
        catch (error) {
            console.error('Error fetching facilities:', error);
            const cachedFacilities = cacheService.loadFacilities();
            setFacilities(cachedFacilities || []);
        }
    }, []);

    const fetchInitialData = useCallback(async () => {
        await Promise.all([fetchAllFacilityTypes(), fetchAllLocations(), fetchAllFacilities()]);
    }, [fetchAllFacilityTypes, fetchAllLocations, fetchAllFacilities]);

    useEffect(() => {
        fetchInitialData();
    }, []);

    useEffect(() => {
        if (selectedFilters.length === 0) {
            fetchAllFacilities();
        }
    }, [selectedFilters]);

    useEffect(() => {
        if (reload) {
            cacheService.clearCache()
            fetchInitialData();
        }
    }, [reload]);

    useEffect(() => {
        const cachedFacilities = cacheService.loadFacilities();
        const cachedFacilityTypes = cacheService.loadFacilityTypes();
        const cachedFacilityLocations = cacheService.loadFacilityLocations();
        if (cachedFacilities.length > 0) {
            setFacilities(cachedFacilities);
        }
        setFacilityTypes(cachedFacilityTypes);
        setFacilityLocations(cachedFacilityLocations);
    }, []);

    const filteredFacilities = facilities.filter(facility => {
        return facility.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
            facility.location.toLowerCase().includes(searchTerm.toLowerCase()) ||
            facility.maximumCapacity.toString().includes(searchTerm.toLowerCase()) ||
            facility.type.toLowerCase().includes(searchTerm.toLowerCase()) ||
            facility.usagePolicy.toLowerCase().includes(searchTerm.toLowerCase());
    });

    const totalPages = Math.ceil(filteredFacilities.length / facilitiesPerPage);

    const currentFacilities = filteredFacilities.slice(
        (currentPage - 1) * facilitiesPerPage,
        currentPage * facilitiesPerPage
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
                case "Nombre":
                    setName('');
                    break;
                case "Ubicación":
                    setLocation('');
                    break;
                case "Capacidad Máxima":
                    setMaximumCapacity(null);
                    break;
                case "Tipo de Instalación":
                    setType('');
                    break;
                case "Política de Uso":
                    setUsagePolicy('');
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
        // Reiniciar la paginación a la primera página
        setCurrentPage(1);

        const filters: FacilityFilters = {
            useCase: 'AdminEducatorView'
        };

        if (name) {
            filters.name = name;
        }
        if (location) {
            filters.location = location;
        }
        if (type) {
            filters.type = type;
        }
        if (usagePolicy) {
            filters.usagePolicy = usagePolicy;
        }
        if (maximumCapacity) {
            filters.maximumCapacity = maximumCapacity;
        }

        try {
            const facilities: ListFacilityResponse = await facilityService.getAllFacilities(filters);
            const facilitiesArray: Facility[] = Array.isArray(facilities.result)
                ? facilities.result as Facility[]
                : [];

            setFacilities(facilitiesArray);
            cacheService.saveFacilities(facilitiesArray);
        }
        catch (error) {
            console.error('Error fetching facilities:', error);
            const cachedFacilities = cacheService.loadFacilities();
            setFacilities(cachedFacilities || []);
        }
    }

    const handleRemoveFacility = (id: string) => {
        setSelectedFacility(id); // Guardar datos relevantes
        setOpenDialog(true); // Abrir diálogo de confirmación
    };

    const confirmRemoveFacility = async () => {
        if (selectedFacility) {
            await facilityService.removeFacility(selectedFacility);
            setOpenDialog(false); // Cerrar el diálogo
            setSelectedFacility(null); // Limpiar selección
        }
    };

    const cancelRemoveFacility = () => {
        setOpenDialog(false); // Cerrar el diálogo sin hacer nada
        setSelectedFacility(null); // Limpiar selección
    };

    const menuItems = [
        { label: "Nombre", value: "Nombre" },
        { label: "Ubicación", value: "Ubicación" },
        { label: "Tipo de Instalación", value: "Tipo de Instalación" },
        { label: "Política de Uso", value: "Política de Uso" },
        { label: "Capacidad Máxima", value: "Capacidad Máxima" },
    ];

    return (
        <Box sx={{ width: '100vw', minHeight: '100vh', py: 4, px: 2, backgroundColor: '#f8f9fa' }}>
            {/* Buscador con filtros */}
            <Box sx={{ display: 'flex', justifyContent: 'center', mb: 4 }}>
                <SearchBar searchTerm={searchTerm} handleSearchChange={handleSearchChange} labelText='Instalaciones' />
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
                        <Typography variant="h6">Filtros</Typography>
                        <IconButton onClick={toggleFiltersOpen}>
                            {filtersOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                        </IconButton>
                    </Box>
                    <Collapse in={filtersOpen}>
                        {selectedFilters.map((filter) => (
                            <Box key={filter} sx={{ mt: 2 }}>
                                {filter === "Nombre" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>Buscar instalación por nombre:</Typography>
                                        <FormControl sx={{ ml: 2, width: 200 }} variant="outlined">
                                            <Input
                                                type="text"
                                                value={name || ''}
                                                onChange={(e) => {
                                                    const value = e.target.value;
                                                    setName(value || '');
                                                }}
                                                placeholder="Escribe el nombre"
                                                sx={{
                                                    textAlign: 'left',
                                                    '&::placeholder': {
                                                        color: '#aaa',
                                                        fontStyle: 'italic',
                                                    },
                                                }}
                                            />
                                        </FormControl>
                                    </Box>
                                )}
                                {filter === "Política de Uso" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>Buscar instalación por política de uso:</Typography>
                                        <FormControl sx={{ ml: 2, width: 200 }} variant="outlined">
                                            <Input
                                                type="text"
                                                value={usagePolicy || ''}
                                                onChange={(e) => {
                                                    const value = e.target.value;
                                                    setUsagePolicy(value || '');
                                                }}
                                                placeholder="Escribe la política de uso"
                                                sx={{
                                                    textAlign: 'left',
                                                    '&::placeholder': {
                                                        color: '#aaa',
                                                        fontStyle: 'italic',
                                                    },
                                                }}
                                            />
                                        </FormControl>
                                    </Box>
                                )}
                                {filter === "Tipo de Instalación" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>Tipos de Instalaciones:</Typography>
                                        <FormControl sx={{ ml: 2, minWidth: 200 }} variant="outlined">
                                            <InputLabel id="facility-One-type-select-label">Escoger Tipo</InputLabel>
                                            <Select
                                                labelId="facility-One-type-select-label"
                                                value={type}
                                                onChange={(e) => setType(e.target.value || '')}
                                                label="Escoger Tipo"
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
                                {filter === "Capacidad Máxima" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>Capacidad máxima de:</Typography>
                                        <FormControl sx={{ ml: 2, width: 40 }} variant="outlined">
                                            <Input
                                                type="number"
                                                value={maximumCapacity !== null ? maximumCapacity : ''}
                                                onChange={(e) => {
                                                    const value = e.target.value;
                                                    setMaximumCapacity(value ? parseInt(value, 10) : null);
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
                                {filter === "Ubicación" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>Ubicaciones:</Typography>
                                        <FormControl sx={{ ml: 2, minWidth: 200 }} variant="outlined">
                                            <InputLabel id="facility-location-select-label">Escoger Ubicación</InputLabel>
                                            <Select
                                                labelId="facility-location-select-label"
                                                value={location}
                                                onChange={(e) => setLocation(e.target.value || '')}
                                                label="Escoger Ubicación"
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
                                                {facilityLocations.map((typeOption) => (
                                                    <MenuItem key={typeOption} value={typeOption}>
                                                        {typeOption}
                                                    </MenuItem>
                                                ))}
                                            </Select>
                                        </FormControl>
                                    </Box>
                                )}
                            </Box>
                        ))}
                    </Collapse>
                </Box>
            )}

            {/* Botón de "Crear Nueva Instalación" visible para Admins */}
            {myRole === 'Admin' && isAuthenticated && (
                <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center', gap: 2 }}>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={() => navigate('/createFacility')}
                    >
                        Crear Nueva Instalación
                    </Button>
                </Box>
            )}

            {/*  Separador entre el botón y el listado de instalaciones  */}
            <Box sx={{ height: 16 }} />

            {/* Grid de instlaciones */}
            <Grid2 container spacing={4}>
                {currentFacilities.map(facility => (
                    <Grid2
                        size={{ xs: 12, sm: 6, md: 4 }}
                        key={facility.id}
                        sx={{
                            display: 'flex',
                            justifyContent: 'center',
                        }}
                    >
                        <GenericCard
                            title="Información de la Instalación"
                            fields={[
                                { label: 'Nombre', value: facility.name },
                                { label: 'Tipo', value: facility.type },
                                { label: 'Capacidad Máxima', value: facility.maximumCapacity },
                                { label: 'Política de Uso', value: facility.usagePolicy },
                                { label: 'Ubicación', value: facility.location },
                            ]}
                            actions={myRole === 'Admin' && isAuthenticated ?
                                [
                                    {
                                        label: 'Modificar Instalación',
                                        onClick: () => navigate(
                                            `/updateFacility?id=${facility.id}&name=${facility.name}&type=${facility.type}&location=${facility.location}&maximumCapacity=${facility.maximumCapacity}&usagePolicy=${facility.usagePolicy}`
                                        ),
                                    },
                                    {
                                        label: 'Eliminar Instalación Permanentemente',
                                        onClick: () => handleRemoveFacility(facility.id),
                                    },
                                ] : []}
                        />
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

            {/* Diálogo de confirmación */}
            <Dialog open={openDialog} onClose={cancelRemoveFacility}>
                <DialogTitle>Confirmación</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        ¿Estás seguro de que quieres eliminar esta instalación PERMANENTEMENTE?
                        Todas las actividades, reservas, reseñas y recursos asociados a esta también serán borrados.
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={cancelRemoveFacility} color="primary">
                        No
                    </Button>
                    <Button onClick={confirmRemoveFacility} color="primary" autoFocus>
                        Sí
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    )
}

export default FacilitiesPage;