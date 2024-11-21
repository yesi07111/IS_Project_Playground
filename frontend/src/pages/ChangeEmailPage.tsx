import React, { useState } from 'react';
import { Box, Container, Paper, TextField, Button, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { authService } from '../services/authService';

const ChangeEmailPage: React.FC = () => {
    const navigate = useNavigate();
    const [newEmail, setNewEmail] = useState('');
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);

    const handleChangeEmail = async () => {
        try {
            await authService.changeEmail(newEmail);
            setSuccess('Email cambiado exitosamente. Por favor, verifica tu nuevo correo.');
            navigate('/verify-email');
        } catch {
            setError('Error al cambiar el correo electrónico');
        }
    };

    return (
        <Box
            sx={{
                minHeight: '100vh',
                minWidth: '100vw',
                display: 'flex',
                backgroundColor: '#f8f9fa',
                position: 'relative',
                overflow: 'hidden'
            }}
        >
            <Container maxWidth="sm" sx={{
                my: 'auto',
                position: 'relative',
                zIndex: 1
            }}>
                <Paper
                    elevation={3}
                    sx={{
                        p: 4,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        backgroundColor: 'transparent',
                        borderRadius: 2,
                        border: '2px solid rgba(0, 0, 0, 0.2)',
                        transition: 'all 0.3s ease',
                        transform: 'scale(1)',
                        boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
                        background: 'rgba(255, 255, 255, 0)',
                        '&:hover': {
                            transform: 'scale(1.02) translateY(-5px)',
                            boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                            border: '2px solid rgba(0, 0, 0, 0.3)'
                        }
                    }}
                >
                    <Typography
                        component="h1"
                        variant="h4"
                        sx={{
                            color: 'primary.main',
                            fontWeight: 700,
                            mb: 2
                        }}
                    >
                        Cambiar Correo Electrónico
                    </Typography>

                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        name="newEmail"
                        label="Nuevo Correo Electrónico"
                        type="email"
                        id="newEmail"
                        value={newEmail}
                        onChange={(e) => setNewEmail(e.target.value)}
                        sx={{
                            mb: 3,
                            '& .MuiInputBase-root': {
                                backgroundColor: 'rgba(255, 255, 255, 0.7)'
                            }
                        }}
                    />

                    {error && (
                        <Typography color="error" sx={{ mb: 2 }}>
                            {error}
                        </Typography>
                    )}

                    {success && (
                        <Typography color="success" sx={{ mb: 2 }}>
                            {success}
                        </Typography>
                    )}

                    <Button
                        fullWidth
                        variant="contained"
                        sx={{
                            mt: 2,
                            mb: 2,
                            py: 1.5,
                            backgroundColor: '#1976d2',
                            '&:hover': {
                                backgroundColor: '#1565c0'
                            }
                        }}
                        onClick={handleChangeEmail}
                    >
                        Cambiar Correo
                    </Button>
                </Paper>
            </Container>
        </Box>
    );
};

export default ChangeEmailPage;