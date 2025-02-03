import React, { useState, useEffect, useCallback } from 'react';
import { usePDF } from 'react-to-pdf';
import { useParams } from 'react-router-dom';
import { Box, Container, Typography, CardMedia, Rating, Button } from '@mui/material';
import { styled } from '@mui/material/styles';
import { ActivityDetail } from '../interfaces/Activity';
import { activityService } from '../services/activityService';
import { cacheService } from '../services/cacheService';
import pattern1 from '/images/decorative/swing.png';
import pattern2 from '/images/decorative/tea-set.png';
import CommentsContainer from '../components/features/CommentsContainer';
import { DataPagesProps } from '../interfaces/Pages';
import { dateService } from '../services/dateService';
import UserLink from '../components/auth/UserLink';
import Swal from 'sweetalert2';
import { ReservationFormData } from '../interfaces/Reservation';
import { reservationService } from '../services/reservationService';

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

const ActivityInfoPage: React.FC<DataPagesProps> = ({ reload }) => {
    const { id, imagePath, useCase } = useParams<{ id: string; imagePath: string; useCase: string }>();
    const [activity, setActivity] = useState<ActivityDetail | null>(null);
    const [error, setError] = useState<string | null>(null);

    const decodedImagePath = imagePath ? decodeURIComponent(imagePath) : '';

    const { toPDF, targetRef } = usePDF({ filename: 'Info.Actividad.pdf' });

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

                // Log activity comments and rating
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

    const handleReservation = async () => {
        const availableSpots = activity ? activity.maximumCapacity - activity.currentCapacity : 0;

        // Solicitar la cantidad de niños
        const { value: amount } = await Swal.fire({
            title: 'Reserva de Actividad',
            text: 'Ingrese la cantidad de niños para la reserva:',
            input: 'number',
            inputAttributes: {
                min: '1',
                max: availableSpots.toString(),
                step: '1'
            },
            showCancelButton: true,
            confirmButtonText: 'Siguiente',
            cancelButtonText: 'Cancelar',
            preConfirm: (value) => {
                const parsedValue = parseInt(value);
                if (!value || isNaN(parsedValue) || parsedValue <= 0) {
                    Swal.showValidationMessage('Por favor, ingrese un número válido.');
                    return 0;
                } else if (parsedValue > availableSpots) {
                    Swal.showValidationMessage(`La cantidad de niños no puede superar la disponibilidad actual de ${availableSpots}.`);
                    return 0;
                }
                return parsedValue;
            }
        });
        if (!amount) {
            return; // Si el usuario cancela o no ingresa un valor válido
        }

        // Solicitar comentarios adicionales
        const { value: comments } = await Swal.fire({
            title: 'Comentarios Adicionales',
            text: 'Comentarios adicionales (Opcional):',
            input: 'text',
            showCancelButton: true,
            confirmButtonText: 'Reservar',
            cancelButtonText: 'Cancelar',
            preConfirm: (value) => {
                if (value.length > 150) {
                    Swal.showValidationMessage('Los comentarios adicionales no pueden exceder los 150 caracteres.');
                    return "";
                }
                return value;
            }
        });

        const formData: ReservationFormData = {
            amount: parseInt(amount), // Asegúrate de que amount es un número
            comments: comments || "Sin comentarios adicionales.",
            userId: localStorage.getItem('authId') ?? "",
            activityId: id ?? "",
        };

        try {
            const result = await reservationService.reserveActivityDate(formData);
            if (result.success) {
                Swal.fire("Éxito", "Reserva realizada con éxito, está pendiente a revisión por parte de un administrador.", "success");
            } else {
                Swal.fire("Fallo", "Ha ocurrido un error inesperado al intentar reservar, por favor intente de nuevo.", "error");
            }
        } catch (error) {
            console.error("Ha ocurrido un error al intentar reservar la actividad: ", error);
        }
    };

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

    const { formattedDate, formattedTime } = dateService.parseDate(activity.date);
    const isPastActivity = new Date(activity.date) < new Date();
    const availableSpots = activity.maximumCapacity - activity.currentCapacity;
    const comments = activity.comments.map((commentStr) => {
        const [username, ratingStr, comment] = commentStr.split(':');
        return { username, rating: parseInt(ratingStr), comment };
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

            <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center', gap: 2 }}>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={() => toPDF()}
                >
                    Exportar a PDF
                </Button>
            </Box>

            <BackgroundImage sx={{ backgroundImage: pattern1 }} />
            <BackgroundImage sx={{ backgroundImage: pattern2, opacity: 0.15 }} />
            <Container ref={targetRef} maxWidth="md" sx={{ zIndex: 1 }}>
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
                    <Typography variant="h6" sx={{ color: '#FFEA00', marginBottom: 1 }}>
                        🌐 Disponibilidad: {activity.isPublic}
                    </Typography>
                    <Typography variant="h6" sx={{ color: '#8a2be2' }}>
                        {isPastActivity ? '👥 Asistentes' : '👥 Capacidad'}: {activity.currentCapacity}/{activity.maximumCapacity}
                    </Typography>
                    {!isPastActivity && availableSpots > 0 && (
                        <>
                            <Typography variant="body1" sx={{ color: '#ff8c00', fontWeight: 'bold', marginTop: 2 }}>
                                ¡Quedan solo {availableSpots} plazas disponibles, reserva ahora!
                            </Typography>
                            <Button variant="contained" color="primary" sx={{ marginTop: 2 }} onClick={handleReservation}>
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
                    <Typography variant="body1">
                        Usuario:
                        <UserLink
                            username={activity.educatorUsername}
                            userId={activity.educatorId}
                        />
                    </Typography>
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
                    <Typography variant="h6" sx={{ color: '#32cd32' }}>
                        📚 Recursos
                    </Typography>
                    <Typography variant="body1">
                        {activity.resources.length > 0 ? activity.resources.join(', ') + '.' : 'No hay recursos disponibles.'}
                    </Typography>
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
                        <CommentsContainer comments={comments} invisible={true} />
                    </SectionBox>
                )}
            </Container>
        </Box>
    );
};

export default ActivityInfoPage;