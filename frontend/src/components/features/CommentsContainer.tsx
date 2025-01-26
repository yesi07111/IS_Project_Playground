import React from 'react';
import { Box, Typography, Rating } from '@mui/material';
import { styled } from '@mui/material/styles';
import { CommentsContainerProps } from '../../interfaces/User';

const CommentBox = styled(Box)(({ theme }) => ({
    backgroundColor: '#f9f9f9',
    borderRadius: 8,
    padding: theme.spacing(2),
    boxShadow: '0 2px 4px rgba(0, 0, 0, 0.1)',
    marginBottom: theme.spacing(2),
    '&:hover': {
        backgroundColor: '#f0f0f0',
    },
}));

const StyledCommentsContainer = styled(Box)<{ invisible: boolean }>(({ invisible }) => ({
    maxHeight: '300px',
    overflowY: 'auto',
    padding: '0 16px',
    boxShadow: invisible ? 'none' : '0 4px 8px rgba(0, 0, 0, 0.1)',
    transition: 'box-shadow 0.3s',
    '&:hover': {
        boxShadow: invisible ? 'none' : '0 8px 16px rgba(0, 0, 0, 0.2)',
    },
}));

const CommentsContainer: React.FC<CommentsContainerProps> = ({ comments, invisible = false }) => {
    return (
        <StyledCommentsContainer invisible={invisible}>
            {comments.map((comment, index) => (
                <CommentBox key={index}>
                    <Typography variant="subtitle1" sx={{ fontWeight: 'bold', color: '#ff6347' }}>
                        {comment.username}
                    </Typography>
                    <Rating value={comment.rating} readOnly precision={0.5} sx={{ marginBottom: 1 }} />
                    <Typography variant="body2" sx={{ color: '#555' }}>
                        {comment.comment}
                    </Typography>
                </CommentBox>
            ))}
        </StyledCommentsContainer>
    );
};

export default CommentsContainer;