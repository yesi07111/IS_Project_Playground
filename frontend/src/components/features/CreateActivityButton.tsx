import React from 'react';
import { styled } from '@mui/material/styles';
import { Card, Box, Typography } from '@mui/material';
import { activityService } from '../../services/activityService';
import { useNavigate } from 'react-router-dom';

const ButtonCard = styled(Card)(({ theme }) => ({
  width: '90%',
  maxWidth: 400,
  height: '100%',
  display: 'flex',
  flexDirection: 'column',
  justifyContent: 'center',
  alignItems: 'center',
  textAlign: 'center',
  transition: 'transform 0.2s',
  position: 'relative',
  cursor: 'pointer',
  backgroundColor: theme.palette.background.default,
  color: theme.palette.text.primary,
  opacity: 0.9,
  '&:hover': {
    transform: 'scale(1.03)',
    boxShadow: theme.shadows[4],
    opacity: 1,
  },
}));

const ButtonBox = styled(Box)(({ theme }) => ({
  padding: theme.spacing(3),
  border: `2px dashed ${theme.palette.primary.main}`,
  borderRadius: theme.spacing(1),
  backgroundColor: theme.palette.action.hover,
}));

const CreateActivityButton: React.FC = () => {
  const navigate = useNavigate();

  const handleClick = () => {
    console.log('Solicitar crear nueva actividad');
    navigate(`/activity-form?useCase=${'CreateBoth'}`);
  };

  return (
    <ButtonCard onClick={handleClick}>
      <ButtonBox>
        <Typography variant="h6" component="div" sx={{ fontWeight: 'bold', mb: 1 }}>
          Solicitar crear nueva actividad
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Haz clic aquí para agregar una nueva actividad
        </Typography>
      </ButtonBox>
    </ButtonCard>
  );
};

export default CreateActivityButton;
