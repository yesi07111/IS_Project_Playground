import React from 'react';
import { Box, Typography, Rating } from '@mui/material';
import { styled } from '@mui/material/styles';

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

const StyledCommentsContainer = styled(Box)({
    maxHeight: '300px',
    overflowY: 'auto',
    padding: '0 16px',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    transition: 'box-shadow 0.3s',
    '&:hover': {
        boxShadow: '0 8px 16px rgba(0, 0, 0, 0.2)',
    },
});

interface Comment {
    username: string;
    rating: number;
    comment: string;
}

interface CommentsContainerProps {
    comments: Comment[];
}

const CommentsContainer: React.FC<CommentsContainerProps> = ({ comments }) => {
    return (
        <StyledCommentsContainer>
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