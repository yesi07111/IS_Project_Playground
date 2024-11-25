import React, { useState } from 'react';
import { Box, Container, Paper, Typography, Radio, RadioGroup, FormControlLabel, Button, Snackbar } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import decoration1 from '../assets/images/decorative/bumper-car.png';
import decoration2 from '../assets/images/decorative/mole-game.png';

const UserTypePage: React.FC = () => {
    const navigate = useNavigate();
    const [userType, setUserType] = useState('');
    const [openSnackbar, setOpenSnackbar] = useState(false);

    const handleUserTypeChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setUserType(event.target.value);
    };

    const handleContinue = () => {
        if (!userType) {
            setOpenSnackbar(true);
        } else {
            localStorage.setItem('userType', userType);
            navigate('/register');
        }
    };

    const handleCloseSnackbar = () => {
        setOpenSnackbar(false);
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
                height: '100%',
                py: 3,
                px: 2,
                position: 'relative',
                zIndex: 1
            }}>
                <Paper
                    elevation={3}
                    sx={{
                        p: 4,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                        backgroundColor: 'transparent',
                        border: '2px solid rgba(0, 0, 0, 0.2)',
                        borderRadius: 2,
                        width: '100%',
                        transition: 'all 0.3s ease',
                        transform: 'scale(1)',
                        boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)',
                        '&:hover': {
                            transform: 'scale(1.02) translateY(-5px)',
                            boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                            border: '2px solid rgba(0, 0, 0, 0.3)'
                        }
                    }}
                >
                    <Typography variant="h5" sx={{ mb: 3, color: 'primary.main' }}>
                        ¿Estás aquí como Usuario, Educador o Administrador?
                    </Typography>
                    <Box sx={{ width: '100%', mb: 3, ml: 2 }}>
                        <RadioGroup value={userType} onChange={handleUserTypeChange}>
                            <FormControlLabel
                                value="Parent"
                                control={<Radio sx={{ color: 'black', '&.Mui-checked': { color: 'primary.main' } }} />}
                                label="Usuario"
                            />
                            <FormControlLabel
                                value="Educator"
                                control={<Radio sx={{ color: 'black', '&.Mui-checked': { color: 'primary.main' } }} />}
                                label="Educador"
                            />
                            <FormControlLabel
                                value="Admin"
                                control={<Radio sx={{ color: 'black', '&.Mui-checked': { color: 'primary.main' } }} />}
                                label="Administrador"
                            />
                        </RadioGroup>
                    </Box>
                    <Button
                        variant="contained"
                        onClick={handleContinue}
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
                        Continuar
                    </Button>
                </Paper>
            </Container>

            <Snackbar
                open={openSnackbar}
                autoHideDuration={6000}
                onClose={handleCloseSnackbar}
                message="Por favor, selecciona un tipo de usuario."
                anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
            />
        </Box>
    );
};

export default UserTypePage;