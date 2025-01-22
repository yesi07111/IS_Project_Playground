import React, { useState, useEffect } from 'react';
import { Box, Typography } from '@mui/material';
import { styled } from '@mui/material/styles';
import { reservationService } from '../services/reservationService';
import { ListReservationResponse } from '../interfaces/Reservation';

const ReservationsPage = () => {
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

    useEffect(() => {
        const fetchReservations = async () => {
            try {
                const id = localStorage.getItem('authId') ?? "";
                const response = await reservationService.getAllReservations(id);
                console.log("Reservas:")
                console.table(response)
                setReservations(response);
                console.table(reservations)
            } catch (error) {
                console.error('Error obteniendo las reservas:', error);
            }
        };

        fetchReservations();
    }, []);

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
            {reservations.result && reservations.result.length === 0 ? (
                <Typography variant="h4" sx={{ color: 'text.secondary' }}>
                    No hay reservas actualmente.
                </Typography>
            ) : (
                reservations.result && reservations.result.map((reservation, index) => (
                    <StyledBox key={index}>
                        <Typography variant="h6">Actividad Asociada: {reservation.activityName}</Typography>
                        <Typography variant="body1">Comentarios Adicionales: {reservation.comments}</Typography>
                        <Typography variant="body1">Cantidad de niños: {reservation.amount}</Typography>
                        <Typography variant="body1">Estado: {reservation.state}</Typography>
                    </StyledBox>
                ))
            )}
        </Box>
    );
};

const StyledBox = styled(Box)(({ theme }) => ({
    width: '80%',
    maxWidth: '600px',
    padding: theme.spacing(2),
    marginBottom: theme.spacing(2),
    borderRadius: theme.shape.borderRadius,
    backgroundColor: 'rgba(255, 255, 255, 0.8)',
    boxShadow: theme.shadows[1],
    transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
    '&:hover': {
        boxShadow: theme.shadows[6],
        transform: 'scale(1.05)',
    },
}));

export default ReservationsPage;