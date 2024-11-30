import React from 'react';
import { styled } from '@mui/material/styles';
import {
  Card,
  CardContent,
  CardMedia,
  Typography,
  Rating,
  Box,
  Theme
} from '@mui/material';
import { ActivityCardProps } from '../../interfaces/ActivityCardProps';

/**
 * Componente estilizado de tarjeta que representa una actividad.
 * 
 * Este componente utiliza Material-UI para crear una tarjeta que muestra
 * información sobre una actividad, incluyendo una imagen, nombre, descripción
 * y calificación. La tarjeta tiene un efecto de escala al pasar el ratón sobre ella.
 * 
 * @param {Theme} theme - Tema de Material-UI para aplicar estilos.
 * @returns {object} Estilos aplicados al componente Card.
 */
const StyledCard = styled(Card)(({ theme }: { theme: Theme }) => ({
  width: '100%',
  height: '100%',
  display: 'flex',
  flexDirection: 'column',
  transition: 'transform 0.2s',
  '&:hover': {
    transform: 'scale(1.03)',
    boxShadow: theme.shadows[3]
  }
}));

/**
 * Componente estilizado de caja que contiene la calificación de la actividad.
 * 
 * Este componente utiliza Material-UI para crear una caja que muestra la calificación
 * de la actividad y un borde superior. Se utiliza dentro del contenido de la tarjeta.
 * 
 * @param {Theme} theme - Tema de Material-UI para aplicar estilos.
 * @returns {object} Estilos aplicados al componente Box.
 */
const StyledBox = styled(Box)(({ theme }: { theme: Theme }) => ({
  display: 'flex',
  alignItems: 'center',
  marginTop: theme.spacing(1),
  borderTop: `1px solid ${theme.palette.divider}`,
  paddingTop: theme.spacing(2)
}));

/**
 * Componente de tarjeta de actividad que muestra detalles de una actividad.
 * 
 * Este componente recibe las propiedades de una actividad a través de `ActivityCardProps`
 * y muestra una tarjeta con la imagen de la actividad, su nombre, descripción y calificación.
 * Utiliza componentes de Material-UI para la estructura y el estilo.
 * 
 * @param {ActivityCardProps} props - Propiedades del componente, incluyendo los detalles de la actividad.
 * @returns {JSX.Element} El componente de tarjeta de actividad.
 */
const ActivityCard: React.FC<ActivityCardProps> = ({ activity }) => {
  return (
    <StyledCard>
      <CardMedia
        component="div"
        sx={{
          height: 280,
          backgroundImage: `url(${activity.image})`,
          backgroundSize: 'cover',
          backgroundPosition: 'center'
        }}
      />
      <CardContent>
        <Typography
          gutterBottom
          variant="h5"
          component="div"
          sx={{
            fontWeight: 'bold',
            color: 'primary.main'
          }}
        >
          {activity.name}
        </Typography>
        <Typography
          variant="body2"
          color="text.secondary"
          sx={{ mb: 2 }}
        >
          {activity.description}
        </Typography>
        <StyledBox>
          <Rating
            value={activity.rating}
            readOnly
            precision={0.5}
          />
          <Typography
            variant="body2"
            color="text.secondary"
            sx={{ ml: 1 }}
          >
            ({activity.rating}/5)
          </Typography>
        </StyledBox>
      </CardContent>
    </StyledCard>
  );
};

export default ActivityCard;