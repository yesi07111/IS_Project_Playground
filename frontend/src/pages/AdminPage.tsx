import React, { useState } from 'react';
import { Box, Typography, Button, Menu, MenuItem } from '@mui/material';
import { Link } from 'react-router-dom';

const AdminPage = () => {
    const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
    const handleMenuOpen = (event: React.MouseEvent<HTMLButtonElement>) => setAnchorEl(event.currentTarget);
    const handleMenuClose = () => setAnchorEl(null);

    return (
        <Box
            sx={{
                width: '100vw',
                minHeight: '100vh',
                backgroundColor: 'white',
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                padding: 4,
                position: 'relative',
            }}
        >
            {/* Header Section */}
            <Typography
                variant="h3"
                sx={{ textAlign: 'center', fontWeight: 700, mb: 6, color: '#2C3E50' }}
            >
                Panel de Administración
            </Typography>

            {/* Footer Navigation Bar */}
            <Box
                sx={{
                    width: '100%',
                    backgroundColor: '#D0EFFF',
                    padding: 2,
                    display: 'flex',
                    justifyContent: 'space-around',
                    alignItems: 'center',
                    position: 'fixed', // Fija la barra al borde de la ventana
                    bottom: 0,
                    left: 0,
                    boxShadow: '0 -2px 5px rgba(0,0,0,0.1)',
                }}
            >
                <Button
                    variant="text"
                    component={Link}
                    to="/user-manager"
                    sx={{
                        color: '#2C3E50',
                        '&:hover': { textDecoration: 'underline' },
                    }}
                >
                    Usuarios
                </Button>

                <Button
                    variant="text"
                    component={Link}
                    to="/activities-manager"
                    sx={{
                        color: '#2C3E50',
                        '&:hover': { textDecoration: 'underline' },
                    }}
                >
                    Actividades
                </Button>

                <Button
                    variant="text"
                    component={Link}
                    to="/resources-manager"
                    sx={{
                        color: '#2C3E50',
                        '&:hover': { textDecoration: 'underline' },
                    }}
                >
                    Recursos
                </Button>

                <Button
                    variant="text"
                    component={Link}
                    to="/facilities-manager"
                    sx={{
                        color: '#2C3E50',
                        '&:hover': { textDecoration: 'underline' },
                    }}
                >
                    Instalaciones
                </Button>

                {/* Solicitudes Menu */}
                <Box sx={{ position: 'relative' }}>
                    <Button
                        variant="text"
                        onClick={handleMenuOpen}
                        sx={{
                            color: '#2C3E50',
                            '&:hover': { textDecoration: 'underline' },
                        }}
                    >
                        Solicitudes
                    </Button>
                    <Menu
                        anchorEl={anchorEl}
                        open={Boolean(anchorEl)}
                        onClose={handleMenuClose}
                        disableScrollLock
                        anchorOrigin={{
                            vertical: 'top',
                            horizontal: 'center',
                        }}
                        transformOrigin={{
                            vertical: 'bottom',
                            horizontal: 'center',
                        }}
                    >
                        <MenuItem component={Link} to="/reservations-request-manager" onClick={handleMenuClose}>
                            Reservas
                        </MenuItem>
                        <MenuItem component={Link} to="/activities-request-manager" onClick={handleMenuClose}>
                            Actividades
                        </MenuItem>
                    </Menu>
                </Box>
            </Box>
        </Box>
    );
};

export default AdminPage;
