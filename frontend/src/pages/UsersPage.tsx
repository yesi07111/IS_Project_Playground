import { useCallback, useEffect, useState } from "react";
import { usePDF } from 'react-to-pdf';
import { ListUserResponse, UserResponse } from "../interfaces/User";
import { UserFilters } from "../interfaces/Filters";
import { userService } from "../services/userService";
import { cacheService } from "../services/cacheService";
import { Box, Button, Collapse, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, FormControl, Grid2, IconButton, Input, InputLabel, MenuItem, Pagination, Select, SelectChangeEvent, Typography } from "@mui/material";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import { SearchBar } from "../components/features/StyledSearchBar";
import { FilterSelect } from "../components/features/StyledFilters";
import GenericCard from "../components/features/GenericCard";
import { useAuth } from "../components/auth/authContext";
import { useNavigate } from "react-router-dom";

const UsersPage: React.FC<{ reload: boolean }> = ({ reload }) => {
    const [searchTerm, setSearchTerm] = useState(''); //termino de busqueda ingresado por el admin
    const [currentPage, setCurrentPage] = useState(1); //pagina actual en paginacion
    const [selectedFilters, setSelectedFilters] = useState<string[]>([]); //filtros seleccionados por el admin
    const [filtersOpen, setFiltersOpen] = useState(false); //seccion de filtros abierta o no
    const [firstName, setFirstName] = useState(''); //nombre del usuario elegido
    const [lastName, setLastName] = useState(''); //apellido del usuario elegido
    const [username, setUserName] = useState(''); //nombre de usuario elegido
    const [email, setEmail] = useState(''); //email del usuario elegido
    const [role, setRole] = useState(''); //rol elegido
    const [selectedUser, setSelectedUser] = useState<string | null>(null);
    const [openDialog, setOpenDialog] = useState(false);
    const [users, setUsers] = useState<UserResponse[]>([]); //lista de recursos cargados desd ela API
    const { isAuthenticated } = useAuth();
    const myRole = localStorage.getItem('authUserRole');
    const navigate = useNavigate();

    const { toPDF, targetRef } = usePDF({ filename: 'Usuarios.pdf' });

    const usersPerPage = 9;

    const fetchAllUsers = useCallback(async () => {
        try {
            const users: ListUserResponse = await userService.getAllUsers({ useCase: 'AsFilter' });
            const usersArray: UserResponse[] = Array.isArray(users) ? users : Array.from(users.users);

            setUsers(usersArray);
            cacheService.saveUsers(usersArray);
        } catch (error) {
            console.error('Error fetching users:', error);
            const cachedResources = cacheService.loadUsers();
            setUsers(cachedResources || []);
        }
    }, []);

    const fetchInitialData = useCallback(async () => {
        await Promise.all([fetchAllUsers()]);
    }, [fetchAllUsers]);

    useEffect(() => {
        fetchInitialData();
    }, []);

    useEffect(() => {
        if (selectedFilters.length === 0) {
            fetchAllUsers();
        }
    }, [selectedFilters]);

    useEffect(() => {
        if (reload) {
            cacheService.clearCache()
            fetchInitialData();
        }
    }, [reload]);

    useEffect(() => {
        const cachedUsers = cacheService.loadUsers();
        if (cachedUsers.length > 0) {
            setUsers(cachedUsers);
        }
    }, []);

    const filteredUsers = users.filter(user => {
        return user.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
            user.lastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
            user.username.toLowerCase().includes(searchTerm.toLowerCase()) ||
            user.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
            user.rol.toLowerCase().includes(searchTerm.toLowerCase());
    });

    const totalPages = Math.ceil(filteredUsers.length / usersPerPage);

    const currentUsers = filteredUsers.slice(
        (currentPage - 1) * usersPerPage,
        currentPage * usersPerPage
    );

    const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(event.target.value);
        setCurrentPage(1);
    };

    const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setCurrentPage(value);
    };

    const handleFilterChange = (event: SelectChangeEvent<string[]>) => {
        const value = event.target.value as string[];

        const removedFilters = selectedFilters.filter(filter => !value.includes(filter));

        removedFilters.forEach(filter => {
            switch (filter) {
                case "Nombre":
                    setFirstName('');
                    break;
                case "Apellido":
                    setLastName('');
                    break;
                case "Nombre de Usuario":
                    setUserName('');
                    break;
                case "Email":
                    setEmail('');
                    break;
                case "Rol de Usuario":
                    setRole('');
                    break;
                default:
                    break;
            }
        });

        setSelectedFilters(value);
        setFiltersOpen(value.length > 0);
    }

    const toggleFiltersOpen = () => {
        setFiltersOpen(!filtersOpen);
    };

    const handleApplyFilters = async () => {
        // Reiniciar la paginación a la primera página
        setCurrentPage(1);

        const filters: UserFilters = {
            useCase: 'AsFilter'
        };

        if (firstName) {
            filters.firstName = firstName;
        }
        if (lastName) {
            filters.lastName = lastName;
        }
        if (username) {
            filters.userName = username;
        }
        if (email) {
            filters.email = email;
        }
        if (role) {
            filters.rol = role;
        }

        try {
            const users: ListUserResponse = await userService.getAllUsers(filters);
            const usersArray: UserResponse[] = Array.isArray(users) ? users : Array.from(users.users);

            setUsers(usersArray);
            cacheService.saveUsers(usersArray);
        } catch (error) {
            console.error('Error fetching users:', error);
            const cachedResources = cacheService.loadUsers();
            setUsers(cachedResources || []);
        }
    }

    const handleRemoveUser = (id: string) => {
        setSelectedUser(id); // Guardar datos relevantes
        setOpenDialog(true); // Abrir diálogo de confirmación
    };

    const cancelRemoveUser = () => {
        setOpenDialog(false); // Cerrar el diálogo sin hacer nada
        setSelectedUser(null); // Limpiar selección
    };

    const confirmRemoveUser = async () => {
        if (selectedUser) {
            await userService.deleteUser(selectedUser);
            setOpenDialog(false); // Cerrar el diálogo
            setSelectedUser(null); // Limpiar selección
        }
    };

    const menuItems = [
        { label: "Nombre", value: "Nombre" },
        { label: "Apellido", value: "Apellido" },
        { label: "Nombre de Usuario", value: "Nombre de Usuario" },
        { label: "Email", value: "Email" },
        { label: "Rol de Usuario", value: "Rol de Usuario" },
    ];

    return (
        <Box sx={{ width: '100vw', minHeight: '100vh', py: 4, px: 2, backgroundColor: '#f8f9fa', overflowY: 'auto', }}>
            {/* Buscador con filtros */}
            <Box sx={{ display: 'flex', justifyContent: 'center', mb: 4 }}>
                <SearchBar searchTerm={searchTerm} handleSearchChange={handleSearchChange} labelText='Usuarios' />
                <FilterSelect
                    selectedFilters={selectedFilters}
                    handleFilterChange={handleFilterChange}
                    handleApplyFilters={handleApplyFilters}
                    menuItems={menuItems}
                />
            </Box>
            {/* Sección plegable de filtros */}
            {selectedFilters.length > 0 && (
                <Box sx={{ mb: 4, p: 2, border: '1px solid #ccc', borderRadius: '4px' }}>
                    <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                        <Typography variant="h6">Filtros</Typography>
                        <IconButton onClick={toggleFiltersOpen}>
                            {filtersOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                        </IconButton>
                    </Box>
                    <Collapse in={filtersOpen}>
                        {selectedFilters.map((filter) => (
                            <Box key={filter} sx={{ mt: 2 }}>
                                {filter === "Rol de Usuario" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>Rol de Usuario:</Typography>
                                        <FormControl sx={{ ml: 2, minWidth: 250 }} variant="outlined">
                                            <InputLabel id="role-select-label">Escoger Rol</InputLabel>
                                            <Select
                                                labelId="role-select-label"
                                                value={role}
                                                onChange={(e) => setRole(e.target.value)}
                                                label="Escoger Rol del Usuario"
                                            >
                                                <MenuItem value="Parent">Padre</MenuItem>
                                                <MenuItem value="Educator">Educador</MenuItem>
                                                <MenuItem value="Admin">Administrador</MenuItem>
                                            </Select>
                                        </FormControl>
                                    </Box>
                                )}
                                {filter === "Nombre" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>Buscar usuario por nombre:</Typography>
                                        <FormControl sx={{ ml: 2, width: 200 }} variant="outlined">
                                            <Input
                                                type="text"
                                                value={firstName || ''}
                                                onChange={(e) => {
                                                    const value = e.target.value;
                                                    setFirstName(value || '');
                                                }}
                                                placeholder="Escribe el nombre"
                                                sx={{
                                                    textAlign: 'left',
                                                    '&::placeholder': {
                                                        color: '#aaa',
                                                        fontStyle: 'italic',
                                                    },
                                                }}
                                            />
                                        </FormControl>
                                    </Box>
                                )}
                                {filter === "Apellido" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>Buscar usuario por apellido:</Typography>
                                        <FormControl sx={{ ml: 2, width: 200 }} variant="outlined">
                                            <Input
                                                type="text"
                                                value={lastName || ''}
                                                onChange={(e) => {
                                                    const value = e.target.value;
                                                    setLastName(value || '');
                                                }}
                                                placeholder="Escribe el apellido"
                                                sx={{
                                                    textAlign: 'left',
                                                    '&::placeholder': {
                                                        color: '#aaa',
                                                        fontStyle: 'italic',
                                                    },
                                                }}
                                            />
                                        </FormControl>
                                    </Box>
                                )}
                                {filter === "Nombre de Usuario" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>Buscar usuario por nombre de usuario:</Typography>
                                        <FormControl sx={{ ml: 2, width: 200 }} variant="outlined">
                                            <Input
                                                type="text"
                                                value={username || ''}
                                                onChange={(e) => {
                                                    const value = e.target.value;
                                                    setUserName(value || '');
                                                }}
                                                placeholder="Escribe el nombre de usuario"
                                                sx={{
                                                    textAlign: 'left',
                                                    '&::placeholder': {
                                                        color: '#aaa',
                                                        fontStyle: 'italic',
                                                    },
                                                }}
                                            />
                                        </FormControl>
                                    </Box>
                                )}
                                {filter === "Email" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>Buscar usuario por email:</Typography>
                                        <FormControl sx={{ ml: 2, width: 200 }} variant="outlined">
                                            <Input
                                                type="text"
                                                value={email || ''}
                                                onChange={(e) => {
                                                    const value = e.target.value;
                                                    setEmail(value || '');
                                                }}
                                                placeholder="Escribe el email"
                                                sx={{
                                                    textAlign: 'left',
                                                    '&::placeholder': {
                                                        color: '#aaa',
                                                        fontStyle: 'italic',
                                                    },
                                                }}
                                            />
                                        </FormControl>
                                    </Box>
                                )}

                            </Box>
                        ))}
                    </Collapse>
                </Box>
            )}

            {/* Botón de "Crear Nuevo Usuario" visible para Admins */}
            {myRole === 'Admin' && isAuthenticated && (
                <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center', gap: 2 }}>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={() => navigate('/createUser')}
                    >
                        Crear Nuevo Usuario
                    </Button>
                </Box>
            )}

            <Box sx={{ mt: 2, display: 'flex', justifyContent: 'center', gap: 2 }}>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={() => toPDF()}
                >
                    Exportar a PDF
                </Button>
            </Box>

            {/*  Separador entre el botón y el listado de usuarios  */}
            <Box sx={{ height: 16 }} />

            {/* Grid de usuaios */}
            <Grid2 ref={targetRef} container spacing={4}>
                {currentUsers.map(user => (
                    <Grid2
                        size={{ xs: 12, sm: 6, md: 4 }}
                        key={user.id}
                        sx={{
                            display: 'flex',
                            justifyContent: 'center',
                        }}
                    >
                        <GenericCard
                            title={`${user.firstName} ${user.lastName}`}
                            fields={[
                                { label: 'Nombre de Usuario', value: user.username },
                                { label: 'Email', value: user.email },
                                { label: 'Rol', value: user.rol },
                            ]}
                            actions={myRole === 'Admin' && isAuthenticated ?
                                [
                                    {
                                        label: 'Eliminar Usuario Permanentemente',
                                        onClick: () => handleRemoveUser(user.id),
                                    },
                                ] : []}
                        />
                        {/* <UserCard user={user} /> */}
                    </Grid2>
                ))}
            </Grid2>

            {/* Paginación */}
            <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
                <Pagination
                    count={totalPages}
                    page={currentPage}
                    onChange={handlePageChange}
                    color="primary"
                />
            </Box>

            {/* Diálogo de confirmación */}
            <Dialog open={openDialog} onClose={cancelRemoveUser}>
                <DialogTitle>Confirmación</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        ¿Estás seguro de que quieres eliminar este usuario PERMANENTEMENTE?
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={cancelRemoveUser} color="primary">
                        No
                    </Button>
                    <Button onClick={confirmRemoveUser} color="primary" autoFocus>
                        Sí
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    )
}

export default UsersPage;
