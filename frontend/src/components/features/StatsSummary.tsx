import React from 'react';
import { Paper, Typography, Grid2 as Grid } from '@mui/material';
import { People, Event, Star } from '@mui/icons-material';
import { styled } from '@mui/material/styles';

/**
 * Componente estilizado de Paper que representa un resumen de estadísticas.
 * 
 * Este componente utiliza Material-UI para crear un contenedor de estadísticas
 * con un efecto de escala y sombra al pasar el ratón sobre él. Está diseñado
 * para mostrar información de manera destacada y atractiva.
 * 
 * @param {Theme} theme - Tema de Material-UI para aplicar estilos.
 * @returns {object} Estilos aplicados al componente Paper.
 */
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

/**
 * Componente estilizado para envolver íconos.
 * 
 * Este componente utiliza Material-UI para aplicar estilos a los íconos,
 * asegurando que tengan un tamaño y color consistentes con el tema de la aplicación.
 * 
 * @param {Theme} theme - Tema de Material-UI para aplicar estilos.
 * @returns {object} Estilos aplicados al contenedor de íconos.
 */
const IconWrapper = styled('div')(({ theme }) => ({
    '& > svg': {
        fontSize: 40,
        color: theme.palette.primary.main
    }
}));

/**
 * Componente de resumen de estadísticas que muestra información clave en un formato de cuadrícula.
 * 
 * Este componente utiliza Material-UI para organizar y mostrar estadísticas en un formato de cuadrícula.
 * Cada elemento de la cuadrícula muestra un ícono, un título y un valor numérico, proporcionando una
 * visión general rápida de las métricas importantes.
 * 
 * @returns {JSX.Element} El componente de resumen de estadísticas.
 */
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