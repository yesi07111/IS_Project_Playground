import React from 'react';
import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import { Link } from 'react-router-dom';

const Navbar: React.FC = () => {
    return (
        <AppBar
            position="sticky"
            sx={{
                top: 0,
                width: '100%',
                zIndex: 1100,
                backgroundColor: 'primary.main',
                '& .MuiToolbar-root': {
                    width: '100%',
                    maxWidth: '100%',
                    justifyContent: 'space-between',
                    padding: { xs: '8px 16px', sm: '8px 24px' }, // Reducido el padding vertical
                    minHeight: '48px', // Reducida la altura mínima (por defecto es 64px)
                }
            }}
        >
            <Toolbar
                variant="dense" // Esto hace que la toolbar sea más compacta
                sx={{
                    minHeight: '48px !important', // Forzar la altura mínima
                }}
            >
                <Typography
                    variant="h6"
                    component={Link}
                    to="/"
                    sx={{
                        textDecoration: 'none',
                        color: 'inherit',
                        flexGrow: 0,
                        fontSize: '1.1rem', // Reducido ligeramente el tamaño del texto
                    }}
                >
                    KidsParks
                </Typography>
                <Box sx={{
                    display: 'flex',
                    gap: { xs: 1, sm: 2 }
                }}>
                    <Button
                        color="inherit"
                        component={Link}
                        to="/"
                        sx={{
                            fontWeight: 500,
                            py: 0.5, // Reducido el padding vertical del botón
                        }}
                    >
                        Inicio
                    </Button>
                    <Button
                        color="inherit"
                        component={Link}
                        to="/actividades"
                        sx={{
                            fontWeight: 500,
                            py: 0.5, // Reducido el padding vertical del botón
                        }}
                    >
                        Actividades
                    </Button>
                    <Button
                        color="inherit"
                        component={Link}
                        to="/reservas"
                        sx={{
                            fontWeight: 500,
                            py: 0.5, // Reducido el padding vertical del botón
                        }}
                    >
                        Reservas
                    </Button>
                    <Button
                        color="inherit"
                        component={Link}
                        to="/login"
                        sx={{
                            fontWeight: 500,
                            py: 0.5, // Reducido el padding vertical del botón
                        }}
                    >
                        Iniciar Sesión
                    </Button>
                </Box>
            </Toolbar>
        </AppBar>
    );
};

export default Navbar;