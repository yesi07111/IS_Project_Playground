import React from 'react';
import { styled } from '@mui/material/styles';
import { useNavigate } from 'react-router-dom';
import { Card, CardContent, Typography, Box, Theme, Button } from '@mui/material';
import { ResourceCardProps } from '../../interfaces/ResourceCardProps';

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
    boxShadow: theme.shadows[3],
  },
}));

const BadgeContainer = styled(Box)(({ theme }: { theme: Theme }) => ({
  position: 'absolute',
  top: theme.spacing(1),
  right: theme.spacing(1),
  display: 'flex',
  gap: theme.spacing(1),
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

const ResourceCard: React.FC<ResourceCardProps> = ({ resource }) => {
  const rol = localStorage.getItem('authUserRole');
  const navigate = useNavigate();

  const handleDefineUsageFrequency = () => {
    navigate(`/define-usage-frequency/${resource.id}`);
  };

  return (
    <StyledCard>
      <BadgeContainer>
        <Badge bgcolor={resource.condition === 'Bueno' ? '#1976d2' : resource.condition === 'Deteriorado' ? '#ffa726' : '#d32f2f'}>
          {resource.condition}
        </Badge>
      </BadgeContainer>
      <CardContent>
        <Typography
          gutterBottom
          variant="h5"
          component="div"
          sx={{
            fontWeight: 'bold',
            color: 'primary.main',
          }}
        >
          {resource.name}
        </Typography>
        <Box sx={{ mt: 2 }}>
          <Typography variant="body2" color="text.secondary">
            Tipo: {resource.type}
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Frecuencia de Uso: {resource.useFrequency}
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Ubicación: {resource.facilityLocation}
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Instalación: {resource.facilityName} ({resource.facilityType})
          </Typography>
        </Box>
        {rol === 'Educator' && (
          <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center' }}>
            <Button
              variant="contained"
              color="primary"
              onClick={handleDefineUsageFrequency}
            >
              Definir frecuencia de uso
            </Button>
          </Box>
        )}
      </CardContent>
    </StyledCard>
  );
};

export default ResourceCard;
