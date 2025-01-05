import React, { useState } from 'react';
import { Box, Typography, Button, Dialog, DialogActions, DialogContent, DialogTitle, IconButton } from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';

// Definir los tipos para los usuarios
interface UserManagement {
    padres: string[];
    educators: string[];
    admins: string[];
}

const UserManagementPage = () => {
    // Definir el tipo para selectedUsers
    const [selectedUsers, setSelectedUsers] = useState<{ user: string; category: keyof UserManagement }[]>([]); // Para marcar los usuarios seleccionados
    const [showDeleteDialog, setShowDeleteDialog] = useState(false); // Para mostrar el diálogo de confirmación
    const [showInspectDialog, setShowInspectDialog] = useState(false); // Para mostrar la ventana de inspección
    const [userToInspect, setUserToInspect] = useState<string | null>(null); // Usuario que se va a inspeccionar
    
    // Especificamos que el objeto users es de tipo UserManagement
    const [users, setUsers] = useState<UserManagement>({
        padres: Array.from({ length: 10 }, (_, index) => `Padre ${index + 1}`),
        educators: Array.from({ length: 10 }, (_, index) => `Educador ${index + 1}`),
        admins: Array.from({ length: 10 }, (_, index) => `Administrador ${index + 1}`),
    });

    const handleSelectUser = (user: string, category: keyof UserManagement) => {
        if (selectedUsers.some((u) => u.user === user && u.category === category)) {
            // Si el usuario ya está seleccionado, lo deseleccionamos
            setSelectedUsers(selectedUsers.filter((u) => !(u.user === user && u.category === category)));
        } else {
            // Si el usuario no está seleccionado, lo seleccionamos
            setSelectedUsers([...selectedUsers, { user, category }]);
        }
    };

    const handleDeleteUsers = () => {
        const updatedUsers = { ...users };

        // Eliminamos los usuarios seleccionados de su respectiva categoría
        selectedUsers.forEach((selectedUser) => {
            updatedUsers[selectedUser.category] = updatedUsers[selectedUser.category].filter(
                (user) => user !== selectedUser.user
            );
        });

        setUsers(updatedUsers);
        setSelectedUsers([]); // Limpiamos la selección después de la eliminación
        setShowDeleteDialog(false); // Cerramos el diálogo
    };

    const handleInspectUser = (user: string, event: React.MouseEvent) => {
        event.stopPropagation(); // Previene que se active la selección del usuario
        setUserToInspect(user); // Establecemos el usuario que se va a inspeccionar
        setShowInspectDialog(true); // Mostramos la ventana de inspección
    };

    const handleCloseInspectDialog = () => {
        setShowInspectDialog(false); // Cerramos la ventana de inspección
        setUserToInspect(null); // Limpiamos el usuario seleccionado para inspección
    };

    return (
        <Box
            sx={{
                width: '100vw',
                minHeight: '100vh',
                backgroundColor: 'white',
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                padding: 4,
                position: 'relative',
            }}
        >
            {/* Title Section */}
            <Typography
                variant="h3"
                sx={{ textAlign: 'center', fontWeight: 700, mb: 6, color: '#2C3E50' }}
            >
                Panel de Gestión de Usuario
            </Typography>

            {/* User Management Sections */}
            <Box sx={{ display: 'flex', justifyContent: 'space-around', width: '100%', mb: 4 }}>
                {['padres', 'educators', 'admins'].map((category) => (
                    <Box
                        key={category}
                        sx={{
                            width: '30%',
                            backgroundColor: '#F0F4F8',
                            padding: 2,
                            borderRadius: 1,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            maxHeight: '400px',
                            overflowY: 'auto', // Permite el desplazamiento
                        }}
                    >
                        <Typography variant="h5" sx={{ mb: 2 }}>
                            {category === 'padres'
                                ? 'Padres'
                                : category === 'educators'
                                ? 'Educadores'
                                : 'Administradores'}
                        </Typography>

                        {/* User List with Scroll */}
                        <Box sx={{ width: '100%', mb: 2 }}>
                            {users[category as keyof UserManagement].map((user: string) => ( // Mostrar todos los usuarios sin limitar a 5
                                <Box
                                    key={user}
                                    sx={{
                                        display: 'flex',
                                        justifyContent: 'space-between',
                                        alignItems: 'center',
                                        padding: '5px',
                                        backgroundColor: selectedUsers.some(
                                            (u) => u.user === user && u.category === category
                                        )
                                            ? '#A8D8E6'
                                            : 'transparent',
                                        borderRadius: 1,
                                        mb: 1,
                                    }}
                                    onClick={() => handleSelectUser(user, category as keyof UserManagement)}
                                >
                                    <Typography variant="body1">{user}</Typography>

                                    <Button
                                        variant="contained"
                                        color="success"
                                        onClick={(event) => handleInspectUser(user, event)} // Pasamos el evento al hacer clic en Inspeccionar
                                        sx={{ marginLeft: 1 }}
                                    >
                                        Inspeccionar
                                    </Button>
                                </Box>
                            ))}
                        </Box>
                    </Box>
                ))}
            </Box>

            {/* Add User Buttons outside the scroll */}
            <Box sx={{ display: 'flex', justifyContent: 'space-around', width: '100%' }}>
                <Button
                    variant="contained"
                    color="primary"
                    sx={{ width: '30%' }}
                >
                    Agregar Padre
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    sx={{ width: '30%' }}
                >
                    Agregar Educador
                </Button>
                <Button
                    variant="contained"
                    color="primary"
                    sx={{ width: '30%' }}
                >
                    Agregar Administrador
                </Button>
            </Box>

            {/* Delete Selected Users Button outside the sliders */}
            {selectedUsers.length > 0 && (
                <Button
                    variant="contained"
                    color="error"
                    onClick={() => setShowDeleteDialog(true)}
                    sx={{ width: '100%', mt: 4 }}
                >
                    Eliminar Seleccionados
                </Button>
            )}

            {/* Confirmation Dialog for Deleting Users */}
            <Dialog open={showDeleteDialog} onClose={() => setShowDeleteDialog(false)}>
                <DialogTitle>Confirmar Eliminación</DialogTitle>
                <DialogContent>
                    <Typography>
                        ¿Está seguro de que desea eliminar los usuarios seleccionados?
                    </Typography>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setShowDeleteDialog(false)} color="primary">
                        Cancelar
                    </Button>
                    <Button onClick={handleDeleteUsers} color="error">
                        Eliminar
                    </Button>
                </DialogActions>
            </Dialog>

            {/* Inspection Dialog */}
            <Dialog open={showInspectDialog} onClose={handleCloseInspectDialog}>
                <DialogTitle sx={{ mt: 4 }}>Inspeccionar Usuario</DialogTitle> {/* El título ya está ajustado */}
                <DialogContent>
                    {/* Aquí puedes agregar los detalles de inspección que desees */}
                    <Typography>Detalles de {userToInspect}</Typography>
                </DialogContent>
                <DialogActions>
                    <Box sx={{ position: 'absolute', top: 8, right: 8 }}>
                        <IconButton onClick={handleCloseInspectDialog}>
                            <CloseIcon />
                        </IconButton>
                    </Box>
                </DialogActions>
            </Dialog>
        </Box>
    );
};

export default UserManagementPage;
