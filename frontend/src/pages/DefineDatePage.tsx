import React, { useEffect, useState } from 'react';
import {
    Box,
    Container,
    Paper,
    Typography,
    TextField,
    Button,
    Alert,
    Card,
    Theme,
    CardContent,
} from '@mui/material';
import { useParams, useSearchParams } from 'react-router-dom';
import { DatePicker, LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { resourceService } from '../services/resourceService';
import { FieldErrors } from '../interfaces/Error';

const DefineDatePage: React.FC = () => {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState(false);
    const [selectedDate, setSelectedDate] = useState<Date | null>(null);
    const [fieldErrors, setFieldErrors] = useState<FieldErrors>({
        statusCode: 0,
        message: '',
        errors: {}
    });

    const { resourceId } = useParams(); // Extrae el parámetro de la URL

    const handleConfirm = async () => {
        setError('');
        setFieldErrors({
            statusCode: 0,
            message: '',
            errors: {}
        });
        setSuccess(false);

        if (!selectedDate) {
            setError('Por favor selecciona una fecha antes de continuar.');
            return;
        }

        if (!resourceId) {
            setError('ID del recurso no encontrado.');
            return;
        }


        try {
            const response = await resourceService.removeResourceDate(resourceId, selectedDate);
            setSuccess(true);
        }
        catch (err: any) {
            const apiError = err as FieldErrors;

            if (apiError && apiError.errors) {
                const errorData = apiError.errors;

                setFieldErrors({
                    statusCode: apiError.statusCode || 400,
                    message: apiError.message || 'Ocurrieron errores de validación.',
                    errors: errorData
                });
            }
            else {
                setError('Error al eliminar la frecuencia de uso. Por favor, inténtalo nuevamente.');
            }
        }
    };

    return (
        <LocalizationProvider dateAdapter={AdapterDateFns}>
            <Box
                sx={{
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    width: '100vw',
                    height: '100vh',
                    backgroundColor: '#f5f5f5', // Fondo opcional
                }}
            >
                <Card
                    sx={{
                        width: '90%',
                        maxWidth: 400,
                        display: 'flex',
                        flexDirection: 'column',
                        transition: 'transform 0.2s',
                        '&:hover': {
                            transform: 'scale(1.03)',
                            boxShadow: (theme: Theme) => theme.shadows[3],
                        },
                    }}
                >
                    <CardContent>
                        <Typography
                            gutterBottom
                            variant="h5"
                            component="div"
                            sx={{
                                fontWeight: 'bold',
                                color: 'primary.main',
                            }}
                        >
                            Elegir fecha de la frecuencia de uso que se desea eliminar del recurso
                        </Typography>

                        {/* Mensaje de éxito */}
                        {success && <Alert severity="success" sx={{ mb: 2 }}>Frecuencia de uso eliminada con éxito.</Alert>}

                        {/* Mensaje de error general */}
                        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

                        {/* Errores de validación específicos de la API */}
                        {Object.keys(fieldErrors.errors).map((field) => (
                            <Alert severity="error" key={field} sx={{ mb: 2 }}>
                                {`${fieldErrors.errors[field].join(', ')}`}
                            </Alert>
                        ))}

                        <Box sx={{ mt: 2 }}>
                            <Box sx={{ mt: 2 }}>
                                <DatePicker
                                    value={selectedDate}
                                    onChange={(newValue: Date | null) => setSelectedDate(newValue)}
                                    slotProps={{
                                        textField: {
                                            sx: { ml: 2 },
                                        },
                                    }}
                                />
                            </Box>
                        </Box>
                        <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center' }}>
                            <Button
                                variant="contained"
                                color="primary"
                                onClick={handleConfirm}
                            >
                                Confirmar
                            </Button>
                        </Box>
                    </CardContent>
                </Card>
            </Box>
        </LocalizationProvider>
    );
}

export default DefineDatePage;