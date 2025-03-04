import React from 'react';
import Modal from 'react-modal';
import { Formik, Form } from 'formik';
import * as Yup from 'yup';
import { Button, TextField, Typography } from '@mui/material';

Modal.setAppElement('#root'); // Asegúrate de que el elemento raíz esté configurado

interface ReservationModalProps {
    isOpen: boolean;
    onRequestClose: () => void;
    availableSpots: number;
    onSubmit: (values: { amount: number; comments: string }) => void;
}

const ReservationModal: React.FC<ReservationModalProps> = ({ isOpen, onRequestClose, availableSpots, onSubmit }) => {
    const validationSchema = Yup.object().shape({
        amount: Yup.number()
            .required("Por favor, ingrese un número.")
            .min(1, "Por favor, ingrese un número mayor que 0.")
            .max(availableSpots, `La cantidad de niños no puede superar la disponibilidad actual de ${availableSpots}.`),
        comments: Yup.string()
            .max(150, "Los comentarios adicionales no pueden exceder los 150 caracteres.")
    });

    return (
        <Modal
            isOpen={isOpen}
            onRequestClose={onRequestClose}
            contentLabel="Reserva de Actividad"
            style={{
                content: {
                    top: '50%',
                    left: '50%',
                    right: 'auto',
                    bottom: 'auto',
                    marginRight: '-50%',
                    transform: 'translate(-50%, -50%)',
                    padding: '20px',
                    borderRadius: '10px',
                    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
                    zIndex: 1000, // Asegura que el modal esté por encima de otros elementos
                },
                overlay: {
                    zIndex: 1000, // Asegura que el overlay esté por encima de otros elementos
                }
            }}
        >
            <Typography variant="h6">Reserva de Actividad</Typography>
            <Formik
                initialValues={{ amount: '', comments: '' }}
                validationSchema={validationSchema}
                onSubmit={(values) => {
                    const parsedValues = {
                        amount: Number(values.amount),
                        comments: values.comments
                    };
                    onSubmit(parsedValues);
                    onRequestClose();
                }}
            >
                {({ handleSubmit, handleChange, values, errors, touched }) => (
                    <Form onSubmit={handleSubmit}>
                        <div>
                            <TextField
                                label="Cantidad de niños"
                                type="number"
                                name="amount"
                                value={values.amount}
                                onChange={handleChange}
                                error={touched.amount && Boolean(errors.amount)}
                                helperText={touched.amount && errors.amount}
                                fullWidth
                                margin="normal"
                            />
                        </div>
                        <div>
                            <TextField
                                label="Comentarios adicionales (Opcional)"
                                name="comments"
                                value={values.comments}
                                onChange={handleChange}
                                error={touched.comments && Boolean(errors.comments)}
                                helperText={touched.comments && errors.comments}
                                fullWidth
                                margin="normal"
                                multiline
                                rows={4}
                            />
                        </div>
                        <Button type="submit" variant="contained" color="primary" style={{ marginRight: '10px' }}>
                            Reservar
                        </Button>
                        <Button type="button" variant="outlined" onClick={onRequestClose}>
                            Cancelar
                        </Button>
                    </Form>
                )}
            </Formik>
        </Modal>
    );
};

export default ReservationModal;