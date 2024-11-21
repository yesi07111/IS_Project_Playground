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
    IconButton
} from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import kidsBalloons from '../assets/images/decorative/learning.png';
import kidsStudying from '../assets/images/decorative/tree-house.png';

const LoginPage: React.FC = () => {
    const navigate = useNavigate();
    const [showPassword, setShowPassword] = useState(false);
    const [formData, setFormData] = useState({
        email: '',
        password: ''
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
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

                    <Box component="form" onSubmit={handleSubmit} sx={{ width: '100%' }}>
                        <TextField
                            margin="normal"
                            fullWidth
                            id="email"
                            label="Nombre de Usuario o Correo Electrónico"
                            name="email"
                            autoComplete="email"
                            autoFocus
                            value={formData.email}
                            onChange={handleChange}
                            required={false}
                            sx={{
                                mb: 2,
                                '& .MuiInputBase-root': {
                                    backgroundColor: 'rgba(255, 255, 255, 0.7)'
                                }
                            }}
                        />

                        <TextField
                            margin="normal"
                            fullWidth
                            name="password"
                            label="Contraseña"
                            type={showPassword ? 'text' : 'password'}
                            id="password"
                            autoComplete="current-password"
                            value={formData.password}
                            onChange={handleChange}
                            required={false}
                            InputProps={{
                                endAdornment: (
                                    <InputAdornment position="end">
                                        <IconButton
                                            onClick={() => setShowPassword(!showPassword)}
                                            edge="end"
                                            disableRipple
                                            sx={{
                                                '&:focus': {
                                                    backgroundColor: 'transparent',
                                                    outline: 'none'
                                                },
                                                '&:hover': {
                                                    backgroundColor: 'transparent'
                                                },
                                                '&:active': {
                                                    backgroundColor: 'transparent'
                                                },
                                                '&.MuiIconButton-root': {
                                                    transition: 'none',
                                                    '&::after': {
                                                        display: 'none'
                                                    }
                                                }
                                            }}
                                        >
                                            {showPassword ? <Visibility /> : <VisibilityOff />}
                                        </IconButton>
                                    </InputAdornment>
                                ),
                            }}
                            sx={{
                                mb: 3,
                                '& .MuiInputBase-root': {
                                    backgroundColor: 'rgba(255, 255, 255, 0.7)'
                                }
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