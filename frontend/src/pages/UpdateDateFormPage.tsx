import { useState } from "react";
import { useSearchParams } from "react-router-dom";
import { activityService } from "../services/activityService";
import { Alert, Box, Button, Grid, Paper, Typography } from "@mui/material";
import { DatePicker, LocalizationProvider, TimePicker } from "@mui/x-date-pickers";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { es } from "date-fns/locale";


const UpdateDateFormPage: React.FC = () => {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState(false);
    const [searchParams] = useSearchParams();
    const useCase = searchParams.get('useCase');
    const activityId = searchParams.get('activityId');
    const dateTime = searchParams.get('dateTime');
    const id = searchParams.get('id');

    const parsedDateTime = dateTime ? new Date(dateTime) : null;

    const [formData, setFormData] = useState({
        day: parsedDateTime || null as Date | null,
        time: parsedDateTime || null as Date | null
    });

    const handleDateChange = (newValue: Date | null) => {
        setFormData({
            ...formData,
            day: newValue,
        });
    };

    const handleTimeChange = (newValue: Date | null) => {
        setFormData({
            ...formData,
            time: newValue,
        });
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setError('');
        setSuccess(false);

        try {
            const result = await activityService.updateActivity({
                useCase: useCase || '',
                activityId: activityId || '',
                activityDateId: id || '',
                name: '',
                description: '',
                educator: '',
                type: '',
                date: formData.day ? formData.day : new Date,
                time: formData.time ? formData.time : new Date,
                recommendedAge: 2,
                facility: '',
                pending: false,
                private: false
            });
            setSuccess(true);
        }
        catch (error) {
            setError('Hubo un error al modificar la fecha y hora');
        }
    };

    return (
        <Box
            sx={{
                minHeight: '100vh',
                minWidth: '100vw',
                width: '100%',
                height: '100%',
                display: 'flex',
                backgroundColor: '#f8f9fa',
                position: 'relative',
                overflow: 'auto',
                margin: 0,
                padding: 0,
                boxSizing: 'border-box',
            }}
        >
            <Paper
                elevation={3}
                sx={{
                    flex: 1,
                    padding: 4,
                    margin: 'auto',
                    maxWidth: '600px'
                }}
            >
                <Typography variant="h4" component="h1" gutterBottom>
                    Modificar Fecha y Hora
                </Typography>

                {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                {success && <Alert severity="success" sx={{ mb: 2 }}>Fecha y hora modificada con éxito.</Alert>}

                <Box component="form" onSubmit={handleSubmit}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={es}>
                                <Typography variant="body2" color="text.secondary">
                                    Día de la actividad
                                </Typography>
                                <DatePicker
                                    value={formData.day}
                                    onChange={handleDateChange}
                                    slotProps={{
                                        textField: {
                                            sx: { ml: 2 },
                                        },
                                    }}
                                />
                            </LocalizationProvider>
                        </Grid>
                        <Grid item xs={12}>
                            <LocalizationProvider dateAdapter={AdapterDateFns}>
                                <Typography variant="body2" color="text.secondary">
                                    Hora de la actividad
                                </Typography>
                                <TimePicker
                                    value={formData.time}
                                    onChange={handleTimeChange}
                                />
                            </LocalizationProvider>
                        </Grid>
                        <Grid item xs={12}>
                            <Button
                                type="submit"
                                fullWidth
                                variant="contained"
                                color="primary"
                                sx={{ mt: 2 }}
                            >
                                Modificar 
                            </Button>
                        </Grid>
                    </Grid>
                </Box>
            </Paper>
        </Box>
    );
}

export default UpdateDateFormPage;