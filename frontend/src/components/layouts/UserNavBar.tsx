import React, { useState } from 'react';
import { AppBar, Toolbar, Typography, Button, Box, IconButton, Menu, MenuItem } from '@mui/material';
import { Link } from 'react-router-dom';
import AccountCircle from '@mui/icons-material/AccountCircle';

const Navbar: React.FC = () => {
    // Estado para el menú del usuario
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const [isUserLoggedIn, setIsUserLoggedIn] = useState(true); // Cambia esto según el estado real del usuario
    const [isEmailVerified, setIsEmailVerified] = useState(false); // Cambia esto según el estado real del usuario

    const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    return (
        <AppBar
            position="sticky"
            sx={{
                top: 0,
                width: '100%',
                zIndex: 1100,
                backgroundColor: 'primary.main',
                '& .MuiToolbar-root': {
                    width: '100%',
                    maxWidth: '100%',
                    justifyContent: 'space-between',
                    padding: { xs: '8px 16px', sm: '8px 24px' },
                    minHeight: '48px',
                }
            }}
        >
            <Toolbar
                variant="dense"
                sx={{
                    minHeight: '48px !important',
                }}
            >
                <Typography
                    variant="h6"
                    component={Link}
                    to="/"
                    sx={{
                        textDecoration: 'none',
                        color: 'inherit',
                        flexGrow: 0,
                        fontSize: '1.1rem',
                    }}
                >
                    KidsParks
                </Typography>
                <Box sx={{
                    display: 'flex',
                    gap: { xs: 1, sm: 2 }
                }}>
                    <Button color="inherit" component={Link} to="/" sx={{ fontWeight: 500, py: 0.5 }}>
                        Inicio
                    </Button>
                    <Button color="inherit" component={Link} to="/actividades" sx={{ fontWeight: 500, py: 0.5 }}>
                        Actividades
                    </Button>
                    <Button color="inherit" component={Link} to="/reservas" sx={{ fontWeight: 500, py: 0.5 }}>
                        Reservas
                    </Button>
                    {isUserLoggedIn ? (
                        <div>
                            <IconButton
                                size="large"
                                aria-label="account of current user"
                                aria-controls="menu-appbar"
                                aria-haspopup="true"
                                onClick={handleMenu}
                                color="inherit"
                            >
                                <AccountCircle />
                            </IconButton>
                            <Menu
                                id="menu-appbar"
                                anchorEl={anchorEl}
                                anchorOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                                keepMounted
                                transformOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                                open={Boolean(anchorEl)}
                                onClose={handleClose}
                            >
                                <MenuItem onClick={handleClose} component={Link} to="/profile">Editar Perfil</MenuItem>
                                {!isEmailVerified && (
                                    <MenuItem onClick={handleClose} component={Link} to="/verify-email">Verificar Email</MenuItem>
                                )}
                            </Menu>
                        </div>
                    ) : (
                        <Button color="inherit" component={Link} to="/login" sx={{ fontWeight: 500, py: 0.5 }}>
                            Iniciar Sesión
                        </Button>
                    )}
                </Box>
            </Toolbar>
        </AppBar>
    );
};

export default Navbar;