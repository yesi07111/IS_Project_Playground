import React, { useState, useEffect, useCallback } from 'react';
import { useParams } from 'react-router-dom';
import { Box, Container, Typography, CardMedia, Rating, Button } from '@mui/material';
import { styled } from '@mui/material/styles';
import { ActivityDetail } from '../interfaces/Activity';
import { activityService } from '../services/activityService';
import { cacheService } from '../services/cacheService';
import pattern1 from '/images/decorative/swing.png';
import pattern2 from '/images/decorative/tea-set.png';
import CommentsContainer from '../components/features/CommentsContainer';
import { parseDate } from '../services/dateService';

const BackgroundImage = styled(Box)({
    position: 'absolute',
    top: 0,
    left: 0,
    width: '100%',
    height: '100%',
    backgroundSize: 'cover',
    backgroundPosition: 'center',
    opacity: 0.1,
    zIndex: -1,
});

const SectionBox = styled(Box)(({ theme }) => ({
    backgroundColor: theme.palette.background.paper,
    borderRadius: 8,
    padding: theme.spacing(3),
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    transition: 'transform 0.3s, box-shadow 0.3s',
    '&:hover': {
        transform: 'scale(1.05)',
        boxShadow: '0 8px 16px rgba(0, 0, 0, 0.2)',
    },
    marginBottom: theme.spacing(3),
}));

const LoadingorErrorBox = styled(Box)({
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    minHeight: '100vh',
    width: '100vw',
    backgroundColor: 'rgba(255, 255, 255, 0.8)',
    position: 'absolute',
    top: 0,
    left: 0,
    zIndex: 2,
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    transition: 'box-shadow 0.3s',
    '&:hover': {
        boxShadow: '0 8px 16px rgba(0, 0, 0, 0.2)',
    },
});

const ActivityInfoPage: React.FC<{ reload: boolean }> = ({ reload }) => {
    const { id, imagePath, useCase } = useParams<{ id: string; imagePath: string; useCase: string }>();
    const [activity, setActivity] = useState<ActivityDetail | null>(null);
    const [error, setError] = useState<string | null>(null);

    const decodedImagePath = imagePath ? decodeURIComponent(imagePath) : '';

    const fetchActivityDetail = useCallback(async () => {
        try {
            if (id && useCase) {
                let activityDetail = cacheService.loadActivityDetail(id);

                if (!activityDetail) {
                    const response = await activityService.getActivity(id, useCase);
                    activityDetail = response.result;
                    cacheService.saveActivityDetail(activityDetail);
                }

                setActivity(activityDetail);
            }
        } catch {
            setError('Error al cargar los detalles de la actividad');
        }
    }, [id, useCase]);

    useEffect(() => {
        fetchActivityDetail();
    }, [fetchActivityDetail]);

    useEffect(() => {
        if (reload) {
            fetchActivityDetail();
        }
    }, [reload, fetchActivityDetail]);

    if (error) {
        return (
            <Box sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                minHeight: '100vh',
                width: '100vw',
                backgroundColor: '#f0f8ff',
            }}>
                <LoadingorErrorBox>
                    <Typography variant="h6" sx={{ color: 'error', fontWeight: 'bold' }}>
                        {error}
                    </Typography>
                </LoadingorErrorBox>
            </Box>
        );
    }

    if (!activity) {
        return (
            <Box sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                minHeight: '100vh',
                width: '100vw',
                backgroundColor: '#f0f8ff',
            }}>
                <LoadingorErrorBox>
                    <Typography variant="h6" sx={{ color: '#555', fontWeight: 'bold' }}>
                        ⏳ Cargando...
                    </Typography>
                </LoadingorErrorBox>
            </Box>
        );
    }

    const { formattedDate, formattedTime } = parseDate(activity.date);
    const isPastActivity = new Date(activity.date) < new Date();
    const availableSpots = activity.maximumCapacity - activity.currentCapacity;
    const comments = activity.comments.map((commentStr) => {
        const [username, ratingStr, comment] = commentStr.split(':');
        return { username, rating: parseFloat(ratingStr), comment };
    });

    return (
        <Box sx={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            minHeight: '100vh',
            width: '100vw',
            position: 'relative',
            overflow: 'hidden',
            backgroundColor: '#f0f8ff',
        }}>
            <BackgroundImage sx={{ backgroundImage: pattern1 }} />
            <BackgroundImage sx={{ backgroundImage: pattern2, opacity: 0.15 }} />
            <Container maxWidth="md" sx={{ zIndex: 1 }}>
                <SectionBox>
                    <CardMedia
                        component="img"
                        height="400"
                        image={decodedImagePath || '/images/default.jpg'}
                        alt={activity.name}
                        sx={{ borderRadius: 2, marginBottom: 2 }}
                    />
                    <Typography variant="h5" component="div" sx={{ color: 'text.secondary', marginBottom: 2 }}>
                        🎨 Título: {activity.name}
                    </Typography>
                    <Typography variant="body1" color="text.secondary">
                        📜 Descripción: {activity.description}
                    </Typography>
                </SectionBox>

                <SectionBox>
                    {isPastActivity && (
                        <Box sx={{ display: 'flex', alignItems: 'center', marginBottom: 2 }}>
                            <Typography variant="h6" sx={{ color: '#ff69b4', marginRight: 1 }}>
                                🏆 Calificación:
                            </Typography>
                            <Rating value={activity.rating} readOnly precision={0.5} />
                        </Box>
                    )}
                    <Typography variant="h6" sx={{ color: '#0000FF', marginBottom: 1 }}>
                        📅 Fecha: {formattedDate}
                    </Typography>
                    <Typography variant="h6" sx={{ color: '#DA70D6', marginBottom: 1 }}>
                        🕒 Hora: {formattedTime}
                    </Typography>
                    <Typography variant="h6" sx={{ color: '#8a2be2' }}>
                        {isPastActivity ? '👥 Asistentes' : '👥 Capacidad'}: {activity.currentCapacity}/{activity.maximumCapacity}
                    </Typography>
                    {!isPastActivity && availableSpots > 0 && (
                        <>
                            <Typography variant="body1" sx={{ color: '#ff8c00', fontWeight: 'bold', marginTop: 2 }}>
                                ¡Quedan solo {availableSpots} plazas disponibles, reserva ahora!
                            </Typography>
                            <Button variant="contained" color="primary" sx={{ marginTop: 2 }}>
                                Reservar
                            </Button>
                        </>
                    )}
                    {!isPastActivity && availableSpots == 0 && (
                        <>
                            <Typography variant="body1" sx={{ color: '#ff8c00', fontWeight: 'bold', marginTop: 2 }}>
                                Esta actividad ya está llena, ¡una pena!
                            </Typography>
                        </>
                    )}
                </SectionBox>

                <SectionBox>
                    <Typography variant="h6" sx={{ color: '#32cd32' }}>
                        👨‍🏫 Educador
                    </Typography>
                    <Typography variant="body1">Nombre: {activity.educatorFullName}</Typography>
                    <Typography variant="body1">Usuario: {activity.educatorUsername}</Typography>
                </SectionBox>

                <SectionBox>
                    <Typography variant="h6" sx={{ color: '#1e90ff' }}>
                        🏢 Instalación
                    </Typography>
                    <Typography variant="body1">Nombre: {activity.facilityName}</Typography>
                    <Typography variant="body1">Ubicación: {activity.facilityLocation}</Typography>
                    <Typography variant="body1">Tipo: {activity.facilityType}</Typography>
                </SectionBox>

                <SectionBox>
                    <Typography variant="h6" sx={{ color: '#ff4500' }}>
                        📅 Detalles de la Actividad
                    </Typography>
                    <Typography variant="body1">Tipo de Actividad: {activity.activityType}</Typography>
                    <Typography variant="body1">Política de Uso: {activity.usagePolicy}</Typography>
                    <Typography variant="body1">Edad Recomendada: {activity.recommendedAge}</Typography>
                </SectionBox>

                {isPastActivity && (
                    <SectionBox>
                        <Typography variant="h6" sx={{ color: '#ff6347' }}>
                            💬 Comentarios
                        </Typography>
                        <CommentsContainer comments={comments} />
                    </SectionBox>
                )}
            </Container>
        </Box>
    );
};

export default ActivityInfoPage;