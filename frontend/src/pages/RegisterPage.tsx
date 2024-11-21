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
import decoration1 from '../assets/images/decorative/toy-train.png';
import decoration2 from '../assets/images/decorative/swing.png';
import { authService } from '../services/authService';

const RegisterPage: React.FC = () => {
    const navigate = useNavigate();
    const [showPassword, setShowPassword] = useState(false);
    const [error, setError] = useState('');
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        userName: '',
        password: ''
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');

        try {
            await authService.register({
                FirstName: formData.firstName,
                LastName: formData.lastName,
                Username: formData.userName,
                Password: formData.password,
                Email: formData.email,
                Roles: ['Parent']
            });
            localStorage.setItem('pendingVerificationEmail', formData.userName);
            navigate('/verify-email');
        } catch (error: unknown) {
            console.error('Error during registration:', error);
            setError('There was an error during registration. Please try again.');
        }
    };

    const textFieldSx = {
        mb: 2,
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
                src={decoration1}
                sx={{
                    position: 'absolute',
                    right: -20,
                    bottom: '36%',
                    opacity: 0.1,
                    width: '400px',
                    pointerEvents: 'none',
                    transform: 'translate(-50px, -50px)'
                }}
            />

            <Box
                component="img"
                src={decoration2}
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
                height: '80vh',
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
                        ¡Únete a la diversión!
                    </Typography>

                    {error && (
                        <Alert severity="error" sx={{ width: '100%', mb: 3 }}>
                            {error}
                        </Alert>
                    )}

                    <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%' }}>
                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="firstName"
                            label="Nombre"
                            type="text"
                            id="firstName"
                            autoFocus
                            value={formData.firstName}
                            onChange={handleChange}
                            sx={textFieldSx}
                        />

                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="lastName"
                            label="Apellidos"
                            type="text"
                            id="lastName"
                            value={formData.lastName}
                            onChange={handleChange}
                            sx={textFieldSx}
                        />

                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="email"
                            label="Correo Electrónico"
                            type="email"
                            id="email"
                            value={formData.email}
                            onChange={handleChange}
                            sx={textFieldSx}
                        />

                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="userName"
                            label="Nombre de Usuario"
                            type="text"
                            id="userName"
                            value={formData.userName}
                            onChange={handleChange}
                            sx={textFieldSx}
                        />

                        <TextField
                            margin="normal"
                            required
                            fullWidth
                            name="password"
                            label="Contraseña"
                            type={showPassword ? 'text' : 'password'}
                            id="password"
                            value={formData.password}
                            onChange={handleChange}
                            InputProps={{
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
                            }}
                            sx={{
                                ...textFieldSx,
                                mb: 3
                            }}
                        />

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
                            Registrarse
                        </Button>

                        <Box sx={{ textAlign: 'center' }}>
                            <MuiLink
                                component={Link}
                                to="/login"
                                variant="body2"
                                sx={{
                                    color: 'primary.main',
                                    textDecoration: 'none',
                                    '&:hover': {
                                        textDecoration: 'underline'
                                    }
                                }}
                            >
                                ¿Ya tienes una cuenta? ¡Inicia sesión aquí!
                            </MuiLink>
                        </Box>
                    </Box>
                </Paper>
            </Container>
        </Box>
    );
};

export default RegisterPage;