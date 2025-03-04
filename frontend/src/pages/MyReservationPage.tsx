import React, { useState, useEffect } from 'react';
import { usePDF } from 'react-to-pdf';
import { Box, Typography, Pagination, Grid2, Button } from '@mui/material';
import { styled } from '@mui/material/styles';
import { reservationService } from '../services/reservationService';
import { ListReservationResponse } from '../interfaces/Reservation';
import { SearchBar } from '../components/features/StyledSearchBar';
import ActivityLink from '../components/features/ActivityLink';
import { cacheService } from '../services/cacheService';
import Swal from 'sweetalert2';
import CustomButton from '../components/features/StyledButton';
import { activityService } from '../services/activityService';
import { Activity } from '../interfaces/Activity';

// Define un tipo para los estados de las reservas, incluyendo "Cancelado"
type ReservationState = 'Pendiente' | 'Confirmada' | 'Completada' | 'Cancelado';

const MyReservationPage = () => {
    const reservation: ListReservationResponse = {
        result: [
            {
                reservationId: '',
                firstName: '',
                lastName: '',
                userName: '',
                activityId: '',
                activityName: '',
                activityDate: '',
                comments: '',
                amount: 0,
                state: '' as ReservationState,
                activityRecommendedAge: 0,
                usedCapacity: 0,
                capacity: 0
            }
        ]
    }
    const [reservations, setReservations] = useState<ListReservationResponse>(reservation);
    const [currentPage, setCurrentPage] = useState(1);
    const [searchTerm, setSearchTerm] = useState('');
    const [activityImages, setActivityImages] = useState<{ [id: string]: string }>({});
    const itemsPerPage = 6;

    const { toPDF, targetRef } = usePDF({ filename: 'MisReservas.pdf' });

    useEffect(() => {
        const fetchReservations = async () => {
            try {
                const id = localStorage.getItem('authId') ?? "";
                const response = await reservationService.getAllReservations(id);
                setReservations(response);

                // Obtener IDs de actividades de las reservas
                const activityIds = response.result.map(reservation => reservation.activityId);
                fetchActivityImages(activityIds);
            } catch (error) {
                console.error('Error obteniendo las reservas:', error);
            }
        };

        const fetchActivityImages = async (activityIds: string[]) => {
            const cachedImages = cacheService.loadImages();
            const cachedActivities = cacheService.loadActivities();
            const missingIds = activityIds.filter(id => !cachedImages[id]);

            if (missingIds.length > 0) {
                try {
                    // Obtener actividades para ambos casos de uso
                    const responseReviewView = await activityService.getAllActivities([
                        { type: 'Casos de Uso', useCase: 'ReviewView' }
                    ]);
                    console.log("REVIEW VIEW RESPONSE")
                    console.table(responseReviewView)
                    const responseActivityView = await activityService.getAllActivities([
                        { type: 'Casos de Uso', useCase: 'ActivityView' }
                    ]);
                    console.log("ACTIVITY VIEW RESPONSE")
                    console.table(responseActivityView)
                    const activitiesArray = [
                        ...(Array.isArray(responseReviewView.result) ? responseReviewView.result as Activity[] : []),
                        ...(Array.isArray(responseActivityView.result) ? responseActivityView.result as Activity[] : [])
                    ];

                    const newImagesMap: { [id: string]: string } = {};
                    const newActivitiesMap: { [id: string]: Activity } = {};

                    activitiesArray.forEach(activity => {
                        if (missingIds.includes(activity.id)) {
                            newImagesMap[activity.id] = activity.image || ''; // Asignar imagen si existe
                            newActivitiesMap[activity.id] = activity;
                        }
                    });

                    const updatedImages = { ...cachedImages, ...newImagesMap };
                    const updatedActivities = [...cachedActivities, ...Object.values(newActivitiesMap)].filter((activity, index, self) =>
                        index === self.findIndex((a) => a.id === activity.id)
                    );

                    cacheService.saveImages(updatedImages);
                    cacheService.saveActivities(updatedActivities);
                    setActivityImages(updatedImages);
                } catch (error) {
                    console.error('Error obteniendo imágenes de actividades:', error);
                }
            } else {
                setActivityImages(cachedImages);
            }
        };

        fetchReservations();
        fetchActivityImages(["0"]);
    }, []);

    const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(event.target.value);
        setCurrentPage(1);
    };

    const filteredReservations = reservations.result
        .filter(reservation =>
        (
            reservation.activityName.toLowerCase().includes(searchTerm.toLowerCase()) ||
            reservation.comments.toLowerCase().includes(searchTerm.toLowerCase()) ||
            reservation.amount.toString().includes(searchTerm) ||
            reservation.state.toLowerCase().includes(searchTerm.toLowerCase())
        )
        )
        .sort((a, b) => {
            const order: Record<ReservationState, number> = { 'Pendiente': 1, 'Confirmada': 2, 'Completada': 3, 'Cancelado': 4 };
            return order[a.state as ReservationState] - order[b.state as ReservationState];
        });

    const paginatedReservations = filteredReservations.slice(
        (currentPage - 1) * itemsPerPage,
        currentPage * itemsPerPage
    );

    const totalPages = Math.ceil(filteredReservations.length / itemsPerPage);

    const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setCurrentPage(value);
    };

    const handleCancelReservation = async (activityId: string) => {
        const userId = localStorage.getItem('authId') ?? "";
        const result = await Swal.fire({
            title: '¿Estás seguro?',
            text: "No podrás revertir esto!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, cancelar reserva!'
        });

        if (result.isConfirmed) {
            try {
                const cancelResponse = await reservationService.cancelReservation(activityId, userId);

                if (cancelResponse) {
                    Swal.fire(
                        'Cancelada!',
                        'Tu reserva ha sido cancelada.',
                        'success'
                    );

                    // Refresh reservations after cancellation
                    const response = await reservationService.getAllReservations(userId);
                    setReservations(response);
                }
                else {
                    Swal.fire(
                        'Error!',
                        'Hubo un problema al cancelar la reserva, por favor intente de nuevo.',
                        'error'
                    );
                }
            } catch (error) {
                console.error('Error cancelando la reserva:', error);
                Swal.fire(
                    'Error!',
                    'Hubo un problema al cancelar la reserva.',
                    'error'
                );
            }
        }
    };

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'center',
                minHeight: '100vh',
                minWidth: '100vw',
                bgcolor: 'background.default',
                p: 3,
            }}
        >
            <Button
                variant="contained"
                color="primary"
                onClick={() => toPDF()}
            >
                Exportar a PDF
            </Button>

            <Box sx={{ height: 16 }} />

            <Box sx={{ display: 'flex', mb: 3 }}>
                <SearchBar searchTerm={searchTerm} handleSearchChange={handleSearchChange} labelText="Reservas" />
            </Box>
            {paginatedReservations.length === 0 ? (
                <Typography variant="h4" sx={{ color: 'text.secondary', fontWeight: 'bold', mb: 2 }}>
                    📅 No hay reservas actualmente.
                </Typography>
            ) : (
                <Grid2 ref={targetRef} container spacing={2} justifyContent="center">
                    {paginatedReservations.map((reservation, index) => {
                        let activityImage = activityImages[reservation.activityId] || ''; // Obtener la imagen de la actividad
                        if (activityImage == '') {
                            activityImage = cacheService.defaultImage();
                        }
                        const viewSuffix = (reservation.state === 'Pendiente' || reservation.state === 'Confirmada') ? 'ActivityView' : 'ReviewView';
                        return (
                            <Grid2
                                component="div"
                                key={index}
                                sx={{
                                    width: { xs: '100%', sm: '50%', md: '33.33%' }, // Responsive width
                                    display: 'flex',
                                    justifyContent: 'center',
                                }}
                            >
                                <StyledBox>
                                    <ActivityLink id={reservation.activityId} image={activityImage} viewSuffix={viewSuffix} fontSize="1.5rem" textDisplayed={`🎉 Actividad: ${reservation.activityName}`} underline={false} />
                                    <Typography variant="body1" sx={{ color: 'text.primary', mb: 1 }}>
                                        📝 Comentarios Adicionales: {reservation.comments}
                                    </Typography>
                                    <Typography variant="body1" sx={{ color: 'text.primary', mb: 1 }}>
                                        👶 Cantidad de niños: {reservation.amount}
                                    </Typography>
                                    <Typography variant="body1" sx={{ color: 'text.primary', mb: 1 }}>
                                        📌 Estado: {reservation.state}
                                    </Typography>
                                    {(reservation.state === 'Pendiente' || reservation.state === 'Confirmada') && (
                                        <CustomButton
                                            variant="contained"
                                            color="primary" // Cambia el color a azul
                                            onClick={() => handleCancelReservation(reservation.activityId)}
                                        >
                                            Cancelar Reserva
                                        </CustomButton>
                                    )}
                                </StyledBox>
                            </Grid2>
                        );
                    })}
                </Grid2>
            )}
            <Box sx={{ mt: 3, display: 'flex', justifyContent: 'center' }}>
                <Pagination
                    count={totalPages}
                    page={currentPage}
                    onChange={handlePageChange}
                    color="primary"
                />
            </Box>
        </Box>
    );
};

const StyledBox = styled(Box)(({ theme }) => ({
    width: '100%',
    height: '280px',
    minWidth: '500px',
    minHeight: '200px',
    padding: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
    backgroundColor: 'rgba(255, 255, 255, 0.9)',
    boxShadow: theme.shadows[3],
    transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
    overflow: 'hidden', // Asegura que el contenido no sobresalga
    '&:hover': {
        boxShadow: theme.shadows[6],
        transform: 'scale(1.05)',
    },
}));

export default MyReservationPage;