import React, { useCallback, useEffect, useState } from 'react';
import {
    Box,
    Paper,
    TextField,
    Button,
    Typography,
    Checkbox,
    FormControlLabel,
    Alert,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    SelectChangeEvent,
    Grid,
} from '@mui/material';
import { LocalizationProvider, DatePicker, TimePicker } from '@mui/x-date-pickers';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { facilityService } from '../services/facilityService';
import { cacheService } from '../services/cacheService';
import { activityService } from '../services/activityService';
import { FacilityResponse } from '../interfaces/Facility';
import { userService } from '../services/userService';
import { useSearchParams } from 'react-router-dom';
import { FieldErrors } from '../interfaces/Error';
import { es } from 'date-fns/locale';

const ActivityFormPage: React.FC = () => {
    const educatorId = localStorage.getItem('authId');
    const role = localStorage.getItem('authUserRole');
    const [error, setError] = useState('');
    const [fieldErrors, setFieldErrors] = useState<FieldErrors>({
        statusCode: 0,
        message: '',
        errors: {}
    });
    const [success, setSuccess] = useState(false);
    const [facilityNames, setFacilityNames] = useState<string[]>([]);
    const [facilities, setFacilities] = useState<FacilityResponse[]>([]);
    const [educators, setEducators] = useState<{ id: string, displayName: string }[]>([]);
    const [searchParams] = useSearchParams();
    const useCase = searchParams.get('useCase');
    const id = searchParams.get('activityId');

    console.log(useCase);

    const [formData, setFormData] = useState({
        name: '',
        description: '',
        educator: (role === 'Educator') ? educatorId : '',
        type: '',
        recommendedAge: null as number | null,
        private: false,
        facility: '',
        facilityId: '',
        day: null as Date | null, // Aseguramos que sea de tipo Date o null
        time: null as Date | null
    });

    const fetchAllFacilityNames = useCallback(async () => {
        try {
            const response = await facilityService.getAllFacilities({ useCase: 'AdminEducatorView' });
            const facilityArray: FacilityResponse[] = Array.isArray(response.result) ? response.result as unknown as FacilityResponse[] : Array.from(response.result);
            const namesArray: string[] = facilityArray.map(facility => facility.name);
            setFacilityNames(namesArray);
            setFacilities(facilityArray);
            cacheService.saveFacilityNames(namesArray);
        } catch (error) {
            console.error('Error fetching facility types:', error);
            const cachedFacilityNames = cacheService.loadFacilityNames();
            setFacilityNames(cachedFacilityNames);
        }
    }, []);

    const fetchAllEducators = useCallback(async () => {
        try {
            const users = await userService.getAllUsers({ useCase: 'AsFilter', rol: 'Educator' });
            const usersArray = Array.isArray(users) ? users : Array.from(users.users);
            const formattedEducators = usersArray.map(user => ({
                id: user.id,
                displayName: `${user.firstName} ${user.lastName} @${user.username}`
            }));
            if (role === 'Educator') {
                const currentUser = formattedEducators.find(educator => educator.id === educatorId);
                if (currentUser) {
                    setEducators([{ id: educatorId, displayName: 'Yo' }, ...formattedEducators.filter(educator => educator.id !== educatorId)]);
                } else {
                    setEducators(formattedEducators);
                }
            } else {
                setEducators(formattedEducators);
            }
            cacheService.saveEducators(formattedEducators);
        } catch (error) {
            console.error('Error obteniendo los educadores:', error);
            const cachedEducators = cacheService.loadEducators();
            setEducators(cachedEducators);
        }
    }, [educatorId, role]);

    const fetchInitialData = useCallback(async () => {
        await Promise.all([fetchAllFacilityNames(), fetchAllEducators()]);
    }, [fetchAllFacilityNames, fetchAllEducators]);

    useEffect(() => {
        fetchInitialData();
    }, []);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement> | SelectChangeEvent<string>) => {
        const { name, value, type, checked } = e.target as HTMLInputElement;

        setFormData({
            ...formData,
            [name || '']: type === 'checkbox' ? checked : value,
        });
    };

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
        let selectedName = '';
        let facilityId = '';
        let facility = null;

        e.preventDefault();
        setError('');
        setFieldErrors({
            statusCode: 0,
            message: '',
            errors: {}
        });
        setSuccess(false);

        if (
            (useCase !== 'CreateActivityDate' &&
                (
                    !formData.name ||
                    !formData.description ||
                    !formData.educator ||
                    !formData.type ||
                    !formData.recommendedAge ||
                    !formData.facility
                )
            ) ||
            (useCase !== 'CreateActivity' &&
                (
                    !formData.day ||
                    !formData.time
                )
            )
        ) {
            setError('Por favor, completa todos los campos obligatorios.');
            return;
        }

        selectedName = formData.facility;
        facility = facilities.find(facility => facility.name === selectedName);
        facilityId = facility ? facility.id : '';
        formData.facilityId = facilityId;

        console.log('Datos enviados:', formData);
        console.log('Id de la actividad:', id);

        try {
            await activityService.createActivity({
                useCase: useCase || '',
                activityId: useCase === 'CreateActivityDate' && id ? id : '',
                name: formData.name,
                description: formData.description,
                educator: formData.educator ? formData.educator : '',
                type: formData.type,
                date: formData.day ? formData.day : new Date,
                time: formData.time ? formData.time : new Date,
                recommendedAge: formData.recommendedAge ? formData.recommendedAge : 2,
                facility: formData.facilityId,
                pending: role === 'Educator',
                private: formData.private
            });
            setSuccess(true);
        }
        catch (error) {
            const apiError = error as FieldErrors;
            if (apiError && apiError.errors) {
                const errorData = apiError.errors;

                setFieldErrors({
                    statusCode: apiError.statusCode || 400,
                    message: apiError.message || 'Ocurrieron errores de validación.',
                    errors: errorData
                });
            }
            else {
                setError('Hubo un error al crear la actividad');
            }
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
                    Crear Actividad
                </Typography>

                {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                {/* Errores de validación específicos de la API */}
                {Object.keys(fieldErrors.errors).map((field) => (
                    <Alert severity="error" key={field} sx={{ mb: 2 }}>
                        {`${fieldErrors.errors[field].join(', ')}`}
                    </Alert>
                ))}
                {success && role === 'Educator' && <Alert severity="success" sx={{ mb: 2 }}>Solicitud realizada con éxito.</Alert>}
                {success && role === 'Admin' && <Alert severity="success" sx={{ mb: 2 }}>Actividad creada con éxito.</Alert>}

                <Box component="form" onSubmit={handleSubmit}>
                    <Grid container spacing={2}>
                        {useCase !== 'CreateActivityDate' &&
                            <Grid item xs={12}>
                                <TextField
                                    fullWidth
                                    label="Nombre"
                                    name="name"
                                    value={formData.name}
                                    onChange={handleChange}
                                    margin="normal"
                                    required
                                />
                            </Grid>
                        }
                        {useCase !== 'CreateActivityDate' &&
                            <Grid item xs={12}>
                                <TextField
                                    fullWidth
                                    label="Descripción"
                                    name="description"
                                    value={formData.description}
                                    onChange={handleChange}
                                    margin="normal"
                                    multiline
                                    rows={4}
                                    required
                                />
                            </Grid>
                        }
                        {useCase !== 'CreateActivityDate' &&
                            <Grid item xs={12}>
                                <TextField
                                    fullWidth
                                    label="Tipo"
                                    name="type"
                                    value={formData.type}
                                    onChange={handleChange}
                                    margin="normal"
                                    required
                                />
                            </Grid>
                        }
                        {useCase !== 'CreateActivityDate' &&
                            <Grid item xs={12} sm={6}>
                                <TextField
                                    fullWidth
                                    label="Edad Recomendada"
                                    name="recommendedAge"
                                    type="number"
                                    value={formData.recommendedAge}
                                    onChange={handleChange}
                                    margin="normal"
                                />
                            </Grid>
                        }
                        {useCase !== 'CreateActivityDate' &&
                            <Grid item xs={12} sm={6}>
                                <FormControl sx={{ ml: 2, minWidth: 200 }} variant="outlined">
                                    <InputLabel id="facility-name-select-label"> Instalación</InputLabel>
                                    <Select
                                        labelId="facility-name-select-label"
                                        name="facility"
                                        value={formData.facility}
                                        onChange={handleChange}
                                        label="Instalación"
                                        renderValue={(selected) => selected}
                                        sx={{
                                            minHeight: '56px',
                                            maxHeight: '100vh',
                                            '& .MuiSelect-select': {
                                                paddingTop: '8px',
                                                paddingBottom: '8px',
                                                textAlign: 'left',
                                                whiteSpace: 'normal',
                                                display: 'block',
                                                overflow: 'hidden',
                                            },
                                        }}
                                        MenuProps={{
                                            PaperProps: {
                                                style: {
                                                    maxHeight: 224,
                                                    width: 250,
                                                },
                                            },
                                        }}
                                    >
                                        {facilityNames.map((typeOption) => (
                                            <MenuItem key={typeOption} value={typeOption}>
                                                {typeOption}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            </Grid>
                        }
                        {useCase !== 'CreateActivityDate' &&
                            <Grid item xs={12}>
                                <FormControl sx={{ ml: 2, minWidth: 200 }} variant="outlined">
                                    <InputLabel id="educator-select-label">Educador</InputLabel>
                                    <Select
                                        labelId="educator-select-label"
                                        name="educator"
                                        value={formData.educator || ''}
                                        onChange={handleChange}
                                        label="Educador"
                                        required
                                    >
                                        {educators.map((educator) => (
                                            <MenuItem key={educator.id} value={educator.id}>
                                                {educator.displayName}
                                            </MenuItem>
                                        ))}
                                    </Select>
                                </FormControl>
                            </Grid>
                        }
                        {useCase !== 'CreateActivityDate' &&
                            <Grid item xs={12}>
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            name="private"
                                            checked={formData.private}
                                            onChange={handleChange}
                                        />
                                    }
                                    label="Privada"
                                />
                            </Grid>
                        }
                        {useCase !== 'CreateActivity' &&
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
                        }
                        {useCase !== 'CreateActivity' &&
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
                        }
                        <Grid item xs={12}>
                            <Button
                                type="submit"
                                fullWidth
                                variant="contained"
                                color="primary"
                                sx={{ mt: 2 }}
                            >
                                Crear Actividad
                            </Button>
                        </Grid>
                    </Grid>
                </Box>
            </Paper>
        </Box>
    );
};

export default ActivityFormPage;
