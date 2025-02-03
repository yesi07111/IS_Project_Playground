import { useCallback, useEffect, useState } from "react";
import { ActivityDate } from "../interfaces/Activity";
import { activityService } from "../services/activityService";
import { useNavigate, useSearchParams } from "react-router-dom";
import { Box, Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Grid2, Pagination, Typography } from "@mui/material";
import GenericCard from "../components/features/GenericCard";

const ActivityDates = () => {
    const [activities, setActivities] = useState<ActivityDate[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 6;
    const [searchParams] = useSearchParams();
    const id = searchParams.get('activityId');
    const [selectedActivity, setSelectedActivity] = useState<string | null>(null);
    const [openDialog, setOpenDialog] = useState(false);
    const navigate = useNavigate();

    const fetchActivities = async () => {
        try {
            const response = await activityService.getAllActivityDates(id || '');
            const activitiesArray: ActivityDate[] = Array.isArray(response.result)
                ? response.result as ActivityDate[]
                : [];

            console.table(activitiesArray);
            setActivities(activitiesArray);
        } catch (error) {
            console.error('Error obteniendo las reservas:', error);
        }
    };

    const fetchInitialData = useCallback(async () => {
        await Promise.all([fetchActivities()]);
    }, [fetchActivities]);

    useEffect(() => {
        fetchInitialData();
    }, []);

    const paginatedActivities = activities.slice(
        (currentPage - 1) * itemsPerPage,
        currentPage * itemsPerPage
    );

    const totalPages = Math.ceil(activities.length / itemsPerPage);

    const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setCurrentPage(value);
    };

    const handleDeleteActivity = (id: string) => {
        setSelectedActivity(id); // Guardar datos relevantes
        setOpenDialog(true); // Abrir diálogo de confirmación
    }

    const confirmRemoveActivity = async () => {
        if (selectedActivity) {
            await activityService.deleteActivity('DeleteActivityDate', '', selectedActivity);
            setOpenDialog(false); // Cerrar el diálogo
            setSelectedActivity(null); // Limpiar selección
            // Refresh act 
            const response = await activityService.getAllActivityDates(id || '');
            const activitiesArray: ActivityDate[] = Array.isArray(response.result)
                ? response.result as ActivityDate[]
                : [];

            console.table(activitiesArray);
            setActivities(activitiesArray);
        }
    };

    const cancelRemoveActivity = () => {
        setOpenDialog(false); // Cerrar el diálogo sin hacer nada
        setSelectedActivity(null); // Limpiar selección
    };

    const handleCreateActivity = (id: string) => {
        console.log(id);
        navigate(`/activity-form?useCase=${'CreateActivityDate'}&activityId=${id}`);
    }

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '100vh', minWidth: '100vw', bgcolor: 'background.default', p: 3 }}>

            {/* <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center', gap: 2 }}>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={() => toPDF()}
                >
                    Exportar a PDF
                </Button>
            </Box> */}

            <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center', gap: 2 }}>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={() => handleCreateActivity(id || '')}
                >
                    Crear Actividad
                </Button>
            </Box>
            <Box sx={{ height: 16 }} />
            {paginatedActivities.length === 0 ? (
                <Typography variant="h4" sx={{ color: 'text.secondary', fontWeight: 'bold', mb: 2 }}>
                    📅 No hay actividades actualmente.
                </Typography>
            ) : (
                <Grid2 container spacing={4} justifyContent="center">
                    {paginatedActivities.map((activity: ActivityDate) => (
                        <Grid2
                            size={{ xs: 12, sm: 6, md: 4 }}
                            key={activity.id}
                            sx={{
                                display: 'flex',
                                justifyContent: 'center',
                            }}
                        >
                            {/* <ResourceCard resource={resource} /> */}
                            <GenericCard
                                title={activity.dateTime.toString()}
                                fields={[
                                    { label: 'Pendiente', value: activity.pending ? 'Sí' : 'No' }
                                ]}
                                actions={[
                                    {
                                        label: 'Eliminar actividad permanentemente',
                                        onClick: () => handleDeleteActivity(activity.id),
                                    },
                                ]}
                            />
                        </Grid2>
                    ))}
                </Grid2>
            )}
            <Box sx={{ mt: 3, display: 'flex', justifyContent: 'center' }}>
                <Pagination count={totalPages} page={currentPage} onChange={handlePageChange} color="primary" />
            </Box>

            {/* Diálogo de confirmación de eliminar recurso */}
            <Dialog open={openDialog} onClose={cancelRemoveActivity}>
                <DialogTitle>Confirmación</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        ¿Estás seguro de que quieres eliminar esta actividad PERMANENTEMENTE?
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={cancelRemoveActivity} color="primary">
                        No
                    </Button>
                    <Button onClick={confirmRemoveActivity} color="primary" autoFocus>
                        Sí
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}

export default ActivityDates;