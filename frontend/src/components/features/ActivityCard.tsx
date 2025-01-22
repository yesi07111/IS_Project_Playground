import React from 'react';
import { styled } from '@mui/material/styles';
import {
  Card,
  CardContent,
  CardMedia,
  Typography,
  Rating,
  Box,
  Theme,
  Link
} from '@mui/material';
import { ActivityCardProps } from '../../interfaces/Activity';
import { useLocation } from 'react-router-dom';
import { dateService } from '../../services/dateService';

const StyledCard = styled(Card)(({ theme }: { theme: Theme }) => ({
  width: '90%',
  maxWidth: 400,
  height: '100%',
  display: 'flex',
  flexDirection: 'column',
  transition: 'transform 0.2s',
  position: 'relative',
  '&:hover': {
    transform: 'scale(1.03)',
    boxShadow: theme.shadows[3]
  }
}));

const StyledBox = styled(Box)(({ theme }: { theme: Theme }) => ({
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'space-between',
  marginTop: theme.spacing(1),
  borderTop: `1px solid ${theme.palette.divider}`,
  paddingTop: theme.spacing(2)
}));

const BadgeContainer = styled(Box)(({ theme }: { theme: Theme }) => ({
  position: 'absolute',
  top: theme.spacing(1),
  right: theme.spacing(1),
  display: 'flex',
  gap: theme.spacing(1) // Espaciado entre los badges
}));

const Badge = styled(Box)<{ bgcolor: string }>(({ theme, bgcolor }) => ({
  backgroundColor: bgcolor,
  color: theme.palette.common.white,
  borderRadius: '12px',
  padding: theme.spacing(0.5, 1),
  fontSize: '0.75rem',
  fontWeight: 'bold',
  boxShadow: theme.shadows[2],
}));


const ActivityCard: React.FC<ActivityCardProps> = ({ activity }) => {
  const location = useLocation();
  const { formattedDate, formattedTime } = dateService.parseDate(activity.date);
  const encodedImagePath = encodeURIComponent(activity.image);
  const viewSuffix = location.pathname === '/activities' ? 'ActivityView' : 'ReviewView';

  return (
    <StyledCard>
      <BadgeContainer>
        {activity.isNew && (
          <Badge bgcolor="orange"> {/* Cambiado a un color naranja para el badge "Nueva" */}
            Nueva!
          </Badge>
        )}
        <Badge bgcolor={activity.isPublic === 'true' ? '#1976d2' : '#d32f2f'}> {/* Azul para Pública, Rojo para Privada */}
          {activity.isPublic === 'true' ? 'Pública' : 'Privada'}
        </Badge>
      </BadgeContainer>
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
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
          <Box sx={{ flex: 1, mr: 2 }}>
            <Typography
              variant="body2"
              color="text.secondary"
            >
              Fecha: {formattedDate}
            </Typography>
            <Typography
              variant="body2"
              color="text.secondary"
            >
              Hora: {formattedTime}
            </Typography>
            <Typography
              variant="body2"
              color="text.secondary"
            >
              Participantes: {activity.currentCapacity}/{activity.maximumCapacity}
            </Typography>
          </Box>
        </Box>
        <StyledBox>
          {location.pathname !== '/activities' ? (
            <Box sx={{ display: 'flex', alignItems: 'center', width: '100%', justifyContent: 'space-between' }}>
              <Box sx={{ display: 'flex', alignItems: 'center' }}>
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
              </Box>
              <Link href={`/activities/${activity.id}/${encodedImagePath}/${viewSuffix}`} color="primary" sx={{ fontSize: '1.2rem' }}>
                Ver detalles
              </Link>
            </Box>
          ) : (
            <Box sx={{ display: 'flex', justifyContent: 'center', width: '100%' }}>
              <Link href={`/activities/${activity.id}/${encodedImagePath}/${viewSuffix}`} color="primary" sx={{ fontSize: '1.5rem' }}>
                Ver detalles
              </Link>
            </Box>
          )}
        </StyledBox>
      </CardContent>
    </StyledCard>
  );
};

export default ActivityCard;
