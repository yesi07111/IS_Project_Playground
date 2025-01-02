import React, { useState } from 'react';
import {
    Box,
    Container,
    Paper,
    TextField,
    Button,
    Typography,
    Link as MuiLink,
    InputAdornment,
    IconButton,
    Alert
} from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import kidsBalloons from '/images/decorative/learning.png';
import kidsStudying from '/images/decorative/tree-house.png';
import { useAuth } from '../components/auth/authContext';
import { authService } from '../services/authService';
import { FieldGeneralErrors } from '../types/FieldGeneralErrors'

/**
 * Componente funcional que representa la página de inicio de sesión.
 * 
 * Este componente utiliza varios elementos de Material-UI para estructurar
 * la página, incluyendo `Box`, `Container`, `Paper`, `TextField`, `Button`,
 * `Typography`, `Link`, `InputAdornment`, `IconButton`, y `Alert`.
 * 
 * Funcionalidades principales:
 * - **Manejo de estado**: Utiliza `useState` para manejar el estado del formulario,
 *   errores de campo, visibilidad de la contraseña y mensajes de error.
 * - **Autenticación**: Usa el contexto de autenticación `useAuth` para manejar
 *   el inicio de sesión y la navegación posterior.
 * - **Formulario de inicio de sesión**: Permite al usuario ingresar su identificador
 *   y contraseña, con validación de errores y manejo de visibilidad de la contraseña.
 * - **Navegación**: Utiliza `useNavigate` para redirigir al usuario tras un inicio
 *   de sesión exitoso.
 * 
 * @returns {JSX.Element} El componente de la página de inicio de sesión.
 */
const LoginPage: React.FC = () => {
    const [showPassword, setShowPassword] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();
    const [error, setError] = useState('');
    const [formData, setFormData] = useState({
        identifier: '',
        password: ''
    });

    const [fieldGeneralErrors, setFieldGeneralErrors] = useState<FieldGeneralErrors>({
        statusCode: 0,
        message: '',
        errors: {
            generalErrors: []
        }
    });

    /**
     * Maneja los cambios en los campos del formulario de inicio de sesión.
     * 
     * @param {React.ChangeEvent<HTMLInputElement>} e - Evento de cambio del input.
     */
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    /**
 * Maneja el cambio a la página de registro.
 */
    const handleRegister = () => {
        navigate('/register')
    }

    /**
     * Maneja el envío del formulario de inicio de sesión.
     * 
     * Realiza la autenticación del usuario utilizando el servicio de autenticación.
     * En caso de éxito, almacena el token de autenticación y redirige al usuario.
     * En caso de error, muestra mensajes de error apropiados.
     * 
     * @param {React.FormEvent} e - Evento de envío del formulario.
     */
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setFieldGeneralErrors({
            statusCode: 0,
            message: '',
            errors: {
                generalErrors: []
            }
        });

        try {
            const response = await authService.login(
                formData.identifier,
                formData.password
            );
            if (response) {
                localStorage.setItem('authToken', response.token);
                localStorage.setItem('authId', response.id);
                localStorage.setItem('authUserName', response.username);
                login()
                navigate('/');
            }

        } catch (error) {
            const apiError = error as FieldGeneralErrors;
            if (apiError && apiError.errors) {
                setFieldGeneralErrors({
                    statusCode: apiError.statusCode || 400,
                    message: apiError.message || 'Ocurrieron errores de validación.',
                    errors: apiError.errors
                });
            } else {
                setError('Hubo un error durante el inicio de sesión. Por favor, inténtalo de nuevo.');
            }
        }
    };

    return (
        <Box
            sx={{
                minHeight: '100vh',
                minWidth: '100vw',
                width: '100%',
                height: '100%',
                display: 'flex',
                backgroundColor: '#f8f9fa',
                position: 'relative',
                overflow: 'auto',
                margin: 0,
                padding: 0,
                boxSizing: 'border-box',
            }}
        >
            <Box
                component="img"
                src={kidsBalloons}
                sx={{
                    position: 'absolute',
                    right: -50,
                    bottom: 0,
                    opacity: 0.1,
                    width: '400px',
                    pointerEvents: 'none',
                    transform: 'translate(-50px, -50px)'
                }}
            />

            <Box
                component="img"
                src={kidsStudying}
                sx={{
                    position: 'absolute',
                    left: '5%',
                    top: '50%',
                    transform: 'translateY(-50%)',
                    opacity: 0.1,
                    width: '500px',
                    pointerEvents: 'none',
                    zIndex: 0
                }}
            />

            <Container maxWidth="sm" sx={{
                my: 'auto',
                width: '100%',
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                alignItems: 'center',
                height: '100%',
                py: 3,
                px: 2,
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
                        border: '2px solid rgba(0, 0, 0, 0.2)',
                        borderRadius: 2,
                        width: '100%',
                        transition: 'all 0.3s ease',
                        transform: 'scale(1)',
                        boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
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
                            mb: 4
                        }}
                    >
                        ¡Bienvenido de nuevo!
                    </Typography>

                    {error && (
                        <Alert severity="error" sx={{ width: '100%', mb: 3 }}>
                            {error}
                        </Alert>
                    )}

                    {fieldGeneralErrors.errors.generalErrors && fieldGeneralErrors.errors.generalErrors.length > 0 && (
                        <Alert severity="error" sx={{ width: '100%', mb: 3 }}>
                            {fieldGeneralErrors.errors.generalErrors.join(' ')}
                        </Alert>
                    )}

                    <Box component="form" onSubmit={handleSubmit} noValidate sx={{ width: '100%' }}>
                        {(['identifier', 'password'] as const).map((field) => (
                            <TextField
                                key={field}
                                margin="normal"
                                fullWidth
                                name={field}
                                label={
                                    <span>
                                        {field === 'identifier' && 'Nombre de Usuario o Correo Electrónico '}
                                        {field === 'password' && 'Contraseña '}
                                        <span style={{ color: '#ff0000' }}>*</span>
                                    </span>
                                }
                                type={field === 'password' ? (showPassword ? 'text' : 'password') : 'text'}
                                id={field}
                                value={formData[field]}
                                onChange={handleChange}
                                error={!!fieldGeneralErrors.errors[field]}
                                helperText={fieldGeneralErrors.errors[field]?.join(' ')}
                                slotProps={{
                                    input: field === 'password' ? {
                                        endAdornment: (
                                            <InputAdornment position="end">
                                                <IconButton
                                                    onClick={() => setShowPassword(!showPassword)}
                                                    edge="end"
                                                    sx={{
                                                        '&:focus, &:hover, &:active, &.MuiIconButton-root': {
                                                            backgroundColor: 'transparent',
                                                            outline: 'none',
                                                            transition: 'none'
                                                        }
                                                    }}
                                                >
                                                    {showPassword ? <Visibility /> : <VisibilityOff />}
                                                </IconButton>
                                            </InputAdornment>
                                        ),
                                    } : undefined
                                }}
                                sx={{
                                    mb: 3,
                                    '& .MuiInputBase-root': {
                                        backgroundColor: 'rgba(255, 255, 255, 0.7)'
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
                                mb: 3,
                                py: 1.5,
                                backgroundColor: 'primary.main',
                                '&:hover': {
                                    backgroundColor: 'primary.dark'
                                }
                            }}
                        >
                            Iniciar Sesión
                        </Button>

                        <Box sx={{ textAlign: 'center' }}>
                            <MuiLink
                                component={Link}
                                to="/register"
                                variant="body2"
                                sx={{
                                    color: 'primary.main',
                                    textDecoration: 'none',
                                    '&:hover': {
                                        textDecoration: 'underline'
                                    }
                                }}
                                onClick={handleRegister}
                            >
                                ¿No tienes una cuenta? ¡Regístrate para la diversión!
                            </MuiLink>
                        </Box>
                    </Box>
                </Paper>
            </Container>
        </Box >
    );
}

export default LoginPage;