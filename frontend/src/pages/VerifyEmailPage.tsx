import React, { useState } from 'react';
import {
    Box,
    Container,
    Paper,
    TextField,
    Button,
    Typography
} from '@mui/material';
import { useNavigate } from 'react-router-dom';
import kidsPlay from '../assets/images/decorative/xylophone.png';
import stars from '../assets/images/decorative/toy-train.png';
import { authService } from '../services/authService';

const VerifyEmailPage: React.FC = () => {
    const navigate = useNavigate();
    const [verificationCode, setVerificationCode] = useState('');
    const [error, setError] = useState('');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const email = localStorage.getItem('pendingVerificationEmail');

        if (!email) {
            setError('No email found for verification');
            return;
        }

        try {
            const response = await authService.verifyEmail(email, verificationCode);
            // Si la verificación es exitosa, el backend devolverá un token JWT
            if (response.token) {
                localStorage.setItem('token', response.token);
                localStorage.removeItem('pendingVerificationEmail');
                navigate('/');
            }
        } catch (error) {
            setError('Invalid verification code');
        }
    };

    const textFieldSx = {
        mb: 3,
        '& .MuiInputBase-root': {
            backgroundColor: 'rgba(255, 255, 255, 0.7)'
        },
        '& .MuiFormLabel-root': {
            '& .MuiFormLabel-asterisk': {
                color: '#ff0000'
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
                position: 'relative',
                overflow: 'hidden'
            }}
        >
            {/* Decoración superior derecha */}
            <Box
                component="img"
                src={kidsPlay}
                sx={{
                    position: 'absolute',
                    right: '10%',
                    top: '20%',
                    opacity: 0.1,
                    width: '300px',
                    pointerEvents: 'none',
                    zIndex: 0
                }}
            />

            {/* Decoración centro-izquierda */}
            <Box
                component="img"
                src={stars}
                sx={{
                    position: 'absolute',
                    left: '10%',
                    bottom: '10%',
                    opacity: 0.1,
                    width: '400px',
                    pointerEvents: 'none',
                    zIndex: 0
                }}
            />

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
                        backdropFilter: 'none', // Eliminado el blur
                        borderRadius: 2,
                        border: '2px solid rgba(0, 0, 0, 0.2)',
                        transition: 'all 0.3s ease',
                        transform: 'scale(1)',
                        boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
                        background: 'rgba(255, 255, 255, 0)', // Fondo completamente transparente
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
                        Verifica tu correo
                    </Typography>

                    <Typography
                        variant="body1"
                        sx={{
                            mb: 4,
                            textAlign: 'center',
                            color: 'text.secondary'
                        }}
                    >
                        Hemos enviado un código de verificación a tu correo electrónico.
                        Por favor, introdúcelo a continuación para completar tu registro.
                    </Typography>

                    <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%' }}>
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="verificationCode"
                            label="Código de Verificación"
                            type="text"
                            id="verificationCode"
                            value={verificationCode}
                            onChange={(e) => setVerificationCode(e.target.value)}
                            sx={textFieldSx}
                            inputProps={{
                                maxLength: 6,
                                style: { letterSpacing: '0.5em', textAlign: 'center' }
                            }}
                        />

                        <Button
                            type="submit"
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
                        >
                            Verificar
                        </Button>

                        <Button
                            fullWidth
                            color="primary"
                            sx={{
                                mt: 1,
                                '&:hover': {
                                    backgroundColor: 'rgba(25, 118, 210, 0.04)'
                                }
                            }}
                            onClick={() => {/* Lógica para reenviar el código */ }}
                        >
                            Reenviar código
                        </Button>
                    </Box>
                </Paper>
            </Container>
        </Box>
    );
};

export default VerifyEmailPage;