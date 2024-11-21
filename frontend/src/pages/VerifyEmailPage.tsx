import React, { useState } from 'react';
import {
    Box,
    Container,
    Paper,
    TextField,
    Button,
    Typography,
    Alert,
    IconButton,
} from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import kidsPlay from '../assets/images/decorative/xylophone.png';
import stars from '../assets/images/decorative/toy-train.png';
import { authService } from '../services/authService';
import Swal from 'sweetalert2';

const VerifyEmailPage: React.FC = () => {
    const navigate = useNavigate();
    const [verificationCode, setVerificationCode] = useState('');
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const userName = localStorage.getItem('pendingVerificationEmail');

        if (!userName) {
            setError('No user found for verification');
            return;
        }
        try {
            const response = await authService.verifyEmail(userName, verificationCode);
            if (response.token) {
                localStorage.setItem('token', response.token);
                localStorage.removeItem('pendingVerificationEmail');
                navigate('/');
            }
        } catch {
            setError(error);
        }
    };

    const handleResendCode = async () => {
        const email = localStorage.getItem('pendingVerificationEmail');

        if (!email) {
            setError('No email found for verification');
            return;
        }

        try {
            await authService.resendVerificationCode(email);
            setSuccess('Verification code resent successfully');
        } catch {
            setError('Error resending verification code');
        }
    };

    const handleBackClick = () => {
        Swal.fire({
            title: 'Confirmación de regreso',
            text: 'Si no verifica su correo, su acceso será limitado. Tiene hasta hoy para confirmar su correo. ¿Desea regresar a la página principal?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                navigate('/');
            }
        });
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
            {/* Botón de regresar */}
            <IconButton
                sx={{
                    position: 'absolute',
                    top: 16,
                    left: 16,
                    zIndex: 2,
                    '&:focus': {
                        outline: 'none'
                    }
                }}
                onClick={handleBackClick}
            >
                <ArrowBackIcon />
            </IconButton>

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

                    {error && (
                        <Alert severity="error" sx={{ width: '100%', mb: 3 }}>
                            {error}
                        </Alert>
                    )}

                    {success && (
                        <Alert severity="success" sx={{ width: '100%', mb: 3 }}>
                            {success}
                        </Alert>
                    )}

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
                                mb: 0.5, // Ajustado para reducir el espacio
                                '&:hover': {
                                    backgroundColor: 'rgba(25, 118, 210, 0.04)'
                                }
                            }}
                            onClick={handleResendCode}
                        >
                            Reenviar código
                        </Button>

                        <Typography
                            variant="body2"
                            sx={{
                                mt: 0.5, // Ajustado para reducir el espacio
                                textAlign: 'center',
                                color: 'text.secondary'
                            }}
                        >
                            ¿No es tu correo? <Button component={Link} to="/change-email" color="primary" sx={{ textTransform: 'none' }}>Cámbialo aquí</Button>.
                        </Typography>
                    </Box>
                </Paper>
            </Container>
        </Box>
    );
};

export default VerifyEmailPage;