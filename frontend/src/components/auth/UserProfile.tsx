import React, { useState } from 'react';
import { IconButton, Menu, MenuItem, Avatar } from '@mui/material';
import { Link } from 'react-router-dom';
import { useAuth } from './authContext';

const UserProfile: React.FC = () => {
    const { logout, isEmailVerified } = useAuth(); // Cambia a isEmailVerified
    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

    const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    return (
        <>
            <IconButton
                size="large"
                edge="end"
                aria-label="account of current user"
                aria-controls="menu-appbar"
                aria-haspopup="true"
                onClick={handleMenu}
                color="inherit"
                sx={{
                    ml: -1,
                    '&:focus': {
                        outline: 'none',
                    }
                }}
            >
                <Avatar
                    alt="User Profile"
                    src="/path/to/profile-pic.jpg"
                    sx={{ bgcolor: '#E0BBE4' }}
                />
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
                <MenuItem onClick={handleClose} component={Link} to="/profile">Ver Perfil</MenuItem>
                {!isEmailVerified && (
                    <MenuItem onClick={handleClose} component={Link} to="/verify-email">Verificar Email</MenuItem>
                )}
                <MenuItem onClick={handleClose} component={Link} to="/reservas">Ver Mis Reservas</MenuItem>
                <MenuItem onClick={handleClose} component={Link} to="/reviews">Ver Mis Reseñas</MenuItem>
                <MenuItem onClick={() => { handleClose(); logout(); }}>Cerrar Sesión</MenuItem>
            </Menu>
        </>
    );
};

export default UserProfile;