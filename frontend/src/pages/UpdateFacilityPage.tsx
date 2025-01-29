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

const UpdateFacilityPage: React.FC = () => {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState(false);
    const [searchParams] = useSearchParams();

    // Recuperar los valores de los query params
    const id = searchParams.get('id');
    const name = searchParams.get('name');
    const type = searchParams.get('type');
    const location = searchParams.get('location');
    const maximumCapacity = searchParams.get('maximumCapacity');
    const usagePolicy = searchParams.get('usagePolicy');
    
    const [formData, setFormData] = useState<Facility | null>({
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
        setSuccess(false);

        if (!formData) {
            setError('No hay datos para modificar.');
            return;
        }

        // Validaciones (puedes ajustarlas según tus necesidades)
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
        } catch (error) {
            setError('Error al actualizar la instalación. Inténtalo nuevamente.');
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
                    {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                    {success && <Alert severity="success" sx={{ mb: 2 }}>Instalación actualizada con éxito.</Alert>}

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
