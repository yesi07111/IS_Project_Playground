import React, { useState, useEffect } from 'react';
import { Box, Typography, Button, Avatar, IconButton } from '@mui/material';
import { styled } from '@mui/material/styles';
import { Theme } from '@mui/material/styles';
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

const UserProfilePage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { setCanAccessPasswordReset } = useAuth();
  const [userImage, setUserImage] = useState<string>('');
  const [userData, setUserData] = useState<UserResponse | null>(null);
  const [isCurrentUser, setIsCurrentUser] = useState<boolean>(false);

  useEffect(() => {
    const fetchUserData = async () => {
      if (id) {
        try {
          const data = await userService.getUser(id, 'UserProfileView');
          setUserData(data.result);
          const authId = localStorage.getItem('authId');
          setIsCurrentUser(authId === data.result.id);
          loadUserImage(data.result.username);
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
      setUserImage('/' + main);
      console.log('Imagen principal cargada desde la caché:', main);
    } else if (others.length === 1) {
      setUserImage(others[0]);
      console.log('Imagen alternativa cargada desde la caché:', others[0]);
    } else if (others.length > 1) {
      // Mostrar opciones al usuario para seleccionar una imagen
      const selectedImage = window.prompt(
        'No se encontró la imagen principal. Seleccione una de las siguientes imágenes:\n' +
        others.map((img: string, index: number) => `${index + 1}: ${img}`).join('\n')
      );
      const selectedIndex = parseInt(selectedImage || '', 10) - 1;
      if (selectedIndex >= 0 && selectedIndex < others.length) {
        setUserImage(others[selectedIndex]);
        console.log('Imagen seleccionada por el usuario:', others[selectedIndex]);
      } else {
        console.error('Selección inválida.');
      }
    } else {
      try {
        const response = await imageUploadService.saveUserImage(username, null!);
        const imageUrl = response.imageUrl.replace(/^public[\\/]/, '').replace(/\\/g, '/');
        console.log("Imageurl: " + imageUrl)
        cacheService.saveUserImages(username, imageUrl, response.others);
        setUserImage(`/${imageUrl}`);
        console.log('Imagen cargada desde el servidor:', imageUrl);
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

  useEffect(() => {
    console.log('Imagen de usuario actualizada:', userImage);
  }, [userImage]);

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
        console.log("Guardando la imagen...");
        const response = await imageUploadService.saveUserImage(userData.username, file);
        const imageUrl = response.imageUrl.replace(/^public[\\/]/, '').replace(/\\/g, '/');
        console.log('Imagen guardada exitosamente: ', imageUrl);

        // Guardar la imagen en la caché
        const cachedImages = cacheService.loadImages();
        cachedImages[userData.username] = `/${imageUrl}`;
        cacheService.saveImages(cachedImages);

        setUserImage(`/${imageUrl}`); // Actualiza la imagen después de la carga
      } catch (error) {
        console.error('Error guardando la imagen:', error);
      }
    }
  };

  return (
    <Box minWidth="100vw" sx={{ mt: 4, mb: 4, position: 'relative', overflow: 'hidden' }}>
      <Box
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
            <Typography variant="h3" gutterBottom sx={{ color: 'primary.main', fontWeight: 'bold' }}>
              🎉 Bienvenido de vuelta al Mundo de Diversión 🎈
            </Typography>

            <StyledBox>
              <Typography variant="h6">👤 Nombre: {userData.firstName}</Typography>
            </StyledBox>
            <StyledBox>
              <Typography variant="h6">👥 Apellido: {userData.lastName}</Typography>
            </StyledBox>
            <StyledBox>
              <Typography variant="h6">📛 Nombre de Usuario: @{userData.username}</Typography>
            </StyledBox>
            <StyledBox>
              <Typography variant="h6">📧 Email: {userData.email}</Typography>
            </StyledBox>
          </>
        )}

        {isCurrentUser && (
          <>
            <StyledButton variant="contained" color="primary" sx={{ mt: 2 }} onClick={handleResetPassword}>
              Resetear Contraseña
            </StyledButton>
            <StyledButton variant="contained" color="primary" sx={{ mt: 2 }} onClick={handleEditProfile}>
              Editar Perfil
            </StyledButton>
          </>
        )}
      </Box>
    </Box>
  );
};

const StyledBox = styled(Box)(({ theme }: { theme: Theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'flex-start',
  width: '100%',
  maxWidth: 600,
  padding: theme.spacing(2),
  marginBottom: theme.spacing(2),
  borderRadius: theme.shape.borderRadius,
  boxShadow: theme.shadows[1] || 'none',
  transition: 'transform 0.3s ease-in-out, box-shadow 0.3s ease-in-out',
  '&:hover': {
    boxShadow: theme.shadows[6] || 'none',
    transform: 'scale(1.05)',
  },
}));

const StyledButton = styled(Button)(({ theme }: { theme: Theme }) => ({
  borderRadius: 20,
  padding: theme.spacing(1, 3),
  fontWeight: 'bold',
  transition: 'background-color 0.3s ease-in-out, transform 0.3s ease-in-out',
  '&:hover': {
    transform: 'scale(1.1)',
  },
}));

export default UserProfilePage;