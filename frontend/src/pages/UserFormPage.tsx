import React, { useState } from 'react';
import {
    Box,
    Container,
    Paper,
    TextField,
    Button,
    Typography,
    InputAdornment,
    IconButton
} from '@mui/material';
import { userService } from '../services/userService';
import { Visibility, VisibilityOff } from '@mui/icons-material';

const UserFormPage: React.FC = () => {
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');
    const [showPassword, setShowPassword] = useState(false);

    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        userName: '',
        password: '',
        role: '',
        email: '',
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setSuccess('');

        console.table(formData);

        try {
            await userService.createUser({
                firstName: formData.firstName,
                lastName: formData.lastName,
                userName: formData.userName,
                password: formData.password,
                role: formData.role,
                email: formData.email,
            });
            setSuccess('Usuario creado exitosamente.');
            setFormData({ firstName: '', lastName: '', userName: '', password: '', role: '', email: '' });
        }
        catch {
            setError('Error al crear el usuario. Por favor, inténtalo nuevamente.');
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
                overflowY: 'auto',
                margin: 0,
                padding: 0,
                boxSizing: 'border-box',
            }}
        >
            <Container maxWidth="sm" sx={{ my: 4 }}>
                <Paper elevation={3} sx={{ p: 4 }}>
                    <Typography variant="h4" component="h1" gutterBottom>
                        Crear Usuario
                    </Typography>

                    {error && <Typography color="error" sx={{ mb: 2 }}>{error}</Typography>}
                    {success && <Typography color="success.main" sx={{ mb: 2 }}>{success}</Typography>}

                    <Box component="form" onSubmit={handleSubmit}>
                        <TextField
                            label="Nombre"
                            name="firstName"
                            value={formData.firstName}
                            onChange={handleChange}
                            fullWidth
                            margin="normal"
                            required
                        />
                        <TextField
                            label="Apellidos"
                            name="lastName"
                            value={formData.lastName}
                            onChange={handleChange}
                            fullWidth
                            margin="normal"
                            required
                        />
                        <TextField
                            label="Nombre de Usuario"
                            name="userName"
                            value={formData.userName}
                            onChange={handleChange}
                            fullWidth
                            margin="normal"
                            required
                        />
                        <TextField
                            label="Contraseña"
                            name="password"
                            type={showPassword ? 'text' : 'password'}
                            value={formData.password}
                            onChange={handleChange}
                            fullWidth
                            margin="normal"
                            required
                            slotProps={{
                                input: {
                                    endAdornment: (
                                        <InputAdornment position="end">
                                            <IconButton
                                                onClick={() => setShowPassword(!showPassword)}
                                                edge="end"
                                            >
                                                {showPassword ? <Visibility /> : <VisibilityOff />}
                                            </IconButton>
                                        </InputAdornment>
                                    ),
                                },
                            }}
                        />
                        <TextField
                            label="Rol"
                            name="role"
                            value={formData.role}
                            onChange={handleChange}
                            fullWidth
                            margin="normal"
                            required
                            select
                            SelectProps={{
                                native: true,
                            }}
                        >
                            <option value="Admin">Admin</option>
                            <option value="Educator">Educator</option>
                            <option value="Parent">Parent</option>
                        </TextField>
                        <TextField
                            label="Email"
                            name="email"
                            value={formData.email}
                            onChange={handleChange}
                            fullWidth
                            margin="normal"
                            required
                        />

                        <Button
                            type="submit"
                            variant="contained"
                            color="primary"
                            fullWidth
                            sx={{ mt: 3 }}
                        >
                            Crear Usuario
                        </Button>
                    </Box>
                </Paper>
            </Container>
        </Box>
    );
};

export default UserFormPage;
