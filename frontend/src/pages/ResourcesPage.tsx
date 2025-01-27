import { useCallback, useEffect, useState } from "react";
import { ListResourceResponse, Resource } from "../interfaces/Resource";
import { facilityService } from "../services/facilityService";
import { cacheService } from "../services/cacheService";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { SearchBar } from "../components/features/StyledSearchBar";
import { FilterSelect } from "../components/features/StyledFiltersResource";
import { Box, Collapse, FormControl, Grid2, IconButton, Input, InputLabel, MenuItem, Pagination, Select, SelectChangeEvent, TextField, Typography } from "@mui/material";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import { ResourceFilters } from "../interfaces/ResourceFilters";
import { resourceService } from "../services/resourceService";
import GenericCard from "../components/features/GenericCard";
import { useAuth } from "../components/auth/authContext";
import { useNavigate } from "react-router-dom";

const ResourcesPage: React.FC<{ reload: boolean }> = ({ reload }) => {
    const [searchTerm, setSearchTerm] = useState(''); //termino de busqueda ingresado por el usuario
    const [currentPage, setCurrentPage] = useState(1); //pagina actual en paginacion
    const [selectedFilters, setSelectedFilters] = useState<string[]>([]); //filtros seleccionados por el usuario
    const [filtersOpen, setFiltersOpen] = useState(false); //seccion de filtros abierta o no
    const [selectedFacilityTypes, setSelectedFacilityTypes] = useState<string[]>([]); //filtra por tipo de instalacion los recursos
    const [selectedResourceTypes, setSelectedResourceTypes] = useState<string[]>([]); //filtra por tipo de recurso
    const [selectedResourceName, setSelectedResourceName] = useState<string>(''); //filtra por nombre de recurso ej:'silla'
    const [selectedResourceCondition, setSelectedResourceCondition] = useState<string>(''); //filtra por estad del recurso ej: 'roto'
    const [minUseFrequency, setMinUseFrequency] = useState<number | null>(null); //filtra frecuencia de uso minima
    const [maxUseFrequency, setMaxUseFrequency] = useState<number | null>(null); //filtra frecuencia de uso maxima
    const [resources, setResources] = useState<Resource[]>([]); //lista de recursos cargados desd ela API
    const [minUseFrequencyError, setMinUseFrequencyError] = useState<string>(''); //error para frecuencia de uso minima invalida
    const [maxUseFrequencyError, setMaxUseFrequencyError] = useState<string>(''); //error para frecuencia de uso maxima invalida
    const [facilityTypes, setFacilityTypes] = useState<string[]>([]); //lista de tipos de instalaciones disponibles
    const [resourceTypes, setResourceTypes] = useState<string[]>([]); //lista de tipos de recurso disponibles

    const { isAuthenticated } = useAuth();
    const role = localStorage.getItem('authUserRole');
    const navigate = useNavigate();

    const resourcesPerPage = 6;

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

    const fetchAllResourceTypes = useCallback(async () => {
        try {
            const response = await resourceService.getAllResources([{ type: 'AllTypes' }]);
            const typesArray: string[] = Array.isArray(response.result) ? response.result as unknown as string[] : [];
            setResourceTypes(typesArray);
            cacheService.saveResourceTypes(typesArray);
        }
        catch (error) {
            console.error('Error fetching resource types:', error);
            const cachedResourceTypes = cacheService.loadResourceTypes();
            setResourceTypes(cachedResourceTypes);
        }
    }, []);

    const fetchAllResources = useCallback(async (filters: ResourceFilters[] = []) => {
        try {
            const response: ListResourceResponse = await resourceService.getAllResources(filters);
            const resourcesArray: Resource[] = Array.isArray(response.result)
                ? response.result as Resource[]
                : [];

            setResources(resourcesArray);
            cacheService.saveResources(resourcesArray);
        } catch (error) {
            console.error('Error fetching resources:', error);
            const cachedResources = cacheService.loadResources();
            setResources(cachedResources || []);
        }
    }, []);

    const fetchInitialData = useCallback(async () => {
        await Promise.all([fetchAllFacilityTypes(), fetchAllResourceTypes(), fetchAllResources()]);
    }, [fetchAllFacilityTypes, fetchAllResourceTypes, fetchAllResources]);

    useEffect(() => {
        fetchInitialData();
    }, []);

    useEffect(() => {
        if (selectedFilters.length === 0) {
            fetchAllResources();
        }
    }, [selectedFilters]);

    useEffect(() => {
        if (reload) {
            cacheService.clearCache()
            fetchInitialData();
        }
    }, [reload]);

    useEffect(() => {
        const cachedResources = cacheService.loadResources();
        const cachedFacilityTypes = cacheService.loadFacilityTypes();
        const cachedResourceTypes = cacheService.loadResourceTypes();
        if (cachedResources.length > 0) {
            setResources(cachedResources);
        }
        setFacilityTypes(cachedFacilityTypes);
        setResourceTypes(cachedResourceTypes);
    }, []);

    const filteredResources = resources.filter(resource => {
        return resource.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
            resource.type.toLowerCase().includes(searchTerm.toLowerCase()) ||
            resource.condition.toLowerCase().includes(searchTerm.toLowerCase()) ||
            resource.facilityLocation.toLowerCase().includes(searchTerm.toLowerCase()) ||
            resource.useFrequency.toString().includes(searchTerm.toLowerCase()) ||
            resource.facilityName.toLowerCase().includes(searchTerm.toLocaleLowerCase()) ||
            resource.facilityType.toLocaleLowerCase().includes(searchTerm.toLowerCase());
    });

    const totalPages = Math.ceil(filteredResources.length / resourcesPerPage);

    const currentResources = filteredResources.slice(
        (currentPage - 1) * resourcesPerPage,
        currentPage * resourcesPerPage
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
                case "Tipo de Instalación":
                    setSelectedFacilityTypes([]);
                    break;
                case "Tipo de Recurso":
                    setSelectedResourceTypes([]);
                    break;
                case "Nombre":
                    setSelectedResourceName('');
                    break;
                case "Estado del Recurso":
                    setSelectedResourceCondition('');
                    break;
                case "Frecuencia de Uso":
                    setMinUseFrequency(null);
                    setMaxUseFrequency(null);
                    break;
                default:
                    break;
            }
        });

        setSelectedFilters(value);
        setFiltersOpen(value.length > 0);
    }

    const toggleFiltersOpen = () => {
        setFiltersOpen(!filtersOpen);
    };

    const handleApplyFilters = async () => {
        let valid = true;

        if (minUseFrequency !== null && minUseFrequency < 0) {
            setMinUseFrequencyError('La frecuencia mínima de uso de un recurso no puede ser menor que 0.');
            valid = false;
        }
        else if (minUseFrequency !== null && maxUseFrequency !== null && minUseFrequency > maxUseFrequency) {
            setMinUseFrequencyError('La frecuencia mínima de uso de un recurso no puede ser mayor que la frecuencia máxima.');
            valid = false;
        }
        else {
            setMinUseFrequencyError('');
        }

        if (maxUseFrequency !== null && maxUseFrequency < 0) {
            setMaxUseFrequencyError('La frecuencia máxima de uso de un recurso no puede ser menor que 0.');
            valid = false;
        }
        else {
            setMaxUseFrequencyError('');
        }

        if (!valid) return;

        // Reiniciar la paginación a la primera página
        setCurrentPage(1);


        const filters: ResourceFilters[] = [];

        if (selectedFacilityTypes.length > 0) {
            filters.push({ type: 'Tipos de Instalaciones', value: selectedFacilityTypes.join(',') });
        }
        if (selectedResourceTypes.length > 0) {
            filters.push({ type: 'Tipos de Recursos', value: selectedResourceTypes.join(',') });
        }
        if (selectedResourceName) {
            filters.push({ type: 'Nombre', value: selectedResourceName });
        }
        if (selectedResourceCondition) {
            filters.push({ type: 'Estado', value: selectedResourceCondition });
        }
        if (minUseFrequency || maxUseFrequency) {
            filters.push({
                type: 'Rango de Frecuencia de uso',
                minUseFrequency: minUseFrequency ? minUseFrequency : undefined,
                maxUseFrequency: maxUseFrequency ? maxUseFrequency : undefined
            })
        }

        try {
            const response: ListResourceResponse = await resourceService.getAllResources(filters);
            const resourcesArray: Resource[] = Array.isArray(response.result)
                ? response.result as Resource[]
                : Array.from(response.result);

            setResources(resourcesArray);
            cacheService.saveResources(resourcesArray);
        }
        catch (error) {
            console.error('Error applying filters:', error);
            const catchedResources = cacheService.loadResources();
            setResources(catchedResources);
        }
    }

    const handleDefineUsageFrequency = (id: string) => {
        navigate(`/define-usage-frequency/${id}`);
      };

    const menuItems = [
        { label: "Tipo de Instalación", value: "Tipo de Instalación" },
        { label: "Tipo de Recurso", value: "Tipo de Recurso" },
        { label: "Nombre", value: "Nombre" },
        { label: "Estado del Recurso", value: "Estado del Recurso" },
        { label: "Frecuencia de Uso", value: "Frecuencia de Uso" },
    ];

    return (
        <LocalizationProvider dateAdapter={AdapterDateFns}>
            <Box sx={{ width: '100vw', minHeight: '100vh', py: 4, px: 2, backgroundColor: '#f8f9fa' }}>
                {/* Buscador con filtros */}
                <Box sx={{ display: 'flex', justifyContent: 'center', mb: 4 }}>
                    <SearchBar searchTerm={searchTerm} handleSearchChange={handleSearchChange} labelText='Recursos' />
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
                                    {filter === "Tipo de Instalación" && (
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
                                    {filter === "Tipo de Recurso" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography>🔧 Tipos de Recursos:</Typography>
                                            <FormControl sx={{ ml: 2, minWidth: 200 }} variant="outlined">
                                                <InputLabel id="resource-type-select-label">🔧 Escoger Tipos</InputLabel>
                                                <Select
                                                    labelId="resource-type-select-label"
                                                    multiple
                                                    value={selectedResourceTypes}
                                                    onChange={(e) => setSelectedResourceTypes(e.target.value as string[])}
                                                    label="🔧 Escoger Tipos"
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
                                                    {resourceTypes.map((typeOption) => (
                                                        <MenuItem key={typeOption} value={typeOption}>
                                                            {typeOption}
                                                        </MenuItem>
                                                    ))}
                                                </Select>
                                            </FormControl>
                                        </Box>
                                    )}
                                    {filter === "Frecuencia de Uso" && (
                                        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-start' }}>
                                            <Typography>🔧 Frecuencia de Uso:</Typography>
                                            <Box sx={{ display: 'flex', alignItems: 'center', mt: 1 }}>
                                                <Typography>Mayor o igual que</Typography>
                                                <TextField
                                                    type="number"
                                                    value={minUseFrequency !== null ? minUseFrequency : ''}
                                                    onChange={(e) => {
                                                        const value = e.target.value;
                                                        setMinUseFrequency(value ? parseInt(value, 10) : null);
                                                    }}
                                                    slotProps={{
                                                        htmlInput: {
                                                            min: 0,
                                                            style: { textAlign: 'center' },
                                                        }
                                                    }}
                                                    error={Boolean(minUseFrequencyError)}
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
                                                    value={maxUseFrequency !== null ? maxUseFrequency : ''}
                                                    onChange={(e) => {
                                                        const value = e.target.value;
                                                        setMaxUseFrequency(value ? parseInt(value, 10) : null);
                                                    }}
                                                    slotProps={{
                                                        htmlInput: {
                                                            min: 0,
                                                            style: { textAlign: 'center' },
                                                        }
                                                    }}
                                                    error={Boolean(maxUseFrequencyError)}
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
                                            {minUseFrequencyError && <Typography color="error" sx={{ mt: 1 }}>{minUseFrequencyError}</Typography>}
                                            {maxUseFrequencyError && <Typography color="error" sx={{ mt: 1 }}>{maxUseFrequencyError}</Typography>}
                                        </Box>
                                    )}
                                    {filter === "Estado del Recurso" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography>🔧 Estado del Recurso:</Typography>
                                            <FormControl sx={{ ml: 2, minWidth: 250 }} variant="outlined">
                                                <InputLabel id="resource-condition-select-label">🔓 Escoger Estado</InputLabel>
                                                <Select
                                                    labelId="resource-condition-select-label"
                                                    value={selectedResourceCondition}
                                                    onChange={(e) => setSelectedResourceCondition(e.target.value)}
                                                    label="🔧 Escoger Estado del Recurso"
                                                >
                                                    <MenuItem value="Bueno">Bueno</MenuItem>
                                                    <MenuItem value="Deteriorado">Deteriorado</MenuItem>
                                                    <MenuItem value="Roto">Roto</MenuItem>
                                                </Select>
                                            </FormControl>
                                        </Box>
                                    )}
                                    {filter === "Nombre" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography>🔍 Buscar recurso por nombre:</Typography>
                                            <FormControl sx={{ ml: 2, width: 200 }} variant="outlined">
                                                <Input
                                                    type="text"
                                                    value={selectedResourceName || ''}
                                                    onChange={(e) => {
                                                        const value = e.target.value;
                                                        setSelectedResourceName(value || ''); // Actualiza el estado con el nombre del recurso
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

                                </Box>
                            ))}
                        </Collapse>
                    </Box>
                )}

                {/* Grid de recursos */}
                <Grid2 container spacing={4}>
                    {currentResources.map((resource: Resource) => (
                        <Grid2
                            size={{ xs: 12, sm: 6, md: 4 }}
                            key={resource.id}
                            sx={{
                                display: 'flex',
                                justifyContent: 'center',
                            }}
                        >
                            {/* <ResourceCard resource={resource} /> */}
                            <GenericCard
                                title={resource.name}
                                fields={[
                                    { label: 'Tipo', value: resource.type },
                                    { label: 'Frecuencia de Uso', value: resource.useFrequency },
                                    { label: 'Ubicación', value: resource.facilityLocation },
                                    { label: 'Instalación', value: `${resource.facilityName} (${resource.facilityType})` },
                                ]}
                                badge={{
                                    text: resource.condition,
                                    color:
                                        resource.condition === 'Bueno'
                                            ? '#1976d2'
                                            : resource.condition === 'Deteriorado'
                                                ? '#ffa726'
                                                : '#d32f2f',
                                }}
                                actions={role !== 'Parent' && isAuthenticated
                                    ? [
                                        {
                                            label: 'Definir frecuencia de uso',
                                            onClick:  () => handleDefineUsageFrequency(resource.id),
                                        },
                                    ]
                                    : []}
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
            </Box>
        </LocalizationProvider>
    )
}
export default ResourcesPage;