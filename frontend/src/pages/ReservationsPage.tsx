import { useCallback, useEffect, useState } from "react";
import { usePDF } from 'react-to-pdf';
import { ListReservationResponse, ReservationDto } from "../interfaces/Reservation";
import { reservationService } from "../services/reservationService";
import { Box, Pagination, Typography, Button, Grid2 } from "@mui/material";
import { SearchBar } from "../components/features/StyledSearchBar";
import GenericCard from "../components/features/GenericCard";
import { useAuth } from "../components/auth/authContext";
import Swal from 'sweetalert2';

type ReservationState = 'Pendiente' | 'Confirmada' | 'Completada' | 'Cancelado';

const ReservationsManagementPage = () => {
    const { isAuthenticated } = useAuth();
    const myRole = localStorage.getItem('authUserRole');

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
                capacity: 0,
            }
        ]
    }
    const [reservations, setReservations] = useState<ListReservationResponse>(reservation);
    const [currentPage, setCurrentPage] = useState(1);
    const [searchTerm, setSearchTerm] = useState('');
    const itemsPerPage = 6;

    const { toPDF, targetRef } = usePDF({ filename: 'Reservaciones.pdf' });

    // eslint-disable-next-line react-hooks/exhaustive-deps
    const fetchReservations = async () => {
        try {
            const response = await reservationService.getAllReservations("");
            setReservations(response);
        } catch (error) {
            console.error('Error obteniendo las reservas:', error);
        }
    };

    const fetchInitialData = useCallback(async () => {
        await Promise.all([fetchReservations()]);
    }, [fetchReservations]);

    useEffect(() => {
        fetchInitialData();
    }, []);

    const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(event.target.value);
        setCurrentPage(1);
    };

    const filteredReservations = reservations.result
        .filter(reservation =>
        (
            reservation.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
            reservation.lastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
            reservation.userName.toLowerCase().includes(searchTerm.toLowerCase()) ||
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

    const handleConfirm = async (id: string) => {
        const result = await Swal.fire({
            title: '¿Estás seguro?',
            text: "¿Deseas confirmar esta reserva?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, confirmar',
            cancelButtonText: 'Cancelar'
        });

        if (result.isConfirmed) {
            const state = 'Confirmada';
            try {
                await reservationService.updateReservation({
                    reservationId: id,
                    state: state,
                });
                // Refresh reservations 
                const response = await reservationService.getAllReservations("");
                setReservations(response);
                Swal.fire('Confirmada', 'La reserva ha sido confirmada.', 'success');
            } catch (error) {
                console.error('Error confirmando la reservación:', error);
                Swal.fire('Error', 'Hubo un problema al confirmar la reserva.', 'error');
            }
        }
    };

    const handleDecline = async (id: string) => {
        const result = await Swal.fire({
            title: '¿Estás seguro?',
            text: "¿Deseas cancelar esta reserva?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, cancelar',
            cancelButtonText: 'Mantener'
        });

        if (result.isConfirmed) {
            const state = 'Cancelada';
            try {
                await reservationService.updateReservation({
                    reservationId: id,
                    state: state,
                });
                // Refresh reservations 
                const response = await reservationService.getAllReservations("");
                setReservations(response);
                Swal.fire('Cancelada', 'La reserva ha sido cancelada.', 'success');
            } catch (error) {
                console.error('Error cancelando la reservación:', error);
                Swal.fire('Error', 'Hubo un problema al cancelar la reserva.', 'error');
            }
        }
    };

    const handleDelete = async (id: string) => {
        const result = await Swal.fire({
            title: '¿Estás seguro?',
            text: "Esta acción es permanente y no se puede deshacer. ¿Deseas eliminar esta reserva?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        });

        if (result.isConfirmed) {
            try {
                await reservationService.deleteReservation(id);
                // Refresh reservations 
                const response = await reservationService.getAllReservations("");
                setReservations(response);
                Swal.fire('Eliminada', 'La reserva ha sido eliminada permanentemente.', 'success');
            } catch (error) {
                console.error('Error borrando la reservación:', error);
                Swal.fire('Error', 'Hubo un problema al eliminar la reserva.', 'error');
            }
        }
    };

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '100vh', minWidth: '100vw', bgcolor: 'background.default', p: 3, overflow: 'hidden' }}>

            <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center', gap: 2 }}>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={() => toPDF()}
                >
                    Exportar a PDF
                </Button>
            </Box>

            <Box sx={{ height: 16 }} />

            <Box sx={{ display: 'flex', mb: 3 }}>
                <SearchBar searchTerm={searchTerm} handleSearchChange={handleSearchChange} labelText="Reservas" />
            </Box>
            {paginatedReservations.length === 0 ? (
                <Typography variant="h4" sx={{ color: 'text.secondary', fontWeight: 'bold', mb: 2 }}>
                    📅 No hay reservas actualmente.
                </Typography>
            ) : (
                <Grid2 ref={targetRef} container spacing={4} justifyContent="center">
                    {paginatedReservations.map((reservation: ReservationDto) => (
                        <Grid2
                            size={{ xs: 12, sm: 6, md: 4 }}
                            key={reservation.reservationId}
                            sx={{
                                display: 'flex',
                                justifyContent: 'center',
                            }}
                        >
                            <GenericCard
                                title={`${reservation.firstName} ${reservation.lastName}`}
                                fields={[
                                    { label: 'Nombre de Usuario', value: reservation.userName },
                                    { label: 'Actividad', value: `${reservation.activityName} ${reservation.activityDate}` },
                                    { label: 'Comentarios', value: reservation.comments },
                                    { label: 'Cantidad de Niños', value: reservation.amount },
                                    { label: 'Capacidad actual', value: `${reservation.usedCapacity}/${reservation.capacity}` },
                                ]}
                                badge={{
                                    text: reservation.state,
                                    color:
                                        reservation.state === 'Completada'
                                            ? '#1976d2'
                                            : reservation.state === 'Confirmada'
                                                ? '#008000'
                                                : reservation.state === 'Pendiente'
                                                    ? '#ffa726'
                                                    : '#d32f2f',
                                }}
                                actions={myRole === 'Admin' && isAuthenticated ?
                                    [
                                        ...(reservation.state === 'Pendiente'
                                            ? [
                                                ...(reservation.usedCapacity + reservation.amount <= reservation.capacity
                                                    ? [
                                                        {
                                                            label: 'Confirmar',
                                                            onClick: () => handleConfirm(reservation.reservationId),
                                                        }
                                                    ]
                                                    : []
                                                ),
                                                {
                                                    label: 'Declinar',
                                                    onClick: () => handleDecline(reservation.reservationId),
                                                }
                                            ]
                                            : []
                                        ),
                                        ...(reservation.state === 'Confirmada'
                                            ? [
                                                {
                                                    label: 'Cancelar',
                                                    onClick: () => handleDecline(reservation.reservationId),
                                                }
                                            ]
                                            : []
                                        ),
                                        ...(reservation.state === 'Cancelada'
                                            ? [
                                                ...(reservation.usedCapacity + reservation.amount <= reservation.capacity
                                                    ? [
                                                        {
                                                            label: 'Confirmar',
                                                            onClick: () => handleConfirm(reservation.reservationId),
                                                        }
                                                    ]
                                                    : []
                                                )
                                            ]
                                            : []
                                        ),
                                        ...(reservation.state === 'Completada' || reservation.state === 'Cancelada'
                                            ? [
                                                {
                                                    label: 'Eliminar reserva permanentemente',
                                                    onClick: () => handleDelete(reservation.reservationId),
                                                },
                                            ]
                                            : []
                                        ),
                                    ]
                                    : []
                                }
                            />
                        </Grid2>
                    ))}
                </Grid2>
            )}
            <Box sx={{ mt: 3, display: 'flex', justifyContent: 'center' }}>
                <Pagination count={totalPages} page={currentPage} onChange={handlePageChange} color="primary" />
            </Box>
        </Box>
    );
};

export default ReservationsManagementPage;