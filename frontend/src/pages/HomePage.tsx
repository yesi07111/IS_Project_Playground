import React, { useEffect, useState, useCallback } from 'react';
import { Container, Typography, Box, Button } from '@mui/material';
import Grid from '@mui/material/Grid2';
import { Link } from 'react-router-dom';
import ActivityCard from '../components/features/ActivityCard';
import StatsSummary from '../components/features/StatsSummary';
import heroBg from '/images/home-bg.jpg';
import pattern1 from '/images/decorative/hand-print.png';
import pattern2 from '/images/decorative/kindergarten.png';
import pattern3 from '/images/decorative/bumper-car.png';
import pattern4 from '/images/decorative/soccer.png';

import { Activity } from '../interfaces/Activity';
import { activityService } from '../services/activityService';
import { ActivitiesFilters } from '../interfaces/Filters';
import { cacheService } from '../services/cacheService';
import { DataPagesProps } from '../interfaces/Pages';
import { useAuth } from '../components/auth/authContext';

const HomePage: React.FC<DataPagesProps> = ({ reload }) => {
    const [activities, setActivities] = useState<Activity[]>([]);
    const role = localStorage.getItem('authUserRole');
    const userName = localStorage.getItem('authUserName');
    const [activityImages, setActivityImages] = useState<{ [key: string]: string }>(() => cacheService.loadImages() || {});
    const { isAuthenticated } = useAuth();
    const [reserveRoute, setReserveRoute] = useState<string>("/");


    const cacheActivityImages = useCallback((activitiesArray: Activity[]) => {
        setActivityImages((prevImagesMap) => {
            const newImagesMap = { ...prevImagesMap };
            activitiesArray.forEach((activity: Activity) => {
                if (!newImagesMap[activity.id]) {
                    newImagesMap[activity.id] = activity.image;
                }
            });
            cacheService.saveImages(newImagesMap);
            return newImagesMap;
        });
    }, []);

    const fetchActivities = useCallback(async () => {
        if (!navigator.onLine) {
            // Si no hay conexión, cargar desde el caché
            const cachedActivities = cacheService.loadTopActivities();
            const cachedImages = cacheService.loadImages();
            setActivityImages(cachedImages);
            setActivities(cachedActivities.map((activity: Activity) => ({
                ...activity,
                image: cachedImages[activity.id] || activity.image,
            })));
            return;
        }

        try {
            const filters: ActivitiesFilters[] = [{ type: 'Casos de Uso', useCase: 'HomeView' }];
            const response = await activityService.getAllActivities(filters);
            const activitiesArray = response.result as Activity[];
            cacheActivityImages(activitiesArray);
            setActivities(activitiesArray.map(activity => ({
                ...activity,
                image: activityImages[activity.id] || activity.image,
            })));
            cacheService.saveTopActivities(activitiesArray); // Guardar en caché
        } catch (error) {
            console.error('Error fetching activities:', error);
            const cachedActivities = cacheService.loadTopActivities();
            const cachedImages = cacheService.loadImages();
            setActivityImages(cachedImages);
            setActivities(cachedActivities.map((activity: Activity) => ({
                ...activity,
                image: cachedImages[activity.id] || activity.image,
            })));
        }
    }, [cacheActivityImages, activityImages]);

    useEffect(() => {
        fetchActivities();
    }, []);

    useEffect(() => {
        if (isAuthenticated) {
            setReserveRoute("/activities")
        }
        else {
            setReserveRoute("/login")
        }
    }, [isAuthenticated]);

    useEffect(() => {
        if (reload) {
            cacheService.saveTopActivities([]);
            fetchActivities();
        }
    }, [reload, fetchActivities]);

    return (
        <Box sx={{
            width: '100vw',
            minHeight: '100vh',
            overflow: 'hidden',
            backgroundColor: 'white',
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
                    {isAuthenticated && (role === 'Educator' || role === 'Admin') ?
                        (
                            <>
                                <Typography
                                    variant="h1"
                                    sx={{
                                        fontSize: { xs: '2.5rem', sm: '3.5rem', md: '4.5rem' },
                                        fontWeight: 700,
                                        textShadow: '2px 2px 4px rgba(0,0,0,0.3)',
                                        mb: 2
                                    }}
                                >
                                    {role === 'Admin' ? 'Bienvenido Administrador' : 'Bienvenido Profesor'} {userName}
                                </Typography>
                            </>
                        ) :
                        (
                            <>
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
                                    to={reserveRoute}
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
                            </>
                        )}
                </Box>

            </Box>

            {/* Stats Section */}
            <Box
                sx={{
                    width: '100%',
                    backgroundColor: 'white',
                    py: 6,
                    position: 'relative',
                    overflow: 'visible'
                }}
            >
                <Box
                    component="img"
                    src={pattern1}
                    alt="Hand print pattern"
                    sx={{
                        position: 'absolute',
                        left: -40,
                        top: 10,
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
                        right: -10,
                        bottom: 10,
                        opacity: 0.1,
                        width: '200px'
                    }}
                />
                <Container maxWidth={false}>
                    <StatsSummary />
                </Container>
            </Box>

            {/* Hours Section */}
            <Box
                sx={{
                    py: 6,
                    textAlign: 'center',
                    maxWidth: '800px',
                    margin: '0 auto',
                    backgroundColor: 'transparent',
                    border: '1px solid rgba(0, 0, 0, 0.1)',
                    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
                    transition: 'transform 0.3s ease-in-out',
                    '&:hover': {
                        transform: 'scale(1.05)',
                    },
                    position: 'relative',
                    overflow: 'hidden'
                }}
            >
                <Box
                    component="img"
                    src={pattern4}
                    alt="Bumper car pattern"
                    sx={{
                        position: 'absolute',
                        left: -12,
                        top: -20,
                        opacity: 0.1,
                        width: '200px'
                    }}
                />
                <Box
                    component="img"
                    src={pattern3}
                    alt="Soccer pattern"
                    sx={{
                        position: 'absolute',
                        right: -50,
                        bottom: -50,
                        opacity: 0.1,
                        width: '200px'
                    }}
                />
                <Typography
                    variant="h5"
                    sx={{
                        color: '#2C3E50',
                        fontWeight: 700,
                        mb: 2,
                    }}
                >
                    Horario
                </Typography>
                <Typography
                    variant="h6"
                    sx={{
                        color: '#2C3E50',
                    }}
                >
                    Abierto desde las <strong>9:00 am</strong> hasta las <strong>5:00 pm</strong>
                </Typography>
            </Box>

            {/* Activities Section */}
            <Box sx={{
                py: 8,
                px: 0,
                width: '100%',
                backgroundColor: 'white'
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
                        {activities.map((activity) => (
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