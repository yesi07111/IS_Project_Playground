import React, { useState } from 'react';
import { Box, Typography, Button, Dialog, DialogActions, DialogContent, DialogTitle, IconButton, Collapse, FormControl, Select, InputLabel, MenuItem, Input, Grid2, Pagination } from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import { SearchBar } from '../components/features/StyledSearchBar';
import { FilterSelect } from '../components/features/StyledFilters';


const UserManagementPage: React.FC<{ reload: boolean }> = ({ reload }) => {
    const [searchTerm, setSearchTerm] = useState(''); //termino de busqueda ingresado por el usuario
    const [currentPage, setCurrentPage] = useState(1); //pagina actual en paginacion
    const [selectedFilters, setSelectedFilters] = useState<string[]>([]); //filtros seleccionados por el usuario
    const [filtersOpen, setFiltersOpen] = useState(false); //seccion de filtros abierta o no
    const [selectedPersonName, setSelectedPersonName] = useState<string>(''); //filtra por nombre de la persona
    const [selectedPersonLastName, setSelectedPersonLastName] = useState<string>(''); //filtra por apellido de la persona
    const [selectedUserName, setSelectedUserName] = useState<string>(''); //filtra por nombre de usuario
    const [selectedUserRole, setSelectedUserRole] = useState<string>(''); //filtra por rol de usuario

    const resourcesPerPage = 10;

    return (
        <Box sx={{ width: '100vw', minHeight: '100vh', py: 4, px: 2, backgroundColor: '#f8f9fa' }}>
            
            {/* Buscador con filtros */}
            <Box sx={{ display: 'flex', justifyContent: 'center', mb: 4 }}>
                <SearchBar searchTerm={searchTerm} handleSearchChange={handleSearchChange} labelText='Recursos' />
                <FilterSelect
                    selectedFilters={selectedFilters}
                    handleFilterChange={handleFilterChange}
                    handleApplyFilters={handleApplyFilters}
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
                                {filter === "Tipo de Rol" && (
                                        <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                            <Typography>Rol:</Typography>
                                            <FormControl sx={{ ml: 2, minWidth: 250 }} variant="outlined">
                                                <InputLabel id="role-select-label">Escoger Rol</InputLabel>
                                                <Select
                                                    labelId="role-select-label"
                                                    value={selectedUserRole}
                                                    onChange={(e) => setSelectedUserRole(e.target.value)}
                                                    label="Escoger Rol"
                                                >
                                                    <MenuItem value="Admin">Administrador</MenuItem>
                                                    <MenuItem value="Educator">Educador</MenuItem>
                                                    <MenuItem value="Parent">Padre</MenuItem>
                                                </Select>
                                            </FormControl>
                                        </Box>
                                    )}
                                {filter === "Nombre" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>🔍 Buscar por nombre:</Typography>
                                        <FormControl sx={{ ml: 2, width: 200 }} variant="outlined">
                                            <Input
                                                type="text"
                                                value={selectedPersonName || ''}
                                                onChange={(e) => {
                                                    const value = e.target.value;
                                                    setSelectedPersonName(value || ''); // Actualiza el estado con el nombre
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
                                {filter === "Apellidos" && (
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Typography>🔍 Buscar por apellidos:</Typography>
                                        <FormControl sx={{ ml: 2, width: 200 }} variant="outlined">
                                            <Input
                                                type="text"
                                                value={selectedPersonLastName || ''}
                                                onChange={(e) => {
                                                    const value = e.target.value;
                                                    setSelectedPersonLastName(value || ''); // Actualiza el estado con el apellido
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
        

    )
}
export default UserManagementPage;
