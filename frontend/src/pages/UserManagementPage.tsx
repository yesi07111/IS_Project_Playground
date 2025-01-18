import React, { useState } from 'react';
import { Box, Typography, Button, Dialog, DialogActions, DialogContent, DialogTitle, IconButton } from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';


const UserManagementPage: React.FC<{ reload: boolean }> = ({ reload }) => {
    const [searchTerm, setSearchTerm] = useState(''); //termino de busqueda ingresado por el usuario
    const [currentPage, setCurrentPage] = useState(1); //pagina actual en paginacion
    const [selectedFilters, setSelectedFilters] = useState<string[]>([]); //filtros seleccionados por el usuario
    const [filtersOpen, setFiltersOpen] = useState(false); //seccion de filtros abierta o no
    const [selectedPersonName, setSelectedPersonName] = useState<string>(''); //filtra por nombre de la persona
    const [selectedPersonLastName, setSelectedPersonLastName] = useState<string>(''); //filtra por apellido de la persona
    const [selectedUserName, setSelectedUserName] = useState<string>(''); //filtra por nombre de usuario
    const [selectedUserRole, setSelectedUserRole] = useState<string[]>([]); //filtra por rol de usuario

    const resourcesPerPage = 10;

    return()
}
export default UserManagementPage;
