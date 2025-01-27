import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, Typography, Box, Theme, Button, TextField } from '@mui/material';
import { DatePicker, LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { useParams } from 'react-router-dom';
import { resourceService } from '../services/resourceService';
import { Message } from '@mui/icons-material';
import { ResourceDate } from '../interfaces/ResourceDate';

const DefineUsageFrequencyPage = () => {
    const { resourceId } = useParams(); // Extrae el parámetro de la URL
    const navigate = useNavigate();
    const [selectedDate, setSelectedDate] = useState<Date | null>(null);
    const [selectedUsageFrequency, setSelectedUsageFrequency] = useState<number | ''>('');

    console.log('Resource ID:', resourceId);

    const handleConfirm = async () => {
        if (selectedDate && selectedUsageFrequency !== "" && resourceId !== undefined) {
            try {
                console.log('Selected date:', selectedDate);
                console.log('Usage frequency:', selectedUsageFrequency);

                const data: ResourceDate = {
                    resourceId: resourceId,
                    date: selectedDate,
                    usageFrequency: selectedUsageFrequency
                };

                console.table(data);

                const response = await resourceService.saveUsageFrequency(data);
                alert(JSON.stringify(response));
                navigate('/resources');
            }
            catch (error) {
                console.error('Error posting usage frequency:', error);
            }
        } else {
            alert('Por favor selecciona una fecha y define una frecuencia de uso antes de continuar.');
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
                            Elegir Frecuencia de Uso
                        </Typography>
                        <Box sx={{ mt: 2 }}>
                            <Typography variant="body2" color="text.secondary">
                                Selecciona la fecha para definir la frecuencia de uso:
                            </Typography>
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
                        <Box sx={{ mt: 2 }}>
                            <Typography variant="body2" color="text.secondary">
                                Define la frecuencia de uso:
                            </Typography>
                            <TextField
                                type="number"
                                value={selectedUsageFrequency}
                                onChange={(e) => setSelectedUsageFrequency(e.target.value ? parseInt(e.target.value, 10) : "")}
                                sx={{ mt: 1, width: '100%' }}
                                placeholder="Escribe un número"
                                inputProps={{ min: 0 }}
                            />
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
};

export default DefineUsageFrequencyPage;
