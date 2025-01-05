import React from 'react';
import { Container, Typography, Box, Button } from '@mui/material';
import Grid from '@mui/material/Grid2';
import { Link } from 'react-router-dom';

const activities = [
    { id: 1, name: 'Pintura', description: 'Aprende técnicas de pintura.', color: '#FF6B6B' },
    { id: 2, name: 'Fútbol', description: 'Juegos deportivos y actividades físicas.', color: '#4ECDC4' },
    { id: 3, name: 'Ciencias', description: 'Descubre experimentos científicos.', color: '#FFD93D' },
];

const ActivityManagerPage = () => {
    return (
        <Box
            sx={{
                width: '100vw',
                minHeight: '100vh',
                backgroundColor: 'white',
                padding: 4,
            }}
        >
            {/* Header Section */}
            <Typography
                variant="h3"
                sx={{ textAlign: 'center', fontWeight: 700, mb: 6, color: '#2C3E50' }}
            >
                Lista de Actividades
            </Typography>

            {/* Activities Section */}
            <Container maxWidth={false} sx={{ px: { xs: 2, sm: 4, md: 6 } }}>
                <Grid container spacing={4}>
                    {activities.map((activity) => (
                        <Grid size={{ xs: 12, sm: 6, md: 4 }} key={activity.id}>
                            <Box
                                sx={{
                                    padding: 3,
                                    textAlign: 'center',
                                    backgroundColor: activity.color,
                                    borderRadius: 2,
                                    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
                                    color: 'white',
                                    '&:hover': {
                                        transform: 'scale(1.05)',
                                        transition: 'transform 0.3s ease-in-out',
                                    },
                                }}
                            >
                                <Typography variant="h5" fontWeight={700} mb={1}>
                                    {activity.name}
                                </Typography>
                                <Typography variant="body1" mb={2}>
                                    {activity.description}
                                </Typography>
                                <Button
                                    variant="contained"
                                    sx={{ backgroundColor: '#ffffff', color: activity.color, fontWeight: 700 }}
                                >
                                    Modificar
                                </Button>
                            </Box>
                        </Grid>
                    ))}

                    {/* Add Activity Block */}
                    <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                        <Box
                            sx={{
                                padding: 3,
                                textAlign: 'center',
                                backgroundColor: '#E0E0E0',
                                borderRadius: 2,
                                border: '2px dashed #BDBDBD',
                                color: '#757575',
                                '&:hover': {
                                    transform: 'scale(1.05)',
                                    transition: 'transform 0.3s ease-in-out',
                                },
                                height: '100%',
                                display: 'flex',
                                flexDirection: 'column',
                                justifyContent: 'center',
                                alignItems: 'center',
                            }}
                        >
                            <Typography variant="h4" fontWeight={700} mb={1}>
                                +
                            </Typography>
                            <Typography variant="body1" mb={2}>
                                Añadir Actividad
                            </Typography>
                        </Box>
                    </Grid>
                </Grid>
            </Container>
        </Box>
    );
};

export default ActivityManagerPage;

