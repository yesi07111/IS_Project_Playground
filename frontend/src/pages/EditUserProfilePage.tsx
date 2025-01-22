import React, { useState } from 'react';
import Swal from 'sweetalert2';
import { Box, TextField, Button, Typography, InputAdornment, IconButton } from '@mui/material';
import { styled } from '@mui/material/styles';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import { useNavigate } from 'react-router-dom';
import { passwordService } from '../services/passwordService';
import { userService } from '../services/userService';
import { EditUserData } from '../interfaces/User';
import { useAuth } from '../components/auth/authContext';

const EditUserProfilePage: React.FC = () => {
    const navigate = useNavigate();
    const { setCanAccessVerifyEmail } = useAuth();
    const email = localStorage.getItem("email") ?? ''
    const initialFormData: EditUserData = {
        id: '',
        firstName: localStorage.getItem("firstName") ?? '',
        lastName: localStorage.getItem("lastName") ?? '',
        username: localStorage.getItem("userName") ?? '',
        email: email,
        oldPassword: '',
        password: '',
        confirmPassword: '',
    };

    const [formData, setFormData] = useState(initialFormData);
    const [showPassword, setShowPassword] = useState(false);
    const [passwordMatchError, setPasswordMatchError] = useState('');
    const [passwordStrength, setPasswordStrength] = useState({ label: '', color: '' });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        const newFormData = { ...formData, [name]: value };
        setFormData(newFormData);

        if (name === 'password') {
            setPasswordStrength(passwordService.evaluatePasswordStrength(value));
        }

        if (name === 'confirmPassword' || name === 'password') {
            setPasswordMatchError(passwordService.checkPasswordMatch(newFormData.password, newFormData.confirmPassword));
        }
    };

    const validateFields = () => {
        const { firstName, lastName, username, email } = formData;
        const errors = [];

        if (!firstName || firstName.length < 3 || firstName.length > 15) {
            errors.push('El nombre debe tener entre 3 y 15 caracteres.');
        }

        if (!lastName || lastName.length < 3 || lastName.length > 30) {
            errors.push('El apellido debe tener entre 3 y 30 caracteres.');
        }

        if (!username || username.length < 3 || username.length > 15) {
            errors.push('El nombre de usuario debe tener entre 3 y 15 caracteres.');
        }

        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!email || !emailRegex.test(email)) {
            errors.push('Por favor, introduce un correo electrónico válido.');
        }

        if (errors.length > 0) {
            Swal.fire('Error', errors.join(' '), 'error');
            return false;
        }

        return true;
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!validateFields()) {
            return;
        }

        if (formData.oldPassword) {
            const strongPasswordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/;
            if (!strongPasswordRegex.test(formData.oldPassword)) {
                Swal.fire('Error', 'La contraseña anterior debe tener al menos 6 caracteres, incluir un número, una letra mayúscula, una letra minúscula y un símbolo.', 'error');
                return;
            }

            if (!formData.password || !formData.confirmPassword) {
                Swal.fire('Error', 'Por favor, completa los campos de nueva contraseña y confirmar contraseña.', 'error');
                return;
            }

            if (formData.password !== formData.confirmPassword) {
                setPasswordMatchError('NO coinciden.');
                Swal.fire('Error', 'Las contraseñas no coinciden.', 'error');
                return;
            }
        }

        else if ((formData.password || formData.confirmPassword)) {
            Swal.fire('Error', 'Por favor, completa el campo de su contraseña anterior. Si la olvidó podría intentar resetearla en su página de perfil de usuario.', 'error');
        }

        if (JSON.stringify(formData) === JSON.stringify(initialFormData)) {
            Swal.fire({
                title: 'No hubo cambios',
                text: 'Volver a la página anterior',
                icon: 'info',
                showCancelButton: true,
                confirmButtonText: 'Aceptar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    navigate(-1);
                }
            });
            return;
        }

        if (formData) {
            console.log("Entro al if formdata")
            if (formData.email !== email) {
                const result = await Swal.fire({
                    title: '¿Está seguro?',
                    text: "Para cambiar su correo electrónico, debe confirmarlo primero.",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, cambiar',
                    cancelButtonText: 'No, cancelar',
                });

                if (!result.isConfirmed) {
                    return;
                }

                editUser()
                setCanAccessVerifyEmail(true)
                localStorage.setItem("pendingVerificationEmail", formData.username)
                navigate('/verify-email');
                return;
            }
            editUser()
        }
    };

    const editUser = async () => {
        try {
            const id = localStorage.getItem("authId");
            if (id) {
                formData.id = id
                const response = await userService.editUser(formData)
                if (response) {
                    Swal.fire('¡Éxito!', 'Su perfil de usuario ha sido actualizado correctamente.', 'success');
                    clearLocalStorage()
                    if (location.pathname.startsWith('/edit-profile')) {
                        navigate(-1);
                    }
                    else {
                        Swal.fire('Información', 'Debe confirmar su correo', 'info')
                    }
                }
            }
        } catch (error) {
            console.error("Ha ocurrido un error inesperado al intentar editar su perfil de usuario. ", error)
        }
    }
    const clearLocalStorage = () => {
        localStorage.removeItem("email")
        localStorage.removeItem("firstName")
        localStorage.removeItem("lastName")
        localStorage.removeItem("userName")
    }

    return (
        <Box
            component="form"
            onSubmit={handleSubmit}
            sx={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                minWidth: '100vw',
                minHeight: '100vh',
                bgcolor: 'background.paper',
                boxShadow: 3,
                borderRadius: 2,
                p: 3,
            }}
        >
            <Typography variant="h4" gutterBottom sx={{ color: 'primary.main', fontWeight: 'bold' }}>
                🛠️ Editar Perfil de Usuario
            </Typography>
            <StyledTextField
                label="Nombre"
                name="firstName"
                value={formData.firstName}
                onChange={handleChange}
                variant="outlined"
                fullWidth
                margin="normal"
            />
            <StyledTextField
                label="Apellido"
                name="lastName"
                value={formData.lastName}
                onChange={handleChange}
                variant="outlined"
                fullWidth
                margin="normal"
            />
            <StyledTextField
                label="Nombre de Usuario"
                name="username"
                value={formData.username}
                onChange={handleChange}
                variant="outlined"
                fullWidth
                margin="normal"
            />
            <StyledTextField
                label="Email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                variant="outlined"
                fullWidth
                margin="normal"
            />
            <StyledTextField
                label="Contraseña Anterior"
                name="oldPassword"
                type={showPassword ? 'text' : 'password'}
                value={formData.oldPassword}
                onChange={handleChange}
                variant="outlined"
                fullWidth
                margin="normal"
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
            <StyledTextField
                label="Nueva Contraseña"
                name="password"
                type={showPassword ? 'text' : 'password'}
                value={formData.password}
                onChange={handleChange}
                variant="outlined"
                fullWidth
                margin="normal"
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
            <StyledTextField
                label="Confirmar Nueva Contraseña"
                name="confirmPassword"
                type={showPassword ? 'text' : 'password'}
                value={formData.confirmPassword}
                onChange={handleChange}
                variant="outlined"
                fullWidth
                margin="normal"
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
            <StyledButton type="submit" variant="contained" color="primary" sx={{ mt: 2 }}>
                Guardar Cambios
            </StyledButton>
        </Box>
    );
};

const StyledTextField = styled(TextField)(() => ({
    '& .MuiOutlinedInput-root': {
        borderRadius: 20,
    },
}));

const StyledButton = styled(Button)(({ theme }) => ({
    borderRadius: 20,
    padding: theme.spacing(1, 3),
    fontWeight: 'bold',
    transition: 'background-color 0.3s ease-in-out, transform 0.3s ease-in-out',
    '&:hover': {
        transform: 'scale(1.1)',
    },
}));

export default EditUserProfilePage;