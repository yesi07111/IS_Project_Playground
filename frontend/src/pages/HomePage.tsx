import React from 'react';
import { Container, Typography, Box, Button } from '@mui/material';
import Grid from '@mui/material/Grid2';
import { Link } from 'react-router-dom';
import ActivityCard from '../components/features/ActivityCard';
import StatsSummary from '../components/features/StatsSummary';
import heroBg from '../assets/images/home-bg.jpg';
import artWorkshop from '../assets/images/activities/art-workshop-1.jpg';
import decoration1 from '../assets/images/activities/kids-sport.jpg';
import decoration2 from '../assets/images/activities/kids-science.jpg';
import pattern1 from '../assets/images/decorative/hand-print.png';
import pattern2 from '../assets/images/decorative/kindergarten.png';
import { Activity } from '../interfaces/Activity';

const mockActivities: Activity[] = [
    {
        id: 1,
        name: "Taller de Arte",
        description: "Actividades creativas para desarrollar habilidades artísticas",
        image: artWorkshop,
        rating: 4.5,
        color: '#FF6B6B'
    },
    {
        id: 2,
        name: "Juegos Deportivos",
        description: "Actividades físicas y deportivas para niños",
        image: decoration1,
        rating: 4.8,
        color: '#4ECDC4'
    },
    {
        id: 3,
        name: "Club de Ciencias",
        description: "Experimentos y descubrimientos científicos divertidos",
        image: decoration2,
        rating: 4.6,
        color: '#FFD93D'
    }
];

/**
 * Componente funcional que representa la página de inicio.
 * 
 * Este componente utiliza varios elementos de Material-UI para estructurar
 * la página, incluyendo `Box`, `Typography`, `Button`, y `Grid`. También
 * incorpora componentes personalizados como `ActivityCard` y `StatsSummary`.
 * 
 * La página se divide en varias secciones:
 * - **Hero Section**: Presenta un fondo de imagen con un mensaje de bienvenida
 *   y un botón para realizar reservas.
 * - **Stats Section**: Muestra un resumen estadístico utilizando el componente
 *   `StatsSummary`.
 * - **Activities Section**: Lista de actividades destacadas, cada una representada
 *   por un `ActivityCard`.
 * 
 * @returns {JSX.Element} El componente de la página de inicio.
 */
const HomePage: React.FC = () => {
    return (
        <Box sx={{
            width: '100vw',  // Cambiado a viewport width
            minHeight: '100vh',
            overflow: 'hidden',
            backgroundColor: '#f8f9fa',
            margin: 0,
            padding: 0
        }}>
            {/* Hero Section */}
            <Box
                sx={{
                    position: 'relative',
                    width: '100%',
                    height: '70vh',
                    backgroundImage: `url(${heroBg})`,
                    backgroundSize: 'cover',
                    backgroundPosition: 'center',
                    '&::before': {
                        content: '""',
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        right: 0,
                        bottom: 0,
                        backgroundColor: 'rgba(0,0,0,0.5)'
                    }
                }}
            >
                <Box
                    sx={{
                        position: 'absolute',
                        width: '100%',
                        height: '100%',
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: 'center',
                        alignItems: 'center',
                        color: 'white',
                        textAlign: 'center',
                        zIndex: 1
                    }}
                >
                    <Typography
                        variant="h1"
                        sx={{
                            fontSize: { xs: '2.5rem', sm: '3.5rem', md: '4.5rem' },
                            fontWeight: 700,
                            textShadow: '2px 2px 4px rgba(0,0,0,0.3)',
                            mb: 2
                        }}
                    >
                        Bienvenido al Parque Infantil
                    </Typography>
                    <Typography
                        variant="h4"
                        sx={{
                            fontSize: { xs: '1.2rem', sm: '1.5rem', md: '2rem' },
                            maxWidth: '800px',
                            mb: 4,
                            px: 2
                        }}
                    >
                        El mejor lugar para la diversión y el aprendizaje de tus hijos 🎊
                    </Typography>
                    <Button
                        variant="contained"
                        size="large"
                        component={Link}
                        to="/reservas"
                        sx={{
                            backgroundColor: '#FF6B6B',
                            fontSize: '1.2rem',
                            py: 2,
                            px: 4,
                            '&:hover': {
                                backgroundColor: '#ff5252'
                            }
                        }}
                    >
                        ¡Reserva Ahora!
                    </Button>
                </Box>
            </Box>

            {/* Stats Section */}
            <Box
                sx={{
                    width: '100%',
                    backgroundColor: 'white',
                    py: 6,
                    position: 'relative',
                    overflow: 'hidden'
                }}
            >
                <Box
                    component="img"
                    src={pattern1}
                    alt="Hand print pattern"
                    sx={{
                        position: 'absolute',
                        left: -50,
                        top: -50,
                        opacity: 0.1,
                        width: '200px'
                    }}
                />
                <Box
                    component="img"
                    src={pattern2}
                    alt="Kindergarten pattern"
                    sx={{
                        position: 'absolute',
                        right: -50,
                        bottom: -50,
                        opacity: 0.1,
                        width: '200px'
                    }}
                />
                <Container maxWidth={false}>
                    <StatsSummary />
                </Container>
            </Box>

            {/* Activities Section */}
            <Box sx={{
                py: 8,
                px: 0,
                width: '100%',
                backgroundColor: '#f8f9fa'
            }}>
                <Typography
                    variant="h3"
                    sx={{
                        textAlign: 'center',
                        mb: 6,
                        color: '#2C3E50',
                        fontWeight: 700
                    }}
                >
                    Actividades Destacadas
                </Typography>
                <Container maxWidth={false} sx={{ px: { xs: 2, sm: 4, md: 6 } }}>
                    <Grid container spacing={4}>
                        {mockActivities.map((activity) => (
                            <Grid size={{ xs: 12, sm: 6, md: 4 }} key={activity.id}
                                sx={{
                                    display: 'flex',
                                    justifyContent: 'center'
                                }}
                            >
                                <ActivityCard activity={activity} />
                            </Grid>
                        ))}
                    </Grid>
                </Container>
            </Box>
        </Box>
    );
};

export default HomePage;