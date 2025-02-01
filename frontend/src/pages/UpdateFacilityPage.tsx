import React, { useEffect, useState } from 'react';
import {
    Box,
    Container,
    Paper,
    Typography,
    TextField,
    Button,
    Alert,
} from '@mui/material';
import { facilityService } from '../services/facilityService';
import { useParams, useSearchParams } from 'react-router-dom';
import { Facility } from '../interfaces/Facility';
import { FieldErrors } from '../interfaces/Error';

const UpdateFacilityPage: React.FC = () => {
    const [error, setError] = useState('');
    const [fieldErrors, setFieldErrors] = useState<FieldErrors>({
        statusCode: 0,
        message: '',
        errors: {}
    });
    const [success, setSuccess] = useState(false);
    const [searchParams] = useSearchParams();

    // Recuperar los valores de los query params
    const id = searchParams.get('id');
    const name = searchParams.get('name');
    const type = searchParams.get('type');
    const location = searchParams.get('location');
    const maximumCapacity = searchParams.get('maximumCapacity');
    const usagePolicy = searchParams.get('usagePolicy');

    const [formData, setFormData] = useState<Facility>({
        id: id || '',
        name: name || '',
        location: location || '',
        type: type || '',
        usagePolicy: usagePolicy || '',
        maximumCapacity: Number(maximumCapacity) || 0,
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        if (formData) {
            const { name, value } = e.target;
            setFormData({ ...formData, [name]: value });
        }
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setError('');
        setFieldErrors({
            statusCode: 0,
            message: '',
            errors: {}
        });
        setSuccess(false);

        if (formData.name === name && formData.type === type && formData.location === location && formData.usagePolicy === usagePolicy && formData.maximumCapacity === Number(maximumCapacity)) {
            setError('No hay datos para modificar.');
            return;
        }

        try {
            if (id) {
                const response = await facilityService.updateFacility({
                    id: id,
                    name: formData.name,
                    location: formData.location,
                    type: formData.type,
                    usagePolicy: formData.usagePolicy,
                    maximumCapacity: formData.maximumCapacity
                }); // Método para actualizar la instalación
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
                setError('Error al actualizar la instalación. Inténtalo nuevamente.');
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
                <Typography color="error">No se encontraron datos de la instalación.</Typography>
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
                        Modificar Instalación
                    </Typography>

                    {/* Mensaje de éxito */}
                    {success && <Alert severity="success" sx={{ mb: 2 }}>Instalación actualizada con éxito.</Alert>}

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
                            label="Ubicación"
                            name="location"
                            value={formData.location}
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

                        <TextField
                            label="Capacidad Máxima"
                            name="maximumCapacity"
                            type="number"
                            value={formData.maximumCapacity}
                            onChange={handleChange}
                            fullWidth
                            margin="normal"
                            required
                        />

                        <TextField
                            label="Política de Uso"
                            name="usagePolicy"
                            value={formData.usagePolicy}
                            onChange={handleChange}
                            fullWidth
                            margin="normal"
                            multiline
                            rows={4}
                            required
                        />

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

export default UpdateFacilityPage;
