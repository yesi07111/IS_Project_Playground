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

interface Activity {
  id: number;
  name: string;
  description: string;
  image: string;
  rating: number;
}

interface ActivityCardProps {
  activity: Activity;
}

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

const StyledBox = styled(Box)(({ theme }: { theme: Theme }) => ({
  display: 'flex',
  alignItems: 'center',
  marginTop: theme.spacing(1),
  borderTop: `1px solid ${theme.palette.divider}`,
  paddingTop: theme.spacing(2)
}));

const ActivityCard: React.FC<ActivityCardProps> = ({ activity }) => {
  return (
    <StyledCard>
      <CardMedia
        component="div"
        sx={{
          height: 280, // Aumentado a 280px para dar más espacio vertical
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