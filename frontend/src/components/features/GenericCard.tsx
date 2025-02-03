import React from 'react';
import { styled } from '@mui/material/styles';
import { Card, CardContent, Typography, Box, Theme, Button } from '@mui/material';
import { GenericCardProps } from '../../interfaces/GenericCardProps';

const StyledCard = styled(Card)(({ theme }: { theme: Theme }) => ({
  width: '90%',
  minwidth: 400,
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

const GenericCard: React.FC<GenericCardProps> = ({ title, fields, badge, actions }) => {
  return (
    <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', minHeight: '10vh', minWidth: '100vw', bgcolor: 'background.default', p: 3 }}>
      <StyledCard>
        {badge && (
          <BadgeContainer>
            <Badge bgcolor={badge.color}>{badge.text}</Badge>
          </BadgeContainer>
        )}
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
            {title}
          </Typography>
          <Box sx={{ mt: 2 }}>
            {fields.map((field, index) => (
              <Typography key={index} variant="body2" color="text.secondary">
                <strong>{field.label}:</strong> {field.value}
              </Typography>
            ))}
          </Box>
          {actions && (
            <Box sx={{ mt: 2, display: 'flex', flexDirection: 'column', alignItems: 'flex-start', gap: 2, ml: 2 }}>
              {actions.map((action, index) => (
                <Button key={index} variant="contained" color="primary" onClick={action.onClick}>
                  {action.label}
                </Button>
              ))}
            </Box>
          )}
        </CardContent>
      </StyledCard>
    </Box>
  );
};

export default GenericCard;