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
    Alert,
    Checkbox,
    FormControlLabel
} from '@mui/material';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import decoration1 from '/images/decorative/toy-train.png';
import decoration2 from '/images/decorative/swing.png';
import { authService } from '../services/authService';
import { useAuth } from '../components/auth/authContext';
import { FieldErrors } from '../interfaces/Error';
import { GoogleLogin, CredentialResponse } from '@react-oauth/google';
import Swal from 'sweetalert2';
import { tokenService } from '../services/tokenService';
import { OnlinePagesProps } from '../interfaces/Pages';
import { useGoogleReCaptcha } from 'react-google-recaptcha-v3';

const RegisterPage: React.FC<OnlinePagesProps> = ({ online, useCaptcha }) => {
    const navigate = useNavigate();
    const location = useLocation();
    const [showPassword, setShowPassword] = useState(false);
    const [error, setError] = useState('');
    const { setCanAccessVerifyEmail, login } = useAuth();
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        username: '',
        password: '',
        confirmPassword: '',
        userType: 'Parent'
    });
    const [fieldErrors, setFieldErrors] = useState<FieldErrors>({
        statusCode: 0,
        message: '',
        errors: {}
    });

    const [passwordMatchError, setPasswordMatchError] = useState('');
    const [captchaToken, setCaptchaToken] = useState<string | null>(null);
    const [isHuman, setIsHuman] = useState(false);

    const { executeRecaptcha } = useGoogleReCaptcha();

    useEffect(() => {
        const storedFormData = localStorage.getItem('formData');
        if (storedFormData) {
            setFormData(JSON.parse(storedFormData));
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

        if (e.target.name === 'confirmPassword' || e.target.name === 'password') {
            if (newFormData.password !== newFormData.confirmPassword) {
                setPasswordMatchError('NO coinciden.');
            } else {
                setPasswordMatchError('Coinciden.');
            }
        }
    };

    const handleCaptchaVerification = async () => {
        if (!executeRecaptcha) {
            console.log('El reCAPTCHA no está disponible todavía');
            return;
        }

        const action = 'register';
        const token = await executeRecaptcha(action);
        setCaptchaToken(token);
        console.log("Token Recaptcha:")
        console.log(token)
        setIsHuman(true);
        console.log('CAPTCHA completado con éxito');
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setCanAccessVerifyEmail(true);
        setError('');
        setFieldErrors({
            statusCode: 0,
            message: '',
            errors: {}
        });

        if (formData.password !== formData.confirmPassword) {
            setPasswordMatchError('NO coinciden.');
            return;
        }

        if (!isHuman) {
            setError('Por favor, verifica que eres humano.');
            return;
        }

        try {
            if (online) {
                if (useCaptcha && captchaToken) {
                    const response = await authService.verifyCaptcha(captchaToken);
                    if (!response.isValid) {
                        console.log("Response del verifyCaptcha")
                        console.table(response)
                        setError('Se ha detectado un comportamiento sospechoso, por favor vuelva a intentarlo.');
                        setIsHuman(false)
                        return;
                    }
                } else {
                    setError('Ha ocurrido un error inesperado, por favor vuelva a intentarlo.');
                    setIsHuman(false)
                    return;
                }
            }

            // Proceder con el registro
            const result = await authService.register({
                firstName: formData.firstName,
                lastName: formData.lastName,
                username: formData.username,
                password: formData.password,
                email: formData.email,
                rol: formData.userType
            });

            if (result.id) {
                localStorage.setItem('DeleteToken', result.id);
            }
            localStorage.setItem('pendingVerificationEmail', formData.username);
            localStorage.setItem('authUserRole', result.rolName);
            formData.password = '';
            formData.confirmPassword = '';
            localStorage.setItem('formData', JSON.stringify(formData));
            navigate('/verify-email');
        } catch (error) {
            const apiError = error as FieldErrors;

            if (apiError && apiError.errors) {
                const errorData = apiError.errors;

                setFieldErrors({
                    statusCode: apiError.statusCode || 400,
                    message: apiError.message || 'Ocurrieron errores de validación.',
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

    const handleGoogleSuccess = async (credentialResponse: CredentialResponse) => {
        if (credentialResponse.credential) {
            try {
                const tokenDecoded = await tokenService.decodeToken(credentialResponse.credential);
                // Almacenar los datos en localStorage
                if (tokenDecoded && tokenDecoded.claims) {
                    const { given_name, family_name, picture, email, email_verified } = tokenDecoded.claims;
                    const { value: username } = await Swal.fire({
                        title: 'Introduce tu nombre de usuario',
                        input: 'text',
                        inputLabel: 'Nombre de usuario',
                        inputPlaceholder: 'Introduce tu nombre de usuario',
                        showCancelButton: true,
                        inputValidator: (value) => {
                            if (!value) {
                                return 'El nombre de usuario no puede estar vacío.';
                            }
                            if (value.length < 3 || value.length > 15) {
                                return 'El nombre de usuario debe tener entre 3 y 15 caracteres.';
                            }
                            if (!/^(?=[a-zA-Z0-9._]{3,15}$)(?!.*[_.]{2})[^_.].*[^_.]$/.test(value)) {
                                return 'Nombre de usuario no válido.';
                            }
                            if (value.trim().length === 0) {
                                return 'El nombre de usuario no puede ser solo espacios en blanco.';
                            }
                            return null;
                        }
                    });

                    if (username) {
                        localStorage.setItem('username', username);
                        try {
                            if (given_name && family_name && picture && email && email_verified) {
                                const response = await authService.googleAccess({
                                    firstName: given_name,
                                    lastName: family_name,
                                    imageUrl: picture,
                                    email: email,
                                    isConfirmed: email_verified ? 'true' : 'false',
                                    username: username,
                                    rol: 'Parent',
                                    action: 'register'
                                });

                                if (response.token && response.id && response.username) {
                                    localStorage.setItem('authToken', response.token);
                                    localStorage.setItem('authId', response.id);
                                    localStorage.setItem('authUsername', response.username);
                                    login();
                                    tokenService.startTokenRefreshCheck();
                                    navigate('/');
                                }
                            }
                        } catch (error) {
                            console.error("Error durante el registro con Google: ", error);
                            throw error;
                        }
                    }
                }
            } catch (error) {
                console.error("Error al decodificar el token de autenticación de Google: ", error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Ha ocurrido un error inesperado. Por favor, inténtalo de nuevo.',
                });
            }
        } else {
            console.error("No se recibió credencial en la respuesta.");
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se recibió credencial. Por favor, inténtalo de nuevo.',
            });
        }
    };

    const handleGoogleFailure = () => {
        console.error('Fallo el inicio de sesión con Google');
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Fallo el inicio de sesión con Google. Por favor, inténtalo de nuevo.',
        });
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

                    {online && (
                        <>
                            <GoogleLogin
                                onSuccess={handleGoogleSuccess}
                                onError={handleGoogleFailure}
                            />

                            <Typography
                                variant="body1"
                                sx={{
                                    color: 'gray',
                                    my: 2
                                }}
                            >
                                o
                            </Typography>
                        </>
                    )}

                    {error && (
                        <Alert severity="error" sx={{ width: '100%', mb: 3 }}>
                            {error}
                        </Alert>
                    )}

                    <Box component="form" onSubmit={handleSubmit} noValidate sx={{ width: '100%' }}>
                        {(['firstName', 'lastName', 'email', 'username', 'password', 'confirmPassword'] as const).map((field) => (
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
                                        {field === 'confirmPassword' && 'Confirmar Contraseña '}
                                        <span style={{ color: '#ff0000' }}>*</span>
                                    </span>
                                }
                                type={(field === 'password' || field === 'confirmPassword') ? (showPassword ? 'text' : 'password') : 'text'}
                                id={field}
                                value={formData[field]}
                                onChange={handleChange}
                                error={!!fieldErrors.errors[field]}
                                helperText={fieldErrors.errors[field]?.join(' ')}
                                slotProps={{
                                    input: (field === 'password' || field === 'confirmPassword') ? {
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

                        {formData.confirmPassword && (
                            <Typography
                                variant="body2"
                                sx={{
                                    mt: 1,
                                    mb: 2
                                }}
                            >
                                Las contraseñas: <span style={{ color: passwordMatchError.includes('NO') ? 'red' : 'green' }}>{passwordMatchError}</span>
                            </Typography>
                        )}

                        {online && useCaptcha && (<FormControlLabel
                            control={
                                <Checkbox
                                    checked={isHuman}
                                    onChange={handleCaptchaVerification}
                                    name="isHuman"
                                    color="primary"
                                />
                            }
                            label="Verifica que eres humano"
                        />)}

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
                            disabled={formData.password !== formData.confirmPassword || (online && useCaptcha && !captchaToken)}
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