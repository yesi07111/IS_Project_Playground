import React, { useState } from 'react';
import {
    Box,
    Container,
    Paper,
    Typography,
    TextField,
    Button,
    MenuItem,
    Alert,
} from '@mui/material';
import { facilityService } from '../services/facilityService';
import { FieldErrors } from '../interfaces/Error';

const FacilityFormPage: React.FC = () => {
    const [success, setSuccess] = useState(false);
    const [error, setError] = useState('');
    const [fieldErrors, setFieldErrors] = useState<FieldErrors>({
        statusCode: 0,
        message: '',
        errors: {}
    });
    const [formData, setFormData] = useState({
        name: '',
        location: '',
        type: '',
        usagePolicy: '',
        maximumCapacity: 1,
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
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

        try {
            const response = await facilityService.createFacility(formData);
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
                setError('Error al crear la instalación. Por favor, inténtalo nuevamente.');
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
                        Crear Instalación
                    </Typography>

                    {/* Mensaje de éxito */}
                    {success && <Alert severity="success" sx={{ mb: 2 }}>Instalación creada con éxito.</Alert>}

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
                            Crear Instalación
                        </Button>
                    </Box>
                </Paper>
            </Container>
        </Box>
    );
};

export default FacilityFormPage;