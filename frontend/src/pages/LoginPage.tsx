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
import kidsBalloons from '../assets/images/decorative/learning.png';
import kidsStudying from '../assets/images/decorative/tree-house.png';
import { useAuth } from '../components/auth/authContext';
import { authService } from '../services/authService';

const LoginPage: React.FC = () => {
    const [showPassword, setShowPassword] = useState(false);
    const { login, setCanAccessUserType } = useAuth();
    const navigate = useNavigate();
    const [error, setError] = useState('');
    const [formData, setFormData] = useState({
        identifier: '',
        password: ''
    });

    type FieldErrors = {
        statusCode: number;
        message: string;
        errors: {
            [key: string]: string[];
            generalErrors: string[];
        };
    };

    const [fieldErrors, setFieldErrors] = useState<FieldErrors>({
        statusCode: 0,
        message: '',
        errors: {
            generalErrors: []
        }
    });

    const handleUserTypeAccess = () => {
        setCanAccessUserType(true);
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setFieldErrors({
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
                login()
                navigate('/');
            }

        } catch (error) {
            const apiError = error as FieldErrors;
            if (apiError && apiError.errors) {
                setFieldErrors({
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

                    {fieldErrors.errors.generalErrors.length > 0 && (
                        <Alert severity="error" sx={{ width: '100%', mb: 3 }}>
                            {fieldErrors.errors.generalErrors.join(' ')}
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
                                error={!!fieldErrors.errors[field]}
                                helperText={fieldErrors.errors[field]?.join(' ')}
                                InputProps={{
                                    endAdornment: field === 'password' ? (
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
                                    ) : undefined
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
                                to="/user-type"
                                variant="body2"
                                sx={{
                                    color: 'primary.main',
                                    textDecoration: 'none',
                                    '&:hover': {
                                        textDecoration: 'underline'
                                    }
                                }}
                                onClick={handleUserTypeAccess}
                            >
                                ¿No tienes una cuenta? ¡Regístrate para la diversión!
                            </MuiLink>
                        </Box>
                    </Box>
                </Paper>
            </Container>
        </Box>
    );
}

export default LoginPage;