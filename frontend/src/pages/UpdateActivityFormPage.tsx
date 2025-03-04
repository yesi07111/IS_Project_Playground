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
    Grid2
} from '@mui/material';
import { LocalizationProvider, DatePicker, TimePicker } from '@mui/x-date-pickers';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { facilityService } from '../services/facilityService';
import { cacheService } from '../services/cacheService';
import { activityService } from '../services/activityService';
import { FacilityResponse } from '../interfaces/Facility';
import { userService } from '../services/userService';
import { useSearchParams } from 'react-router-dom';

const UpdateActivityFormPage: React.FC = () => {
    const educatorId = localStorage.getItem('authId');
    const role = localStorage.getItem('authUserRole');
    const [error, setError] = useState('');
    const [success, setSuccess] = useState(false);
    const [facilityNames, setFacilityNames] = useState<string[]>([]);
    const [facilities, setFacilities] = useState<FacilityResponse[]>([]);
    const [educators, setEducators] = useState<{ id: string, displayName: string }[]>([]);
    const [searchParams] = useSearchParams();

    const useCase = searchParams.get('useCase');
    const id = searchParams.get('activityId');
    const name = searchParams.get('name');
    const description = searchParams.get('description');
    const educatorFirstName = searchParams.get('educFN');
    const educatorLastName = searchParams.get('educLN');
    const educatorUserName = searchParams.get('educUN');
    const type = searchParams.get('type');
    const recommendedAge = searchParams.get('recAge');
    const itsPrivate = searchParams.get('itsPrivate');
    const facilityName = searchParams.get('facilityName');

    const [formData, setFormData] = useState({
        name: name || '',
        description: description || '',
        educatorDis: `${educatorFirstName} ${educatorLastName} @${educatorUserName}`,
        educator: '',
        type: type || '',
        recommendedAge: Number(recommendedAge) || null as number | null,
        private: itsPrivate === 'true'? true : false,
        facility: facilityName || '',
        facilityId: '',
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

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        let selectedName = '';
        let facilityId = '';
        let facility = null;
        let selectedEducator = '';
        let educator;
        let educatorId = '';

        e.preventDefault();
        setError('');
        setSuccess(false);

        if (!formData.name ||
            !formData.description ||
            !formData.educatorDis ||
            !formData.type ||
            !formData.recommendedAge ||
            !formData.facility) {
            setError('Por favor, completa todos los campos obligatorios.');
            return;
        }

        selectedName = formData.facility;
        facility = facilities.find(facility => facility.name === selectedName);
        facilityId = facility ? facility.id : '';
        formData.facilityId = facilityId;

        selectedEducator = formData.educatorDis;
        educator = educators.find(item => item.displayName === selectedEducator);
        educatorId = educator ? educator.id : '';
        formData.educator = educatorId;

        try {
            const result = await activityService.updateActivity({
                useCase: useCase || '',
                activityId: id || '',
                activityDateId: '',
                name: formData.name,
                description: formData.description,
                educator: formData.educator,
                type: formData.type,
                recommendedAge: formData.recommendedAge,
                facility: formData.facilityId,
                pending: role === 'Educator',
                private: formData.private
            });
            setSuccess(true);
        }
        catch (error) {
            setError('Hubo un error al modificar la actividad');
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
                    Modificar Actividad
                </Typography>

                {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                {success && <Alert severity="success" sx={{ mb: 2 }}>Actividad modificada con éxito.</Alert>}

                <Box component="form" onSubmit={handleSubmit}>
                    <Grid container spacing={2}>
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
                        <Grid item xs={12}>
                            <FormControl sx={{ ml: 2, minWidth: 200 }} variant="outlined">
                                <InputLabel id="educator-select-label">Educador</InputLabel>
                                <Select
                                    labelId="educator-select-label"
                                    name="educatorDis"
                                    value={formData.educatorDis} // Mostramos el display name
                                    onChange={handleChange}
                                    label="Educador"
                                    required
                                >
                                    {educators.map((educator) => (
                                        <MenuItem key={educator.id} value={educator.displayName}>
                                            {educator.displayName}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>
                        </Grid>
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
                        <Grid item xs={12}>
                            <Button
                                type="submit"
                                fullWidth
                                variant="contained"
                                color="primary"
                                sx={{ mt: 2 }}
                            >
                                Modificar Actividad
                            </Button>
                        </Grid>
                    </Grid>
                </Box>
            </Paper>
        </Box>
    );
};

export default UpdateActivityFormPage;