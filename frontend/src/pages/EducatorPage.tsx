import React from 'react';
import { Container, Typography, Box, Button } from '@mui/material';
import Grid from '@mui/material/Grid2';
import { Link } from 'react-router-dom';

const EducatorPage = () => {
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
                Bienvenido Profesor
            </Typography>

            {/* Navigation Buttons */}
            <Box
                sx={{
                    display: 'flex',
                    justifyContent: 'center',
                    gap: 4,
                    mt: 6,
                }}
            >
                <Button
                    variant="contained"
                    size="large"
                    component={Link}
                    to="/activity-manager"
                    sx={{
                        backgroundColor: '#FFD93D',
                        fontSize: '1rem',
                        py: 2,
                        px: 4,
                        '&:hover': {
                            backgroundColor: '#e6c332',
                        },
                    }}
                >
                    Gestionar Actividades
                </Button>

                <Button
                    variant="contained"
                    size="large"
                    component={Link}
                    to="/statistics"
                    sx={{
                        backgroundColor: '#4ECDC4',
                        fontSize: '1rem',
                        py: 2,
                        px: 4,
                        '&:hover': {
                            backgroundColor: '#3bb3a2',
                        },
                    }}
                >
                    Ver Estadísticas
                </Button>

                <Button
                    variant="contained"
                    size="large"
                    component={Link}
                    to="/resources"
                    sx={{
                        backgroundColor: '#FF6B6B',
                        fontSize: '1rem',
                        py: 2,
                        px: 4,
                        '&:hover': {
                            backgroundColor: '#ff5252',
                        },
                    }}
                >
                    Ver Recursos
                </Button>
            </Box>
        </Box>
    );
};

export default EducatorPage;
