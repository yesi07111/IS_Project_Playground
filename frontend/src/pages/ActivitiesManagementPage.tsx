import { useCallback, useEffect, useState } from "react";
import { ListOnlyActivityResponse, OnlyActivity } from "../interfaces/Activity";
import { activityService } from "../services/activityService";
import { Box, Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Grid2, Pagination, Typography } from "@mui/material";
import { SearchBar } from "../components/features/StyledSearchBar";
import GenericCard from "../components/features/GenericCard";
import { useNavigate } from "react-router-dom";
import { usePDF } from "react-to-pdf";

const ActivitiesManagementPage = () => {
    const [activities, setActivities] = useState<OnlyActivity[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedActivity, setSelectedActivity] = useState<string | null>(null);
    const [openDialog, setOpenDialog] = useState(false);
    const navigate = useNavigate();
    const itemsPerPage = 6;
    const { toPDF, targetRef } = usePDF({ filename: 'Actividades.pdf' });

    const fetchActivities = async () => {
        try {
            const response = await activityService.getAllOnlyActivities();
            const activitiesArray: OnlyActivity[] = Array.isArray(response.result)
                ? response.result as OnlyActivity[]
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

    const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(event.target.value);
        setCurrentPage(1);
    };

    const filteredActivities = activities.filter(activity => (
        activity.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        activity.description.toLowerCase().includes(searchTerm.toLowerCase()) ||
        activity.educatorFirstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        activity.educatorLastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        activity.educatorUserName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        activity.type.toString().includes(searchTerm) ||
        activity.recommendedAge.toString().includes(searchTerm.toLowerCase()) ||
        activity.itsPrivate.toString().includes(searchTerm) ||
        activity.facilityName.toLowerCase().includes(searchTerm.toLowerCase())
    ));

    const paginatedActivities = filteredActivities.slice(
        (currentPage - 1) * itemsPerPage,
        currentPage * itemsPerPage
    );

    const totalPages = Math.ceil(filteredActivities.length / itemsPerPage);

    const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setCurrentPage(value);
    };

    const handleCreateActivity = () => {
        navigate(`/activity-form?useCase=${'CreateActivity'}`);
    };

    const handleUpdateActivity = (id: string, name: string, description: string, educatorFirstName: string, educatorLastName: string, educatorUserName: string, type: string, recommendedAge: string, itsPrivate: boolean, facilityName: string) => {
        navigate(`/updateActivity?useCase=${'UpdateActivity'}&activityId=${id}&name=${name}&description=${description}&educFN=${educatorFirstName}&educLN=${educatorLastName}&educUN=${educatorUserName}&type=${type}&recAge=${recommendedAge}&itsPrivate=${itsPrivate}&facilityName=${facilityName}`);
    };

    const handleDeleteActivity = (id: string) => {
        setSelectedActivity(id); // Guardar datos relevantes
        setOpenDialog(true); // Abrir diálogo de confirmación
    };

    const confirmRemoveActivity = async () => {
        if (selectedActivity) {
            await activityService.deleteActivity('DeleteActivity', selectedActivity, '');
            setOpenDialog(false); // Cerrar el diálogo
            setSelectedActivity(null); // Limpiar selección
            // Refresh act 
            const response = await activityService.getAllOnlyActivities();
            const activitiesArray: OnlyActivity[] = Array.isArray(response.result)
                ? response.result as OnlyActivity[]
                : [];

            console.table(activitiesArray);
            setActivities(activitiesArray);
        }
    };

    const cancelRemoveActivity = () => {
        setOpenDialog(false); // Cerrar el diálogo sin hacer nada
        setSelectedActivity(null); // Limpiar selección
    };

    const handleSeeActivityDate = (id: string) => {
        navigate(`/activityDates?activityId=${id}`)
    };

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '100vh', minWidth: '100vw', bgcolor: 'background.default', p: 3, overflowY: 'hidden', }}>
            <Box sx={{ display: 'flex', mb: 3 }}>
                <SearchBar searchTerm={searchTerm} handleSearchChange={handleSearchChange} labelText="Actividades" />
            </Box>
            <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center', gap: 2 }}>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={() => toPDF()}
                >
                    Exportar a PDF
                </Button>
            </Box>
            <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center', gap: 2 }}>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={() => handleCreateActivity()}
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
                <Grid2 ref={targetRef} container spacing={4} justifyContent="center">
                    {paginatedActivities.map((activity: OnlyActivity) => (
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
                                title={activity.name}
                                fields={[
                                    { label: 'Descripción', value: activity.description },
                                    { label: 'Educador Encargado', value: `${activity.educatorFirstName} ${activity.educatorLastName} (${activity.educatorUserName})` },
                                    { label: 'Tipo', value: activity.type },
                                    { label: 'Edad Recomendada', value: activity.recommendedAge },
                                    { label: 'Privada', value: activity.itsPrivate ? 'Sí' : 'No' },
                                    { label: 'Instalación', value: activity.facilityName }
                                ]}
                                actions={[
                                    {
                                        label: 'Modificar actividad',
                                        onClick: () => handleUpdateActivity(activity.id, activity.name, activity.description, activity.educatorFirstName, activity.educatorLastName, activity.educatorUserName, activity.type, activity.recommendedAge, activity.itsPrivate, activity.facilityName),
                                    },
                                    {
                                        label: 'Eliminar actividad permanentemente',
                                        onClick: () => handleDeleteActivity(activity.id),
                                    },
                                    {
                                        label: 'Ver Fechas',
                                        onClick: () => handleSeeActivityDate(activity.id),
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

export default ActivitiesManagementPage;