import React from 'react';
import { Link as MuiLink } from '@mui/material';
import { useNavigate } from 'react-router-dom';

// Definición de tipos de las props para TypeScript
interface UserLinkProps {
    username: string;
    userId: string;
}

// Componente UserLink
const UserLink: React.FC<UserLinkProps> = ({ username, userId }) => {
    const navigate = useNavigate();

    const handleClick = (event: React.MouseEvent<HTMLAnchorElement>) => {
        event.preventDefault(); // Evita el comportamiento predeterminado del enlace
        navigate(`/profile/${userId}`); // Navega a la página del usuario
    };

    return (
        <MuiLink
            href={`/profile/${userId}`}
            onClick={handleClick}
            sx={{
                color: 'inherit',
                textDecoration: 'none',
                cursor: 'pointer',
                transition: 'color 0.3s',
                '&:hover': {
                    color: 'blue',
                },
            }}
        >
            {' @' + username}
        </MuiLink>
    );
};

export default UserLink;