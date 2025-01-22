import React, { useState, useEffect } from 'react';
import { Box, Container, Paper, TextField, Button, Typography, Alert } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import kidsPlay from '/images/decorative/xylophone.png';
import stars from '/images/decorative/toy-train.png';
import { authService } from '../services/authService';
import { useAuth } from '../components/auth/authContext';
import { FieldErrors } from '../interfaces/Error';
import Swal from 'sweetalert2';
import { tokenService } from '../services/tokenService';

/**
 * Componente funcional que representa la página de restablecimiento de contraseña.
 * 
 * Este componente permite a los usuarios restablecer su contraseña ingresando un código
 * de verificación enviado a su dirección de correo. Utiliza elementos de Material-UI como `Box`,
 * `Container`, `Paper`, `TextField`, `Button`, `Typography` y `Alert`.
 * 
 * Funcionalidades principales:
 * - **Manejo de estado**: Utiliza `useState` para manejar el estado del código de verificación,
 *   mensajes de error, mensajes de éxito, correo electrónico del usuario, y el temporizador para
 *   reenviar el código.
 * - **Efectos secundarios**: Usa `useEffect` para cargar el correo electrónico del usuario desde
 *   `localStorage` y manejar cambios de ruta.
 * - **Restablecimiento de contraseña**: Permite al usuario ingresar un código de verificación y manejar
 *   el proceso de restablecimiento a través de `authService`.
 * - **Reenvío de código**: Permite al usuario solicitar un nuevo código de verificación si es necesario,
 *   con un temporizador para evitar múltiples solicitudes en poco tiempo.
 * 
 * @returns {JSX.Element} El componente de la página de restablecimiento de contraseña.
 */
const ResetPasswordPage: React.FC = () => {
    const navigate = useNavigate();
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

    useEffect(() => {
        const storedEmail = localStorage.getItem('pendingVerificationEmail');
        if (storedEmail) {
            setEmail(storedEmail);
            console.log('En ResetPasswordPage, correo electrónico pendiente de verificación:', storedEmail);
        }
    }, []);

    /**
     * Limpia los datos almacenados en `localStorage` relacionados con el restablecimiento de contraseña.
     */
    const clearLocalStorage = async () => {
        localStorage.removeItem('pendingVerificationEmail');
    };

    /**
     * Maneja el envío del formulario de restablecimiento de contraseña.
     * 
     * Verifica el código ingresado por el usuario utilizando el servicio de autenticación.
     * En caso de éxito, inicia sesión y redirige al usuario a la página principal.
     * 
     * @param {React.FormEvent} e - Evento de envío del formulario.
     */
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const storedEmail = localStorage.getItem('pendingVerificationEmail');
        const fullCode = localStorage.getItem('fullCode');
        const newPassword = localStorage.getItem('newPassword');
        localStorage.removeItem('hasCalledEndpoint');

        if (!storedEmail) {
            setError('No se encontró un usuario para la verificación.');
            return;
        }
        if (!fullCode) {
            setError('No se encontró un código de verificación.');
            return;
        }
        if (!newPassword) {
            setError('No se encontró una nueva contraseña.');
            return;
        }
        try {
            console.log('Enviando solicitud de restablecimiento de contraseña...');
            await authService.resetPassword(storedEmail, verificationCode, fullCode, newPassword);
            const loginResponse = await authService.login(storedEmail, newPassword);
            if (loginResponse.token && loginResponse.id && loginResponse.username) {
                await clearLocalStorage();
                localStorage.setItem('authToken', loginResponse.token);
                localStorage.setItem('authId', loginResponse.id);
                localStorage.setItem('authUsername', loginResponse.username);
                await Swal.fire({
                    title: 'Éxito',
                    text: 'Cambio de contraseña exitoso',
                    icon: 'success',
                    confirmButtonText: 'Aceptar'
                });
                login();
                tokenService.startTokenRefreshCheck();
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

            if (!isResendDisabled) {
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
            } else {
                Swal.fire({
                    title: 'Error',
                    text: 'Vuelva a intentar cuando acabe el temporizador.',
                    icon: 'error',
                    confirmButtonText: 'Aceptar'
                });
            }
        }
    };

    /**
     * Maneja el reenvío del código de verificación.
     * 
     * Envía una solicitud para reenviar el código de verificación al correo del usuario.
     * Desactiva el botón de reenvío durante un tiempo determinado para evitar múltiples solicitudes.
     */
    const handleResendCode = async () => {
        const storedEmail = localStorage.getItem('pendingVerificationEmail');

        if (!storedEmail) {
            setError('No se encontró un usuario para la verificación.');
            return;
        }
        try {
            const response = await authService.sendResetPasswordCode(storedEmail);
            localStorage.setItem("fullCode", response.token)
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

    /**
     * Maneja el cambio en el campo del código de verificación.
     * 
     * Limita la longitud del código a 6 caracteres.
     * 
     * @param {React.ChangeEvent<HTMLInputElement>} e - Evento de cambio del input.
     */
    const handleVerificationCodeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const inputValue = e.target.value;
        if (inputValue.length <= 6) { // Limita la longitud a 6
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
                        Restablece tu contraseña
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
                        Por favor, introdúcelo a continuación para restablecer tu contraseña.
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
                            onClick={handleSubmit}
                        >
                            Restablecer
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

export default ResetPasswordPage;