import React, { useState, useEffect, useCallback } from 'react';
import { Box, Typography, Button, Rating, Dialog, DialogActions, DialogContent, DialogTitle } from '@mui/material';
import { styled } from '@mui/material/styles';
import CommentsContainer from '../components/features/CommentsContainer';
import { SearchBar } from '../components/features/StyledSearchBar';
import ActivityLink from '../components/features/ActivityLink';
import { activityService } from '../services/activityService';
import { Activity } from '../interfaces/Activity';
import { Review } from '../interfaces/Review';
import { cacheService } from '../services/cacheService';
import { reviewService } from '../services/reviewService';
import Swal from 'sweetalert2';
import * as BadWordsFilter from 'bad-words';
import { BadWords } from '../interfaces/BadWords';

const TransparentBox = styled(Box)(({ theme }) => ({
    width: '75%',
    backgroundColor: 'transparent',
    borderRadius: 8,
    padding: theme.spacing(3),
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    transition: 'transform 0.3s, box-shadow 0.3s',
    '&:hover': {
        transform: 'scale(1.05)',
        boxShadow: '0 8px 16px rgba(0, 0, 0, 0.2)',
    },
    margin: '20px auto',
}));

const MyReviewPage: React.FC = () => {
    const [searchTerm, setSearchTerm] = useState('');
    const [reviews, setReviews] = useState<Review[]>([]);
    const [activityImages, setActivityImages] = useState<{ [id: string]: string }>({});
    const [rating, setRating] = useState<number | null>(null);
    const [originalRating, setOriginalRating] = useState(0);
    const [openRatingDialog, setOpenRatingDialog] = useState(false);
    const [currentComment, setCurrentComment] = useState('');
    const [newComment, setNewComment] = useState('');
    const [currentReview, setCurrentReview] = useState<Review>();
    const [action, setAction] = useState('');
    const viewSuffix = "ReviewView";

    const filter = new BadWordsFilter.Filter();
    filter.addWords(...BadWords);

    useEffect(() => {
        const fetchReviews = async () => {
            const userId = localStorage.getItem('authId');
            if (!userId) return;

            try {
                const response = await reviewService.getAllReviews([
                    { type: 'Casos de Uso', useCase: 'MyReviewView' },
                    { type: 'UserId', value: userId },
                ]);
                setReviews(response.result as Review[]);
            } catch (error) {
                console.error('Error obteniendo las reseñas:', error);
            }
        };

        fetchReviews();
    }, []);

    const fetchActivityImages = useCallback(async (activityIds: string[]) => {
        const cachedImages = cacheService.loadImages();
        const cachedActivities = cacheService.loadActivities();
        const missingIds = activityIds.filter(id => !cachedImages[id]);
        if (missingIds.length > 0) {
            try {
                const response = await activityService.getAllActivities([
                    { type: 'Casos de Uso', useCase: 'ReviewView' }
                ]);
                const activitiesArray = Array.isArray(response.result) ? response.result as Activity[] : [];
                const newImagesMap: { [id: string]: string } = {};
                const newActivitiesMap: { [id: string]: Activity } = {};

                activitiesArray.forEach(activity => {
                    if (missingIds.includes(activity.id)) {
                        newImagesMap[activity.id] = activity.image;
                        newActivitiesMap[activity.id] = activity;
                    }
                });

                const updatedImages = { ...cachedImages, ...newImagesMap };
                const updatedActivities = [...cachedActivities, ...Object.values(newActivitiesMap)].filter((activity, index, self) =>
                    index === self.findIndex((a) => a.id === activity.id)
                );

                cacheService.saveImages(updatedImages);
                cacheService.saveActivities(updatedActivities);
                setActivityImages(updatedImages);
            } catch (error) {
                console.error('Error obteniendo imágenes de actividades:', error);
            }
        } else {
            setActivityImages(cachedImages);
        }
    }, []);

    useEffect(() => {
        const activityIds = reviews.map(review => review.activityId);
        fetchActivityImages(activityIds);
    }, [reviews, fetchActivityImages]);

    const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(event.target.value);
    };

    const handleEditReview = (review: Review) => {
        Swal.fire({
            title: 'Editar Reseña',
            input: 'textarea',
            inputLabel: 'Comentario',
            inputValue: review.comment,
            showCancelButton: true,
            confirmButtonText: 'Aceptar',
            cancelButtonText: 'Cancelar',
            preConfirm: (comment) => {
                if (filter.isProfane(comment)) {
                    Swal.showValidationMessage('El comentario contiene palabras inapropiadas.');
                }
                return comment;
            }
        }).then((result) => {
            if (result.isConfirmed) {
                setCurrentComment(review.comment);
                setNewComment(result.value);
                setOriginalRating(review.rating);
                setCurrentReview(review);
                setAction("edit");
                setOpenRatingDialog(true);
            }
        });
    };

    const handleDeleteReview = (reviewId: string) => {
        Swal.fire({
            title: '¿Estás seguro?',
            text: "¡No podrás revertir esto!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, eliminarlo',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                try {
                    reviewService.deleteReview(reviewId, "SoftDelete");
                    Swal.fire("Hecho!", "Se ha eliminado con éxito la reseña.", "success");
                    // Actualizar el estado local
                    setReviews(prevReviews => prevReviews.map(review =>
                        review.reviewId === reviewId ? { ...review, comment: 'No hay comentario aún.', rating: 0 } : review
                    ));
                } catch (error) {
                    Swal.fire("Error", "Ha ocurrido un error inesperado al intentar eliminar la reseña, por favor intente de nuevo.", "error");
                    console.error("Ha ocurrido un error al intentar eliminar la reseña: ", error);
                }
            }
        });
    };

    const handleCreateComment = (review: Review) => {
        Swal.fire({
            title: 'Crear Comentario',
            input: 'textarea',
            inputLabel: 'Comentario',
            showCancelButton: true,
            confirmButtonText: 'Aceptar',
            cancelButtonText: 'Cancelar',
            preConfirm: (comment) => {
                if (filter.isProfane(comment)) {
                    Swal.showValidationMessage('El comentario contiene palabras inapropiadas.');
                }
                return comment;
            }
        }).then((result) => {
            if (result.isConfirmed) {
                setCurrentComment(result.value);
                setCurrentReview(review);
                setAction("create");
                setOpenRatingDialog(true);
            }
        });
    };

    const handleRatingSubmit = () => {
        if (action === "edit") {
            if (newComment === currentComment && originalRating && rating === originalRating) {
                Swal.fire("Sin cambios", "No se realizaron cambios en la reseña.", "info");
            } else {
                try {
                    if (currentReview) {
                        reviewService.updateReview({
                            ActivityDateId: currentReview.activityId,
                            UserId: localStorage.getItem("authId") ?? '',
                            Comment: newComment ?? "",
                            Rating: rating ?? 0,
                        }).then(() => {
                            Swal.fire("Hecho!", "La reseña se ha actualizado con éxito.", "success");
                            // Actualizar el estado local
                            setReviews(prevReviews => prevReviews.map(review =>
                                review.reviewId === currentReview.reviewId ? { ...review, comment: newComment, rating: rating ?? 0 } : review
                            ));
                        });
                    }
                } catch (error) {
                    Swal.fire("Error", "Ha ocurrido un error inesperado al intentar actualizar la reseña, por favor intente de nuevo.", "error");
                    console.error("Ha ocurrido un error al intentar actualizar la reseña: ", error);
                }
            }
        } else {
            try {
                if (currentReview) {
                    reviewService.createReview({
                        ActivityDateId: currentReview.activityId,
                        UserId: localStorage.getItem("authId") ?? '',
                        Comment: currentComment,
                        Rating: rating ?? 0,
                    }).then(() => {
                        Swal.fire("Hecho!", "La reseña se ha creado con éxito.", "success");
                        // Actualizar el estado local
                        setReviews(prevReviews => prevReviews.map(review =>
                            review.reviewId === currentReview.reviewId ? { ...review, comment: currentComment, rating: rating ?? 0 } : review
                        ));
                    });
                }
            } catch (error) {
                Swal.fire("Error", "Ha ocurrido un error inesperado al intentar crear la reseña, por favor intente de nuevo.", "error");
                console.error("Ha ocurrido un error al intentar crear la reseña: ", error);
            }
        }

        setOpenRatingDialog(false);
    };

    const filteredReviews = reviews.filter(review =>
        review.comment.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <Box
            sx={{
                width: '100vw',
                minHeight: '100vh',
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'center',
            }}
        >
            <SearchBar
                searchTerm={searchTerm}
                handleSearchChange={handleSearchChange}
                labelText="Comentarios"
            />
            {filteredReviews.map((review, index) => (
                <TransparentBox key={index}>
                    <Typography variant="h5" component="h2" gutterBottom>
                        <ActivityLink
                            id={review.activityId}
                            image={activityImages[review.activityId] || ''}
                            viewSuffix={viewSuffix}
                            textDisplayed={`Actividad: ${review.activityName}`}
                            underline={false}
                        />
                    </Typography>
                    <CommentsContainer
                        comments={[
                            {
                                username: 'Yo',
                                comment: review.comment || 'No hay comentario aún.',
                                rating: review.rating !== -1 ? review.rating : 0,
                            },
                        ]}
                        invisible={true}
                    />
                    {review.comment && review.rating !== 0 ? (
                        <Box>
                            <Button onClick={() => handleEditReview(review)}>Editar</Button>
                            <Button onClick={() => handleDeleteReview(review.reviewId)}>Eliminar</Button>
                        </Box>
                    ) : (
                        <Button onClick={() => handleCreateComment(review)}>Comentar</Button>
                    )}
                </TransparentBox>
            ))}

            <Dialog
                open={openRatingDialog}
                onClose={() => setOpenRatingDialog(false)}
                maxWidth="sm"
            >
                <DialogTitle>Califica la actividad</DialogTitle>
                <DialogContent>
                    <Rating
                        name="rating"
                        value={rating}
                        onChange={(event, newValue) => {
                            setRating(newValue);
                        }}
                        precision={1}
                        max={5}
                        size="large"
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setOpenRatingDialog(false)}>Cancelar</Button>
                    <Button onClick={handleRatingSubmit} color="primary">Guardar</Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
};

export default MyReviewPage;