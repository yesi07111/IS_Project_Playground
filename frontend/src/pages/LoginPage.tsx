import React, { useState, useEffect } from 'react';
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
import { FieldGeneralErrors } from '../interfaces/Error';
import Swal from 'sweetalert2';
import { OnlinePagesProps } from '../interfaces/Pages';
import { passwordService } from '../services/passwordService';
import { CredentialResponse, GoogleLogin } from '@react-oauth/google';
import { tokenService } from '../services/tokenService';

const LoginPage: React.FC<OnlinePagesProps> = ({ online }) => {
    const [showPassword, setShowPassword] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();
    const [error, setError] = useState('');
    const [lockoutDuration, setLockoutDuration] = useState<number | null>(null);
    const [lockoutIdentifier, setLockoutIdentifier] = useState<string | null>(null);
    const { setCanAccessPasswordReset } = useAuth();
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

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleRegister = () => {
        navigate('/register');
    };

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

        if (lockoutDuration && lockoutIdentifier === formData.identifier) {
            handleLoginClick();
            return;
        }

        try {
            const response = await authService.login(
                formData.identifier,
                formData.password
            );
            if (response) {
                localStorage.setItem('authToken', response.token);
                localStorage.setItem('authId', response.id);
                localStorage.setItem('authUserName', response.username);
                localStorage.setItem('authUserRole', response.rolName);
                login();

                switch (response.rolName) {
                    case 'Parent':
                        navigate('/');
                        break;
                    case 'Admin':
                        navigate('/admin');
                        break;
                    default:
                        navigate('/');
                        break;
                }
            }
        } catch (error) {
            const apiError = error as FieldGeneralErrors;
            if (apiError && apiError.errors) {
                setFieldGeneralErrors({
                    statusCode: apiError.statusCode || 400,
                    message: apiError.message || 'Ocurrieron errores de validación.',
                    errors: apiError.errors
                });

                const lockoutMatch = apiError.errors.generalErrors[0].match(/(\d+) minutos/);
                if (lockoutMatch) {
                    const duration = parseInt(lockoutMatch[1], 10);
                    setLockoutDuration(duration * 60);
                    setLockoutIdentifier(formData.identifier);
                }
            } else {
                setError('Hubo un error durante el inicio de sesión. Por favor, inténtalo de nuevo.');
            }
        }
    };

    useEffect(() => {
        if (lockoutDuration) {
            const timer = setInterval(() => {
                setLockoutDuration((prev) => {
                    if (prev && prev > 1) {
                        return prev - 1;
                    } else {
                        clearInterval(timer);
                        setFieldGeneralErrors({
                            statusCode: 0,
                            message: '',
                            errors: {
                                generalErrors: []
                            }
                        });
                        setLockoutIdentifier(null);
                        return null;
                    }
                });
            }, 1000);

            return () => clearInterval(timer);
        }
    }, [lockoutDuration]);

    const handleLoginClick = () => {
        if (lockoutDuration) {
            Swal.fire({
                icon: 'warning',
                title: 'Cuenta bloqueada',
                text: `La cuenta está bloqueada por ${Math.floor(lockoutDuration / 60)}:${(lockoutDuration % 60).toString().padStart(2, '0')} minutos debido a 5 intentos fallidos. Si reestableces tu contraseña puedes acceder nuevamente a tu cuenta.`,
                showCancelButton: true,
                confirmButtonText: 'Restablecer contraseña',
                cancelButtonText: 'Cerrar'
            }).then((result) => {
                if (result.isConfirmed) {
                    handleResetPassword();
                }
            });
        }
    };

    const handleResetPassword = async () => {
        const response = await passwordService.ResetPassword();
        if (response) {
            setCanAccessPasswordReset(true);
            navigate('/reset-password');
        }
    };


    const handleGoogleSuccess = async (credentialResponse: CredentialResponse) => {
        if (credentialResponse.credential) {
            try {
                const tokenDecoded = await tokenService.decodeToken(credentialResponse.credential);
                // Almacenar los datos en localStorage
                if (tokenDecoded && tokenDecoded.claims) {
                    const { given_name, family_name, picture, email, email_verified } = tokenDecoded.claims;

                    try {
                        if (given_name && family_name && picture && email && email_verified) {
                            const response = await authService.googleAccess({
                                firstName: given_name,
                                lastName: family_name,
                                imageUrl: picture,
                                email: email,
                                isConfirmed: email_verified ? 'true' : 'false',
                                username: '',
                                rol: 'Parent',
                                action: 'login'
                            });

                            if (response.token && response.id && response.username) {
                                localStorage.setItem('authToken', response.token);
                                localStorage.setItem('authId', response.id);
                                localStorage.setItem('authUsername', response.username);
                                login();
                                tokenService.startTokenRefreshCheck();
                                navigate('/');
                            }

                        }
                    } catch (error) {
                        const generalError = error as FieldGeneralErrors;
                        setFieldGeneralErrors(generalError);
                    }
                }
            } catch (error) {
                console.error("Error al decodificar el token de autenticación de Google: ", error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Ha ocurrido un error inesperado. Por favor, inténtalo de nuevo.',
                });
            }
        } else {
            console.error("No se recibió credencial en la respuesta.");
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se recibió credencial. Por favor, inténtalo de nuevo.',
            });
        }
    };


    const handleGoogleFailure = () => {
        console.error('Fallo el inicio de sesión con Google');
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Fallo el inicio de sesión con Google. Por favor, inténtalo de nuevo.',
        });
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
                        }}>
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

                    {online && (
                        <>
                            <GoogleLogin
                                onSuccess={handleGoogleSuccess}
                                onError={handleGoogleFailure}
                            />

                            <Typography
                                variant="body1"
                                sx={{
                                    color: 'gray',
                                    my: 2
                                }}
                            >

                                o
                            </Typography>
                        </>
                    )}

                    {lockoutDuration && (
                        <Typography variant="body2" sx={{ mb: 0 }}>
                            Tiempo restante de bloqueo: {Math.floor(lockoutDuration / 60)}:{(lockoutDuration % 60).toString().padStart(2, '0')}
                        </Typography>
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
                                    mb: 0,
                                    mt: 4,
                                    '& .MuiInputBase-root': {
                                        backgroundColor: 'rgba(255, 255, 255, 0.7)'
                                    }
                                }}
                            />
                        ))}

                        <Typography variant="body2" sx={{ textAlign: 'right', mb: 2, fontSize: '0.960rem' }}>
                            <MuiLink
                                component={Link}
                                to="/reset-password"
                                sx={{
                                    color: 'primary.main',
                                    textDecoration: 'none',
                                    '&:hover': {
                                        textDecoration: 'underline'
                                    }
                                }}
                                onClick={handleResetPassword}
                            >
                                ¿Olvidaste tu contraseña?
                            </MuiLink>
                        </Typography>

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
                            onClick={handleLoginClick}
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