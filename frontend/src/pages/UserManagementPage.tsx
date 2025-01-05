import React, { useState } from 'react';
import { Box, Typography, Button } from '@mui/material';
import { Link } from 'react-router-dom';

const UserManagementPage = () => {
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
            {/* Title Section */}
            <Typography
                variant="h3"
                sx={{ textAlign: 'center', fontWeight: 700, mb: 6, color: '#2C3E50' }}
            >
                Panel de Gestión de Usuario
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
                    position: 'absolute',
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
                    variant="contained"
                    color="primary"
                    component={Link}
                    to="/admin"
                    sx={{
                        padding: 1,
                        fontSize: 14,
                        fontWeight: 'bold',
                        backgroundColor: '#3498DB',
                    }}
                >
                    Administrador
                </Button>
                <Typography variant="body2" sx={{ color: '#2C3E50' }}>
                    Fecha: {new Date().toLocaleDateString()}
                </Typography>
            </Box>
        </Box>
    );
};

export default UserManagementPage;
