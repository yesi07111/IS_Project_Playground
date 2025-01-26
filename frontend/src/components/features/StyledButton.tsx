// StyledButton.tsx
import React from 'react';
import { Button, ButtonProps } from '@mui/material';
import { styled } from '@mui/material/styles';
import { Theme } from '@mui/material/styles';

const StyledButton = styled(Button)(({ theme }: { theme: Theme }) => ({
    borderRadius: 20,
    padding: theme.spacing(1, 3),
    fontWeight: 'bold',
    transition: 'background-color 0.3s ease-in-out, transform 0.3s ease-in-out',
    '&:hover': {
        transform: 'scale(1.1)',
    },
}));

const CustomButton: React.FC<ButtonProps> = (props) => {
    return <StyledButton {...props} />;
};

export default CustomButton;