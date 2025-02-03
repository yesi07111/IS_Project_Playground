import { useCallback, useEffect, useState } from "react";
import { ListOnlyActivityResponse, OnlyActivity } from "../interfaces/Activity";
import { activityService } from "../services/activityService";
import { Box, Button, Grid2, Pagination, Typography } from "@mui/material";
import { SearchBar } from "../components/features/StyledSearchBar";
import GenericCard from "../components/features/GenericCard";

const ActivitiesManagementPage = () => {
    const [activities, setActivities] = useState<OnlyActivity[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [searchTerm, setSearchTerm] = useState('');
    const itemsPerPage = 6;

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
            activity.recommendedAge.toLowerCase().includes(searchTerm.toLowerCase()) ||
            activity.itsPrivate.toLowerCase().includes(searchTerm.toLowerCase()) ||
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

    const handleCreateActivity =() =>{
       
    };

    const handleUpdateActivity =() =>{
       
    };
    
    const handleDeleteActivity =() =>{
       
    };

    const handleSeeActivityDate=() => {
        
    };

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

            <Box sx={{ display: 'flex', mb: 3 }}>
                <SearchBar searchTerm={searchTerm} handleSearchChange={handleSearchChange} labelText="Actividades" />
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
                <Grid2 container spacing={4} justifyContent="center">
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
                                { label: 'Privada', value: activity.itsPrivate },
                                { label: 'Instalación', value: activity.facilityName }
                            ]}
                            actions={[
                                {
                                    label: 'Modificar actividad',
                                    onClick: () => handleUpdateActivity(),
                                },
                                {
                                    label: 'Eliminar actividad permanentemente',
                                    onClick: () => handleDeleteActivity(),
                                },
                                {
                                    label: 'Ver Fechas',
                                    onClick: () => handleSeeActivityDate(),
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
        </Box>
    );
}

export default ActivitiesManagementPage;