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
} from '@mui/material';
import { facilityService } from '../services/facilityService';
import { useParams } from 'react-router-dom';
import { Facility } from '../interfaces/Facility';

const FacilityFormPage: React.FC = () => {
    const [success, setSuccess] = useState(false);
    const [error, setError] = useState('');
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
        setSuccess(false);

        if (formData.name.length < 3) {
            setError('El nombre debe tener entre 3 y 15 caracteres.');
            return;
        }
        if (formData.location.length < 3) {
            setError('La ubicación debe tener entre 3 y 15 caracteres.');
            return;
        }
        if (formData.type.length < 3) {
            setError('El tipo debe tener entre 3 y 15 caracteres.');
            return;
        }
        if (formData.usagePolicy.length < 5) {
            setError('Las políticas de uso deben tener entre 5 y 15 caracteres.');
            return;
        }
        if (formData.maximumCapacity <= 0) {
            setError('La capacidad máxima debe ser mayor a cero.');
            return;
        }

        try {
            const response = await facilityService.createFacility(formData);
            setSuccess(true);
        } catch (error) {
            setError('Error al crear la instalación. Por favor, inténtalo nuevamente.');
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
                    {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                    {success && <Alert severity="success" sx={{ mb: 2 }}>Instalación creada con éxito.</Alert>}

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