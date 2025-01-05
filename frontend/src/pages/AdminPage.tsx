import React, { useState } from 'react';
import { Box, Typography, Button, Menu, MenuItem } from '@mui/material';
import { Link } from 'react-router-dom';

const AdminPage = () => {
    const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
    const handleMenuOpen = (event: React.MouseEvent<HTMLButtonElement>) => setAnchorEl(event.currentTarget);
    const handleMenuClose = () => setAnchorEl(null);

    return (
        <Box
            sx={{
                width: '100vw',
                minHeight: '100vh',
                backgroundColor: 'white',
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                padding: 4,
                position: 'relative',
            }}
        >
            {/* Header Section */}
            <Typography
                variant="h3"
                sx={{ textAlign: 'center', fontWeight: 700, mb: 6, color: '#2C3E50' }}
            >
                Panel de Administración
            </Typography>

            
        </Box>
    );
};

export default AdminPage;
