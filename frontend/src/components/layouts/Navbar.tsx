import React, { useState, useEffect } from 'react';
import { AppBar, Toolbar, Button, Typography, Box, IconButton } from '@mui/material';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import Confetti from 'react-confetti';
import { useAuth } from '../auth/authContext';
import styled, { keyframes } from 'styled-components';
import UserProfile from '../auth/UserProfile';

/**
 * Animación de gradiente para el texto.
 * 
 * Define una animación de gradiente que se mueve de izquierda a derecha.
 */
const gradientAnimation = keyframes`
    0% { background-position: 0% 50%; }
    100% { background-position: 100% 50%; }
`;

/**
 * Componente estilizado de texto con gradiente animado.
 * 
 * Este componente utiliza `styled-components` para aplicar un gradiente animado
 * al texto, creando un efecto visual atractivo.
 */
const GradientText = styled.span`
    background: linear-gradient(90deg, #ff0000, #006400, #0000ff, #ff0000);
    background-size: 300% 300%;
    -webkit-background-clip: text;
    background-clip: text;
    color: transparent;
    animation: ${gradientAnimation} 5s linear infinite;
    font-size: 1.5rem;
`;

/**
 * Componente de barra de navegación que proporciona enlaces de navegación y funcionalidad de usuario.
 * 
 * Este componente utiliza Material-UI para crear una barra de navegación que incluye enlaces a diferentes
 * secciones de la aplicación, un botón de retroceso condicional, y un efecto de confeti en la página de inicio.
 * También gestiona el estado de autenticación del usuario para mostrar opciones de perfil o inicio de sesión.
 * 
 * @returns {JSX.Element} El componente de barra de navegación.
 */
const Navbar: React.FC = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const [showConfetti, setShowConfetti] = useState(false);
    const { isAuthenticated } = useAuth();

    // Determina si se debe mostrar el botón de retroceso
    const showBackButton = location.pathname !== '/';

    /**
     * Maneja la acción de retroceso, navegando a diferentes rutas según la ubicación actual.
     */
    const handleBack = async () => {
        if (location.pathname === '/verify-email') {
            const storedFormData = localStorage.getItem('formData');
            localStorage.setItem("ToDelete", "true");
            if (storedFormData) {
                navigate('/register', { state: JSON.parse(storedFormData) });
            } else {
                navigate(-1);
            }
        } else if (location.pathname === '/register') {
            localStorage.removeItem('formData');
            navigate('/user-type');
        } else {
            navigate(-1);
        }
    };

    /**
     * Efecto que muestra confeti cuando la ruta es la página de inicio.
     */
    useEffect(() => {
        if (location.pathname === '/') {
            setShowConfetti(true);
            const timer = setTimeout(() => setShowConfetti(false), 7000);
            return () => clearTimeout(timer);
        }
    }, [location.pathname]);

    /**
     * Maneja el evento de pasar el ratón sobre el texto, activando el confeti.
     */
    const handleMouseEnter = () => {
        setShowConfetti(true);
        setTimeout(() => setShowConfetti(false), 7000);
    };

    return (
        <>
            {showConfetti && <Confetti width={window.innerWidth} height={window.innerHeight} recycle={false} />}
            <AppBar
                position="sticky"
                sx={{
                    top: 0,
                    width: '100%',
                    zIndex: 1100,
                    backgroundColor: 'primary.main',
                    '& .MuiToolbar-root': {
                        width: '100%',
                        maxWidth: '100%',
                        justifyContent: 'space-between',
                        padding: { xs: '8px 16px', sm: '8px 24px' },
                        minHeight: '48px',
                    }
                }}
            >
                <Toolbar
                    variant="dense"
                    sx={{
                        minHeight: '48px !important',
                        position: 'relative',
                    }}
                >
                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                        {showBackButton ? (
                            <IconButton
                                color="inherit"
                                onClick={handleBack}
                                sx={{
                                    marginRight: 2,
                                    backgroundColor: 'transparent',
                                    '&:focus': {
                                        outline: 'none',
                                    }
                                }}
                            >
                                <ArrowBackIcon />
                            </IconButton>
                        ) : (
                            <Typography
                                variant="h6"
                                sx={{
                                    color: 'inherit',
                                    flexGrow: 0,
                                    fontSize: '1.1rem',
                                    cursor: 'default',
                                    position: 'relative',
                                    display: 'flex',
                                    alignItems: 'center',
                                }}
                                onMouseEnter={handleMouseEnter}
                            >
                                <GradientText>Parque Infantil </GradientText>
                            </Typography>
                        )}
                    </Box>
                    <Box sx={{
                        display: 'flex',
                        gap: { xs: 1, sm: 2 }
                    }}>
                        <Button
                            color="inherit"
                            component={Link}
                            to="/"
                            sx={{
                                fontWeight: 500,
                                py: 0.5,
                            }}
                        >
                            Inicio
                        </Button>
                        <Button
                            color="inherit"
                            component={Link}
                            to="/actividades"
                            sx={{
                                fontWeight: 500,
                                py: 0.5,
                            }}
                        >
                            Actividades
                        </Button>
                        <Button
                            color="inherit"
                            component={Link}
                            to="/reservas"
                            sx={{
                                fontWeight: 500,
                                py: 0.5,
                            }}
                        >
                            Reservas
                        </Button>
                        {isAuthenticated ? (
                            <>
                                <UserProfile />
                            </>
                        ) : (
                            <Button
                                color="inherit"
                                component={Link}
                                to="/login"
                                sx={{
                                    fontWeight: 500,
                                    py: 0.5,
                                }}
                            >
                                Iniciar Sesión
                            </Button>
                        )}
                    </Box>
                </Toolbar>
            </AppBar>
        </>
    );
};

export default Navbar;