import React from 'react';
import { Paper, Typography, Grid2 as Grid } from '@mui/material';
import { People, Event, Star } from '@mui/icons-material';
import { styled } from '@mui/material/styles';

const StyledPaper = styled(Paper)(({ theme }) => ({
    padding: theme.spacing(2),
    textAlign: 'center',
    transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
    cursor: 'pointer',
    backgroundColor: 'transparent',
    '&:hover': {
        transform: 'scale(1.05)',
        boxShadow: theme.shadows[8],
        backgroundColor: 'transparent',
    }
}));

const IconWrapper = styled('div')(({ theme }) => ({
    '& > svg': {
        fontSize: 40,
        color: theme.palette.primary.main
    }
}));

const StatsSummary: React.FC = () => {
    return (
        <Grid container spacing={3} sx={{ my: 3 }}>
            <Grid size={{ xs: 12, md: 4 }}>
                <StyledPaper elevation={3}>
                    <IconWrapper>
                        <People />
                    </IconWrapper>
                    <Typography variant="h6">Visitantes este mes</Typography>
                    <Typography variant="h4">1,234</Typography>
                </StyledPaper>
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <StyledPaper elevation={3}>
                    <IconWrapper>
                        <Event />
                    </IconWrapper>
                    <Typography variant="h6">Actividades Activas</Typography>
                    <Typography variant="h4">25</Typography>
                </StyledPaper>
            </Grid>
            <Grid size={{ xs: 12, md: 4 }}>
                <StyledPaper elevation={3}>
                    <IconWrapper>
                        <Star />
                    </IconWrapper>
                    <Typography variant="h6">Calificación Promedio</Typography>
                    <Typography variant="h4">4.8</Typography>
                </StyledPaper>
            </Grid>
        </Grid>
    );
};

export default StatsSummary;