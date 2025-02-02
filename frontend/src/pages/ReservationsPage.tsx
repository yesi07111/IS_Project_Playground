import { useCallback, useEffect, useState } from "react";
import { ListReservationResponse, ReservationDto } from "../interfaces/Reservation";
import { reservationService } from "../services/reservationService";
import { Box, Grid, Pagination, Typography, Button, Grid2 } from "@mui/material";
import { SearchBar } from "../components/features/StyledSearchBar";
import GenericCard from "../components/features/GenericCard";
import { useAuth } from "../components/auth/authContext";

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
            }
        ]
    }
    const [reservations, setReservations] = useState<ListReservationResponse>(reservation);
    const [currentPage, setCurrentPage] = useState(1);
    const [searchTerm, setSearchTerm] = useState('');
    const itemsPerPage = 6;

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
        const state = 'Confirmada';
        try {
            await reservationService.updateReservation({
                reservationId: id,
                state: state,
            })
            // Refresh reservations 
            const response = await reservationService.getAllReservations("");
            setReservations(response);
        }
        catch (error) {
            console.error('Error confirmando la reservación:', error);
        }
    };

    const handleDecline = async (id: string) => {
        const state = 'Cancelada';
        try {
            await reservationService.updateReservation({
                reservationId: id,
                state: state,
            })
            // Refresh reservations 
            const response = await reservationService.getAllReservations("");
            setReservations(response);
        }
        catch (error) {
            console.error('Error confirmando la reservación:', error);
        }
    };

    const handleDelete = async (id: string) => {
        try {
            await reservationService.deleteReservation(id)
            // Refresh reservations 
            const response = await reservationService.getAllReservations("");
            setReservations(response);
        }
        catch (error) {
            console.error('Error borrando la reservación:', error);
        }
    };

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '100vh', minWidth: '100vw', bgcolor: 'background.default', p: 3 }}>
            <Box sx={{ display: 'flex', mb: 3 }}>
                <SearchBar searchTerm={searchTerm} handleSearchChange={handleSearchChange} labelText="Reservas" />
            </Box>
            {paginatedReservations.length === 0 ? (
                <Typography variant="h4" sx={{ color: 'text.secondary', fontWeight: 'bold', mb: 2 }}>
                    📅 No hay reservas actualmente.
                </Typography>
            ) : (
                <Grid2 container spacing={4} justifyContent="center">
                    {paginatedReservations.map((reservation: ReservationDto) => (
                        <Grid2
                            size={{ xs: 12, sm: 6, md: 4 }}
                            key={reservation.reservationId}
                            sx={{
                                display: 'flex',
                                justifyContent: 'center',
                            }}
                        >
                            {/* <ResourceCard resource={resource} /> */}
                            <GenericCard
                                title={`${reservation.firstName} ${reservation.lastName}`}
                                fields={[
                                    { label: 'Nombre de Usuario', value: reservation.userName },
                                    { label: 'Actividad', value: `${reservation.activityName} ${reservation.activityDate}` },
                                    { label: 'Comentarios', value: reservation.comments },
                                    { label: 'Cantidad de Niños', value: reservation.amount },
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
                                                {
                                                    label: 'Confirmar',
                                                    onClick: () => handleConfirm(reservation.reservationId),
                                                },
                                                {
                                                    label: 'Declinar',
                                                    onClick: () => handleDecline(reservation.reservationId),
                                                },
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