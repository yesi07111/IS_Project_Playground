import React, { useState } from 'react';
import { Box, Typography, Button } from '@mui/material';

const UserManagementPage = () => {
    const [users, setUsers] = useState({
        padres: Array.from({ length: 10 }, (_, index) => `Padre ${index + 1}`),
        educators: Array.from({ length: 10 }, (_, index) => `Educador ${index + 1}`),
        admins: Array.from({ length: 10 }, (_, index) => `Administrador ${index + 1}`),
    });

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

            {/* User Management Sections */}
            <Box sx={{ display: 'flex', justifyContent: 'space-around', width: '100%', mb: 4 }}>
                {['padres', 'educators', 'admins'].map((category) => (
                    <Box
                        key={category}
                        sx={{
                            width: '30%',
                            backgroundColor: '#F0F4F8',
                            padding: 2,
                            borderRadius: 1,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            maxHeight: '400px',
                            overflowY: 'auto', // Permite el desplazamiento
                        }}
                    >
                        <Typography variant="h5" sx={{ mb: 2 }}>
                            {category === 'padres'
                                ? 'Padres'
                                : category === 'educators'
                                ? 'Educadores'
                                : 'Administradores'}
                        </Typography>

                        {/* User List with Scroll */}
                        <Box sx={{ width: '100%', mb: 2 }}>
                            {users[category].map((user) => (
                                <Box key={user} sx={{ mb: 1 }}>
                                    <Typography variant="body1">{user}</Typography>
                                </Box>
                            ))}
                        </Box>
                    </Box>
                ))}
            </Box>

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
