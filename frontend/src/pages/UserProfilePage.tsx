import React, { useState, useEffect } from 'react';
import { usePDF } from 'react-to-pdf';
import { Box, Typography, Avatar, IconButton, Button } from '@mui/material';
import { useParams, useNavigate } from 'react-router-dom';
import CameraAltIcon from '@mui/icons-material/CameraAlt';
import decoration1 from '/images/decorative/toy-train.png';
import decoration2 from '/images/decorative/swing.png';
import { useAuth } from '../components/auth/authContext';
import { imageUploadService } from '../services/imageUploadService';
import { passwordService } from '../services/passwordService';
import { cacheService } from '../services/cacheService';
import { UserResponse } from '../interfaces/User';
import { userService } from '../services/userService';
import CustomButton from '../components/features/StyledButton';
import CustomBox from '../components/features/StyledBox';

const UserProfilePage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { setCanAccessPasswordReset } = useAuth();
  const [userImage, setUserImage] = useState<string>('');
  const [userData, setUserData] = useState<UserResponse | null>(null);
  const [isCurrentUser, setIsCurrentUser] = useState<boolean>(false);
  const [rol, setRol] = useState<string>('');

  const { toPDF, targetRef } = usePDF({ filename: 'MiPerfil.pdf' });

  useEffect(() => {
    const fetchUserData = async () => {
      if (id) {
        try {
          const data = await userService.getUser(id, 'UserProfileView');
          setUserData(data.result);
          const authId = localStorage.getItem('authId');
          setIsCurrentUser(authId === data.result.id);
          loadUserImage(data.result.username);
          setRol(data.result.rol)
        } catch (error) {
          console.error('Error al obtener los datos del usuario:', error);
        }
      } else {
        console.error('Error al obtener la id del usuario.');
      }
    };

    fetchUserData();
  }, [id]);

  const loadUserImage = async (username: string) => {
    const { main, others } = cacheService.loadUserImages(username);

    if (main) {
      setUserImage(main);
    } else if (others.length === 1) {
      setUserImage(others[0]);
    } else if (others.length > 1) {
      const selectedImage = window.prompt(
        'No se encontró la imagen principal. Seleccione una de las siguientes imágenes:\n' +
        others.map((img: string, index: number) => `${index + 1}: ${img}`).join('\n')
      );
      const selectedIndex = parseInt(selectedImage || '', 10) - 1;
      if (selectedIndex >= 0 && selectedIndex < others.length) {
        setUserImage(others[selectedIndex]);
      } else {
        console.error('Selección inválida.');
      }
    } else {
      try {
        const response = await imageUploadService.saveUserImage(username, null!);
        const imageUrl = response.imageUrl.replace(/^public[\\/]/, '').replace(/\\/g, '/');
        cacheService.saveUserImages(username, imageUrl, response.others);
        setUserImage(`/${imageUrl}`);
      } catch (error) {
        console.error('Error cargando la imagen:', error);
      }
    }
  };

  const handleEditProfile = () => {
    localStorage.setItem("firstName", userData?.firstName ?? '')
    localStorage.setItem("lastName", userData?.lastName ?? '')
    localStorage.setItem("userName", userData?.username ?? '')
    localStorage.setItem("email", userData?.email ?? '')

    navigate(`/edit-profile/${id}`);
  };

  const handleResetPassword = async () => {
    const response = await passwordService.ResetPassword();
    if (response) {
      setCanAccessPasswordReset(true);
      navigate('/reset-password');
    }
  };

  const handleImageUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file && userData) {
      try {
        const response = await imageUploadService.saveUserImage(userData.username, file);
        const imageUrl = response.imageUrl.replace(/^public[\\/]/, '').replace(/\\/g, '/');
        cacheService.saveUserImages(userData.username, `/${imageUrl}`, response.others);

        setUserImage(`/${imageUrl}`);
      } catch (error) {
        console.error('Error guardando la imagen:', error);
      }
    }
  };

  return (
    <Box minWidth="100vw" sx={{ mt: 4, mb: 4, position: 'relative', overflow: 'hidden' }}>
      <Box
        ref={targetRef}
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          minWidth: '100vw',
          minHeight: '100vh',
          bgcolor: 'background.paper',
          boxShadow: 3,
          borderRadius: 2,
          p: 3,
          position: 'relative',
          zIndex: 1,
        }}
      >
        <Box
          component="img"
          src={decoration1}
          sx={{
            position: 'absolute',
            right: -20,
            bottom: '36%',
            opacity: 0.1,
            width: '400px',
            pointerEvents: 'none',
            transform: 'translate(-50px, -50px)',
          }}
        />

        <Box
          component="img"
          src={decoration2}
          sx={{
            position: 'absolute',
            left: '5%',
            top: '50%',
            transform: 'translateY(-50%)',
            opacity: 0.1,
            width: '500px',
            pointerEvents: 'none',
            zIndex: 0,
          }}
        />

        <Box sx={{ position: 'relative', mb: 3 }}>
          <Avatar
            alt="User Photo"
            src={userImage}
            sx={{ width: 300, height: 300 }}
          />
          {isCurrentUser && (
            <input
              accept="image/*"
              style={{ display: 'none' }}
              id="icon-button-file"
              type="file"
              onChange={handleImageUpload}
            />
          )}
          {isCurrentUser && (
            <label htmlFor="icon-button-file">
              <IconButton
                component="span"
                sx={{
                  position: 'absolute',
                  bottom: 10,
                  right: 30,
                  bgcolor: 'primary.main',
                  color: 'white',
                  '&:hover': {
                    bgcolor: 'primary.dark',
                  },
                }}
              >
                <CameraAltIcon />
              </IconButton>
            </label>
          )}
        </Box>

        {userData && (
          <>
            {(rol == "Parent" || rol == "") && <Typography variant="h3" gutterBottom sx={{ color: 'primary.main', fontWeight: 'bold' }}>
              🎉 Bienvenido de vuelta al Mundo de Diversión 🎈
            </Typography>}

            {(rol == "Educator" || rol == "") && <Typography variant="h3" gutterBottom sx={{ color: 'primary.main', fontWeight: 'bold' }}>
              Cuenta de Educador
            </Typography>}

            {(rol == "Admin" || rol == "") && <Typography variant="h3" gutterBottom sx={{ color: 'primary.main', fontWeight: 'bold' }}>
              Cuenta de Administración
            </Typography>}

            <CustomBox>
              <Typography variant="h6">👤 Nombre: {userData.firstName}</Typography>
            </CustomBox>
            <CustomBox>
              <Typography variant="h6">👥 Apellido: {userData.lastName}</Typography>
            </CustomBox>
            <CustomBox>
              <Typography variant="h6">📛 Nombre de Usuario: @{userData.username}</Typography>
            </CustomBox>
            <CustomBox>
              <Typography variant="h6">📧 Email: {userData.email}</Typography>
            </CustomBox>
          </>
        )}

        {isCurrentUser && (
          <>
            <CustomButton variant="contained" color="primary" sx={{ mt: 2 }} onClick={handleResetPassword}>
              Resetear Contraseña
            </CustomButton>
            <CustomButton variant="contained" color="primary" sx={{ mt: 2 }} onClick={handleEditProfile}>
              Editar Perfil
            </CustomButton>

            <Box sx={{ height: 16 }} />
            <Button
              variant="contained"
              color="primary"
              onClick={() => toPDF()}
            >
              Exportar a PDF
            </Button>
          </>
        )}
      </Box>
    </Box>
  );
};

export default UserProfilePage;