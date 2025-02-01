import React, { useCallback, useEffect, useState } from 'react';
import {
    Box,
    Container,
    Paper,
    Typography,
    TextField,
    Button,
    Alert,
    SelectChangeEvent,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
} from '@mui/material';
import { facilityService } from '../services/facilityService';
import { useParams, useSearchParams } from 'react-router-dom';
import { Facility, FacilityResponse } from '../interfaces/Facility';
import { Resource } from '../interfaces/Resource';
import { cacheService } from '../services/cacheService';
import { resourceService } from '../services/resourceService';
import { FieldErrors } from '../interfaces/Error';

const UpdateResourcePage: React.FC = () => {
    const [error, setError] = useState('');
    const [fieldErrors, setFieldErrors] = useState<FieldErrors>({
        statusCode: 0,
        message: '',
        errors: {}
    });
    const [success, setSuccess] = useState(false);
    const [searchParams] = useSearchParams();
    const [facilityNames, setFacilityNames] = useState<string[]>([]);
    const [facilities, setFacilities] = useState<FacilityResponse[]>([]);

    // Recuperar los valores de los query params
    const id = searchParams.get('id');
    const name = searchParams.get('name');
    const type = searchParams.get('type');
    const resourceCondition = searchParams.get('resourceCondition');
    const facilityName = searchParams.get('facilityName');

    const [formData, setFormData] = useState({
        id: id || '',
        name: name || '',
        type: type || '',
        condition: resourceCondition || '',
        facilityName: facilityName || '',
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
        if (formData) {
            const { name, value, type, checked } = e.target as HTMLInputElement;
            setFormData({ ...formData, [name || '']: type === 'checkbox' ? checked : value, });
        }
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

        if (formData.name === name && formData.type === type && formData.condition === resourceCondition && formData.facilityName === facilityName) {
            setError('No hay datos para modificar.');
            return;
        }

        selectedName = formData.facilityName;
        facility = facilities.find(facility => facility.name === selectedName);
        facilityId = facility ? facility.id : '';

        console.log('Datos enviados:', formData);

        try {
            if (id) {
                const response = await resourceService.updateResource({
                    id: id,
                    name: formData.name,
                    type: formData.type,
                    resourceCondition: formData.condition,
                    facilityId: facilityId
                });
                setSuccess(true);
            }
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
                setError('Error al actualizar el recurso. Inténtalo nuevamente.');
            }
        }
    };

    if (!formData) {
        return (
            <Box
                sx={{
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    minHeight: '100vh',
                }}
            >
                <Typography color="error">No se encontraron datos del recurso.</Typography>
            </Box>
        );
    }

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
                        Modificar Recurso
                    </Typography>

                    {/* Mensaje de éxito */}
                    {success && <Alert severity="success" sx={{ mb: 2 }}>Recurso actualizado con éxito.</Alert>}

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
                        />

                        <TextField
                            label="Tipo"
                            name="type"
                            value={formData.type}
                            onChange={handleChange}
                            fullWidth
                            margin="normal"
                        />

                        <FormControl fullWidth margin="normal">
                            <InputLabel id="resource-condition-label">Estado del recurso</InputLabel>
                            <Select
                                labelId="resource-condition-label"
                                name="condition"
                                value={formData.condition}
                                onChange={handleChange}
                                label="Estado del recurso"
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
                            Guardar Cambios
                        </Button>
                    </Box>
                </Paper>
            </Container>
        </Box>
    );
};

export default UpdateResourcePage;