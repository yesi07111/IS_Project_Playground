import React, { useState, useEffect } from 'react';
import { Box, Container, Paper, TextField, Button, Typography, Alert } from '@mui/material';
import { useNavigate, useLocation } from 'react-router-dom';
import kidsPlay from '../assets/images/decorative/xylophone.png';
import stars from '../assets/images/decorative/toy-train.png';
import { authService } from '../services/authService';
import { useAuth } from '../components/auth/authContext';

const VerifyEmailPage: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const { login } = useAuth();
    const [verificationCode, setVerificationCode] = useState('');
    const [error, setError] = useState<string | null>(null);
    const [fieldErrors, setFieldErrors] = useState<FieldErrors>({
        statusCode: 0,
        message: '',
        errors: {}
    });
    const [success, setSuccess] = useState<string | null>(null);
    const [email, setEmail] = useState<string | null>(null);
    const [isResendDisabled, setIsResendDisabled] = useState(false);
    const [timeLeft, setTimeLeft] = useState<number>(0);

    type FieldErrors = {
        statusCode: number;
        message: string;
        errors: {
            [key: string]: string[];
        };
    };

    useEffect(() => {
        const storedEmail = localStorage.getItem('formData');
        if (storedEmail) {
            const parsedData = JSON.parse(storedEmail);
            setEmail(parsedData.email);
        }
    }, []);

    useEffect(() => {
        const handleRouteChange = () => {
            const currentPage = window.location.pathname;
            const storedData = localStorage.getItem('formData');
            const deleteToken = localStorage.getItem('DeleteToken');

            if (storedData && deleteToken) {
                localStorage.setItem("ToDelete", "true");
            }
            if (currentPage !== '/register' && currentPage !== '/verify-email') {
                clearLocalStorage();
            }
        };

        handleRouteChange();
    }, [location]);

    const clearLocalStorage = async () => {
        await localStorage.removeItem('formData');
        await localStorage.removeItem('pendingVerificationEmail');
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const userName = localStorage.getItem('pendingVerificationEmail');

        if (!userName) {
            setError('No se encontró un usuario para la verificación.');
            return;
        }
        try {
            const response = await authService.verifyEmail(userName, verificationCode);
            if (response.token) {
                localStorage.setItem('token', response.token);
                await clearLocalStorage();
                login();
                navigate('/');
            }
        } catch (err) {
            const apiError = err as FieldErrors;

            if (apiError && apiError.errors) {
                const errorData = apiError.errors;

                setFieldErrors({
                    statusCode: apiError.statusCode || 400,
                    message: apiError.message || 'Ocurrieron errores de validación.',
                    errors: errorData
                });
            } else {
                setError('Error durante la verificación.');
            }
        }
    };

    const handleResendCode = async () => {
        const userName = localStorage.getItem('pendingVerificationEmail');

        if (!userName) {
            setError('No se encontró un usuario para la verificación.');
            return;
        }
        try {
            await authService.resendVerificationCode(userName);
            setSuccess('Código de verificación reenviado con éxito.');
            setIsResendDisabled(true);
            setTimeLeft(180); // 180 seconds = 3 minutes

            const timer = setInterval(() => {
                setTimeLeft((prevTime) => {
                    if (prevTime <= 1) {
                        clearInterval(timer);
                        setIsResendDisabled(false);
                        return 0;
                    }
                    return prevTime - 1;
                });
            }, 1000);
        } catch {
            setError('Error al reenviar el código de verificación.');
        }
    };

    const handleVerificationCodeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const inputValue = e.target.value;
        if (inputValue.length <= 6) { // Limit the length to 6
            setVerificationCode(inputValue);
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
                        Hemos enviado un código de verificación a tu correo electrónico {email}.
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

                    <Box component="form" onSubmit={handleSubmit} noValidate sx={{ width: '100%' }}>
                        {(['code'] as const).map((field) => (
                            <TextField
                                key={field}
                                margin="normal"
                                fullWidth
                                name={field}
                                label={
                                    <span>
                                        {field === 'code' && 'Código de Verificación '}
                                        <span style={{ color: '#ff0000' }}>*</span>
                                    </span>
                                }
                                type="text"
                                id={field}
                                value={verificationCode}
                                onChange={handleVerificationCodeChange}
                                error={!!fieldErrors.errors[field]}
                                helperText={fieldErrors.errors[field]?.join(' ')}
                                sx={{
                                    ...textFieldSx,
                                    '& .MuiInputBase-input': {
                                        textAlign: 'center'
                                    }
                                }}
                                slotProps={{
                                    input: {
                                        style: { textAlign: 'center', textAlignLast: 'center' }
                                    }
                                }}
                            />
                        ))}

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
                                mb: 0.2,
                                '&:hover': {
                                    backgroundColor: 'rgba(25, 118, 210, 0.04)'
                                }
                            }}
                            onClick={handleResendCode}
                            disabled={isResendDisabled}
                        >
                            Reenviar código
                        </Button>

                        {isResendDisabled && (
                            <Typography variant="body2" sx={{ mt: 1, textAlign: 'center' }}>
                                Puedes reenviar el código en {Math.floor(timeLeft / 60)}:{(timeLeft % 60).toString().padStart(2, '0')} minutos.
                            </Typography>
                        )}
                    </Box>
                </Paper>
            </Container>
        </Box>
    );
};

export default VerifyEmailPage;