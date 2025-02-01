import React, { useCallback, useEffect, useState } from 'react';
import {
    Box,
    Container,
    Paper,
    Typography,
    TextField,
    Button,
    MenuItem,
    Alert,
    SelectChangeEvent,
    FormControl,
    InputLabel,
    Select,
} from '@mui/material';
import { resourceService } from '../services/resourceService';
import { facilityService } from '../services/facilityService';
import { FacilityResponse } from '../interfaces/Facility';
import { cacheService } from '../services/cacheService';
import { FieldErrors } from '../interfaces/Error';

const ResourceFormPage: React.FC = () => {
    const [success, setSuccess] = useState(false);
    const [error, setError] = useState('');
    const [fieldErrors, setFieldErrors] = useState<FieldErrors>({
        statusCode: 0,
        message: '',
        errors: {}
    });
    const [facilityNames, setFacilityNames] = useState<string[]>([]);
    const [facilities, setFacilities] = useState<FacilityResponse[]>([]);
    const [formData, setFormData] = useState({
        name: '',
        type: '',
        resourceCondition: '',
        facilityName: '',
        facilityId: '',
    });

    const fetchAllFacilityNames = useCallback(async () => {
        try {
            const response = await facilityService.getAllFacilities({ useCase: 'AdminEducatorView' });
            const facilityArray: FacilityResponse[] = Array.isArray(response.result) ? response.result as unknown as FacilityResponse[] : Array.from(response.result);
            const namesArray: string[] = facilityArray.map(facility => facility.name);
            setFacilityNames(namesArray);
            setFacilities(facilityArray);
            cacheService.saveFacilityNames(namesArray);
        } catch (error) {
            console.error('Error fetching facility types:', error);
            const cachedFacilityNames = cacheService.loadFacilityNames();
            setFacilityNames(cachedFacilityNames);
        }
    }, []);

    const fetchInitialData = useCallback(async () => {
        await Promise.all([fetchAllFacilityNames()]);
    }, [fetchAllFacilityNames]);

    useEffect(() => {
        fetchInitialData();
    }, []);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement> | SelectChangeEvent<string>) => {
        const { name, value, type, checked } = e.target as HTMLInputElement;
        setFormData({ ...formData, [name || '']: type === 'checkbox' ? checked : value, });
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        let selectedName = '';
        let facilityId = '';
        let facility = null;

        e.preventDefault();
        setError('');
        setFieldErrors({
            statusCode: 0,
            message: '',
            errors: {}
        });
        setSuccess(false);

        selectedName = formData.facilityName;
        facility = facilities.find(facility => facility.name === selectedName);
        facilityId = facility ? facility.id : '';
        formData.facilityId = facilityId;

        console.log('Datos enviados:', formData);

        try {
            const response = await resourceService.createResource({
                name: formData.name,
                type: formData.type,
                resourceCondition: formData.resourceCondition,
                facilityId: formData.facilityId,
            });
            setSuccess(true);
        } catch (err: any) {
            const apiError = err as FieldErrors;

            if (apiError && apiError.errors) {
                const errorData = apiError.errors;

                setFieldErrors({
                    statusCode: apiError.statusCode || 400,
                    message: apiError.message || 'Ocurrieron errores de validación.',
                    errors: errorData
                });
            }
            else {
                setError('Error al crear el recurso. Por favor, inténtalo nuevamente.');
            }
        }
    };

    return (
        <Box
            sx={{
                minHeight: '100vh',
                minWidth: '100vw',
                display: 'flex',
                backgroundColor: '#f8f9fa',
                justifyContent: 'center',
                alignItems: 'center',
            }}
        >
            <Container maxWidth="sm">
                <Paper elevation={3} sx={{ p: 4 }}>
                    <Typography variant="h4" component="h1" gutterBottom>
                        Crear Recurso
                    </Typography>

                    {/* Mensaje de éxito */}
                    {success && <Alert severity="success" sx={{ mb: 2 }}>Recurso creado con éxito.</Alert>}

                    {/* Mensaje de error general */}
                    {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

                    {/* Errores de validación específicos de la API */}
                    {Object.keys(fieldErrors.errors).map((field) => (
                        <Alert severity="error" key={field} sx={{ mb: 2 }}>
                            {`${fieldErrors.errors[field].join(', ')}`}
                        </Alert>
                    ))}

                    <Box component="form" onSubmit={handleSubmit}>
                        <TextField
                            label="Nombre"
                            name="name"
                            value={formData.name}
                            onChange={handleChange}
                            fullWidth
                            margin="normal"
                            required
                        />

                        <TextField
                            label="Tipo"
                            name="type"
                            value={formData.type}
                            onChange={handleChange}
                            fullWidth
                            margin="normal"
                            required
                        />

                        <FormControl fullWidth margin="normal">
                            <InputLabel id="resource-condition-label">Estado del recurso</InputLabel>
                            <Select
                                labelId="resource-condition-label"
                                name="resourceCondition"
                                value={formData.resourceCondition}
                                onChange={handleChange}
                                label="Estado del recurso"
                                required
                            >
                                <MenuItem value="Bueno">Bueno</MenuItem>
                                <MenuItem value="Deteriorado">Deteriorado</MenuItem>
                                <MenuItem value="Roto">Roto</MenuItem>
                            </Select>
                        </FormControl>

                        <FormControl fullWidth margin="normal">
                            <InputLabel id="facility-name-select-label"> Instalación</InputLabel>
                            <Select
                                labelId="facility-name-select-label"
                                name="facilityName"
                                value={formData.facilityName}
                                onChange={handleChange}
                                label="Instalación"
                                required
                                renderValue={(selected) => selected}
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
                                {facilityNames.map((typeOption) => (
                                    <MenuItem key={typeOption} value={typeOption}>
                                        {typeOption}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>

                        <Button
                            type="submit"
                            variant="contained"
                            color="primary"
                            fullWidth
                            sx={{ mt: 3 }}
                        >
                            Crear Recurso
                        </Button>
                    </Box>
                </Paper>
            </Container>
        </Box>
    );
};

export default ResourceFormPage;