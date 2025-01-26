import React, { useState, useEffect } from 'react';
import { Box, Typography, Grid2, Pagination } from '@mui/material';
import { styled } from '@mui/material/styles';
import { reservationService } from '../services/reservationService';
import { ListReservationResponse } from '../interfaces/Reservation';
import { SearchBar } from '../components/features/StyledSearchBar';
import ActivityLink from '../components/features/ActivityLink';
import { cacheService } from '../services/cacheService';

const MyReservationPage = () => {
    const reservation: ListReservationResponse = {
        result: [
            {
                activityId: '',
                activityName: '',
                comments: '',
                amount: 0,
                state: ''
            }
        ]
    }
    const [reservations, setReservations] = useState<ListReservationResponse>(reservation);
    const [currentPage, setCurrentPage] = useState(1);
    const [searchTerm, setSearchTerm] = useState('');
    const itemsPerPage = 6;

    useEffect(() => {
        const fetchReservations = async () => {
            try {
                const id = localStorage.getItem('authId') ?? "";
                const response = await reservationService.getAllReservations(id);
                setReservations(response);
            } catch (error) {
                console.error('Error obteniendo las reservas:', error);
            }
        };

        fetchReservations();
    }, []);

    const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(event.target.value);
        setCurrentPage(1);
    };

    const filteredReservations = reservations.result.filter(reservation =>
        reservation.activityName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        reservation.comments.toLowerCase().includes(searchTerm.toLowerCase()) ||
        reservation.amount.toString().includes(searchTerm) ||
        reservation.state.toLowerCase().includes(searchTerm.toLowerCase())
    );

    const paginatedReservations = filteredReservations.slice(
        (currentPage - 1) * itemsPerPage,
        currentPage * itemsPerPage
    );

    const totalPages = Math.ceil(filteredReservations.length / itemsPerPage);

    const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setCurrentPage(value);
    };

    // Cargar imágenes del caché
    const cachedImages = cacheService.loadImages();

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
            <Box sx={{ display: 'flex', mb: 3 }}>
                <SearchBar searchTerm={searchTerm} handleSearchChange={handleSearchChange} labelText="Reservas" />
            </Box>
            {paginatedReservations.length === 0 ? (
                <Typography variant="h4" sx={{ color: 'text.secondary', fontWeight: 'bold', mb: 2 }}>
                    📅 No hay reservas actualmente.
                </Typography>
            ) : (
                <Grid2 container spacing={2} justifyContent="center">
                    {paginatedReservations.map((reservation, index) => {
                        const activityImage = cachedImages[reservation.activityId] || ''; // Obtener la imagen de la actividad
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
    height: '200px',
    minWidth: '500px',
    minHeight: '200px',
    padding: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
    backgroundColor: 'rgba(255, 255, 255, 0.9)',
    boxShadow: theme.shadows[3],
    transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
    overflow: 'hidden',
    '&:hover': {
        boxShadow: theme.shadows[6],
        transform: 'scale(1.05)',
    },
}));

export default MyReservationPage;