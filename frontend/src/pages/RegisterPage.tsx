import React, { useState, useEffect } from 'react';
import {
    Box,
    Container,
    Paper,
    TextField,
    Button,
    Typography,
    Link as MuiLink,
    InputAdornment,
    IconButton,
    Alert
} from '@mui/material';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import decoration1 from '../assets/images/decorative/toy-train.png';
import decoration2 from '../assets/images/decorative/swing.png';
import { authService } from '../services/authService';
import { useAuth } from '../components/auth/authContext';

const RegisterPage: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const [showPassword, setShowPassword] = useState(false);
    const [error, setError] = useState('');
    const { setCanAccessVerifyEmail } = useAuth();
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        username: '',
        password: '',
        userType: ''
    });

    useEffect(() => {
        const storedFormData = localStorage.getItem('formData');
        const storedUserType = localStorage.getItem('userType');
        if (storedFormData) {
            setFormData(JSON.parse(storedFormData));
        }
        if (storedUserType) {
            setFormData((prevData) => ({
                ...prevData,
                userType: storedUserType
            }));
        }
    }, []);

    useEffect(() => {
        const clearLocalStorage = () => {
            localStorage.removeItem('formData');
        };

        const handleBeforeUnload = () => {
            if (location.pathname !== '/verify-email') {
                clearLocalStorage();
            }
        };

        const handlePopState = () => {
            if (location.pathname !== '/verify-email') {
                clearLocalStorage();
            }
        };

        if (location.pathname !== '/verify-email') {
            clearLocalStorage();
        }

        window.addEventListener('beforeunload', handleBeforeUnload);
        window.addEventListener('popstate', handlePopState);

        return () => {
            window.removeEventListener('beforeunload', handleBeforeUnload);
            window.removeEventListener('popstate', handlePopState);
        };
    }, [location]);

    useEffect(() => {
        if (location.pathname !== '/verify-email') {
            localStorage.removeItem('formData');
        }
    }, [location.pathname]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newFormData = {
            ...formData,
            [e.target.name]: e.target.value
        };
        setFormData(newFormData);
        localStorage.setItem('formData', JSON.stringify(newFormData));
    };

    type FieldErrors = {
        statusCode: number;
        message: string;
        errors: {
            [key: string]: string[];
        };
    };

    const [fieldErrors, setFieldErrors] = useState<FieldErrors>({
        statusCode: 0,
        message: '',
        errors: {}
    });

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setCanAccessVerifyEmail(true);
        setError('');
        setFieldErrors({
            statusCode: 0,
            message: '',
            errors: {}
        });

        const ToDelete = localStorage.getItem("ToDelete");
        if (ToDelete) {
            const storedFormData = await localStorage.getItem('formData');
            const storedUserType = await localStorage.getItem('userType');
            const deleteToken = await localStorage.getItem('DeleteToken') ?? '';
            if (storedFormData) {
                setFormData(JSON.parse(storedFormData));

                if (storedUserType) {
                    setFormData((prevData) => ({
                        ...prevData,
                        userType: storedUserType
                    }));
                }
                await authService.deleteUserFromDB(formData.firstName, formData.lastName, formData.username, formData.email, formData.userType, deleteToken);
                localStorage.removeItem('DeleteToken');
                localStorage.removeItem('ToDelete');
            }
        }

        try {
            const result = await authService.register({
                FirstName: formData.firstName,
                LastName: formData.lastName,
                Username: formData.username,
                Password: formData.password,
                Email: formData.email,
                Roles: [formData.userType]
            });

            if (result.deleteToken) {
                localStorage.setItem('DeleteToken', result.deleteToken);
            }
            localStorage.setItem('pendingVerificationEmail', formData.username);
            formData.password = '';
            localStorage.setItem('formData', JSON.stringify(formData));
            navigate('/verify-email');
        } catch (error) {
            const apiError = error as FieldErrors;

            if (apiError && apiError.errors) {
                const errorData = apiError.errors;

                setFieldErrors({
                    statusCode: apiError.statusCode || 400,
                    message: apiError.message || 'Validation errors occurred',
                    errors: errorData
                });
            } else {
                setError('Hubo un error durante el registro. Por favor, inténtalo de nuevo.');
            }
        }
    };

    const evaluatePasswordStrength = (password: string) => {
        let strength = 0;
        if (/[A-Z]/.test(password)) strength++;
        if (/[a-z]/.test(password)) strength++;
        if (/[0-9]/.test(password)) strength++;
        if (/[^a-zA-Z0-9]/.test(password)) strength++;
        if (password.length >= 8) strength++;

        if (strength <= 2) return { label: 'Baja', color: 'red' };
        if (strength <= 4) return { label: 'Media', color: '#DAA520' };
        return { label: 'Fuerte', color: 'green' };
    };

    const passwordStrength = evaluatePasswordStrength(formData.password);

    const textFieldSx = {
        mb: 2,
        '& .MuiInputBase-root': {
            backgroundColor: 'rgba(255, 255, 255, 0.7)'
        },
        '& .MuiFormLabel-root': {
            '& .MuiFormLabel-asterisk': {
                color: '#ff0000'
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
                overflowY: 'auto',
                margin: 0,
                padding: 0,
                boxSizing: 'border-box',
            }}
        >
            <Box
                component="img"
                src={decoration1}
                sx={{
                    position: 'absolute',
                    right: -20,
                    bottom: '36%',
                    opacity: 0.1,
                    width: '400px',
                    pointerEvents: 'none',
                    transform: 'translate(-50px, -50px)'
                }}
            />

            <Box
                component="img"
                src={decoration2}
                sx={{
                    position: 'absolute',
                    left: '5%',
                    top: '50%',
                    transform: 'translateY(-50%)',
                    opacity: 0.1,
                    width: '500px',
                    pointerEvents: 'none',
                    zIndex: 0
                }}
            />

            <Container maxWidth="sm" sx={{
                my: 'auto',
                width: '100%',
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                alignItems: 'center',
                py: 2,
                px: 2,
                position: 'relative',
                zIndex: 1,
                minHeight: '100vh',
            }}>
                <Paper
                    elevation={3}
                    sx={{
                        p: 2,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        backgroundColor: 'transparent',
                        border: '2px solid rgba(0, 0, 0, 0.2)',
                        borderRadius: 2,
                        width: '100%',
                        transition: 'all 0.3s ease',
                        boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
                        '&:hover': {
                            transform: 'scale(1.02) translateY(-5px)',
                            boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                            border: '2px solid rgba(0, 0, 0, 0.3)'
                        }
                    }}
                >
                    <Typography
                        component="h1"
                        variant="h4"
                        sx={{
                            color: 'primary.main',
                            fontWeight: 700,
                            mb: 2
                        }}
                    >
                        ¡Únete a la diversión!
                    </Typography>

                    {error && (
                        <Alert severity="error" sx={{ width: '100%', mb: 3 }}>
                            {error}
                        </Alert>
                    )}

                    <Box component="form" onSubmit={handleSubmit} noValidate sx={{ width: '100%' }}>
                        {(['firstName', 'lastName', 'email', 'username', 'password'] as const).map((field) => (
                            <TextField
                                key={field}
                                margin="normal"
                                fullWidth
                                name={field}
                                label={
                                    <span>
                                        {field === 'firstName' && 'Nombre '}
                                        {field === 'lastName' && 'Apellidos '}
                                        {field === 'email' && 'Correo Electrónico '}
                                        {field === 'username' && 'Nombre de Usuario '}
                                        {field === 'password' && 'Contraseña '}
                                        <span style={{ color: '#ff0000' }}>*</span>
                                    </span>
                                }
                                type={field === 'password' ? (showPassword ? 'text' : 'password') : 'text'}
                                id={field}
                                value={formData[field]}
                                onChange={handleChange}
                                error={!!fieldErrors.errors[field]}
                                helperText={fieldErrors.errors[field]?.join(' ')}
                                slotProps={{
                                    input: field === 'password' ? {
                                        endAdornment: (
                                            <InputAdornment position="end">
                                                <IconButton
                                                    onClick={() => setShowPassword(!showPassword)}
                                                    edge="end"
                                                    sx={{
                                                        '&:focus, &:hover, &:active, &.MuiIconButton-root': {
                                                            backgroundColor: 'transparent',
                                                            outline: 'none',
                                                            transition: 'none'
                                                        }
                                                    }}
                                                >
                                                    {showPassword ? <Visibility /> : <VisibilityOff />}
                                                </IconButton>
                                            </InputAdornment>
                                        ),
                                    } : undefined
                                }}
                                sx={textFieldSx}
                            />
                        ))}

                        {formData.password && (
                            <Typography
                                variant="body2"
                                sx={{
                                    color: 'gray',
                                    mt: 1,
                                    mb: 2
                                }}
                            >
                                Fortaleza de la contraseña: <span style={{ color: passwordStrength.color }}>{passwordStrength.label}</span>
                            </Typography>
                        )}

                        <input type="hidden" name="userType" value={formData.userType} />

                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            sx={{
                                mt: 2,
                                mb: 3,
                                py: 1.5,
                                backgroundColor: 'primary.main',
                                '&:hover': {
                                    backgroundColor: 'primary.dark'
                                }
                            }}
                        >
                            Registrarse
                        </Button>

                        <Box sx={{ textAlign: 'center' }}>
                            <MuiLink
                                component={Link}
                                to="/login"
                                variant="body2"
                                sx={{
                                    color: 'primary.main',
                                    textDecoration: 'none',
                                    '&:hover': {
                                        textDecoration: 'underline'
                                    }
                                }}
                            >
                                ¿Ya tienes una cuenta? ¡Inicia sesión aquí!
                            </MuiLink>
                        </Box>
                    </Box>
                </Paper>
            </Container>
        </Box>
    );
};

export default RegisterPage;