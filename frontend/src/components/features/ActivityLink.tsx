import React from 'react';
import { Link } from '@mui/material';
import { ActivityLinkProps } from '../../interfaces/Activity';

const ActivityLink: React.FC<ActivityLinkProps & { underline?: boolean }> = ({
    id,
    image,
    viewSuffix,
    fontSize = '1.2rem',
    textDisplayed,
    underline = true 
}) => {
    const encodedImagePath = encodeURIComponent(image);
    const url = `/activities/${id}/${encodedImagePath}/${viewSuffix}`;

    return (
        <Link
            href={url}
            color="primary"
            sx={{
                fontSize,
                textDecoration: underline ? 'underline' : 'none' // Controla el subrayado
            }}
        >
            {textDisplayed}
        </Link>
    );
};

export default ActivityLink;