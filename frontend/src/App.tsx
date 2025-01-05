import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import CssBaseline from '@mui/material/CssBaseline';
import { ThemeProvider, createTheme } from '@mui/material/styles';
import Navbar from './components/layouts/Navbar';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import VerifyEmailPage from './pages/VerifyEmailPage';
import { AuthProvider } from './components/auth/authContext';
import ProtectedRoute from './components/auth/ProtectedRoute';
import ActivitiesPage from './pages/ActivitiesPage';
import ActivityInfoPage from './pages/ActivityInfoPage';
import ReviewsPage from './pages/ReviewsPage';
import EducatorPage from './pages/EducatorPage';
import ActivityManagerPage from './pages/ActivityManagerPage';
// import AdminPage from './pages/AdminPage';

// const [reload, setReload] = useState(false);
/**
 * Configuración del tema de Material-UI para la aplicación.
 * 
 * Este objeto define el tema personalizado de Material-UI, incluyendo la paleta de colores,
 * la tipografía y las modificaciones de estilo para componentes específicos como MuiCard y MuiCardMedia.
 * 
 * - **palette**: Define los colores primarios y secundarios, con variantes claras y oscuras.
 * - **typography**: Configura el peso de la fuente para los encabezados h5 y el tamaño de fuente para body2.
 * - **components**: Sobrescribe estilos para componentes de Material-UI, ajustando el radio de borde de las tarjetas.
 */
const theme = createTheme({
  palette: {
    primary: {
      main: '#2196f3',
      light: '#64b5f6',
      dark: '#1976d2',
    },
    secondary: {
      main: '#ff9800',
      light: '#ffb74d',
      dark: '#f57c00',
    },
  },
  typography: {
    h5: {
      fontWeight: 600,
    },
    body2: {
      fontSize: '0.95rem',
    },
  },
  components: {
    MuiCard: {
      styleOverrides: {
        root: {
          borderRadius: 8,
        },
      },
    },
    MuiCardMedia: {
      styleOverrides: {
        root: {
          borderTopLeftRadius: 8,
          borderTopRightRadius: 8,
        },
      },
    },
  },
});

/**
 * Componente principal de la aplicación.
 * 
 * Este componente configura el tema de Material-UI, proporciona el contexto de autenticación
 * y define las rutas de la aplicación utilizando React Router. Incluye la barra de navegación
 * y las páginas principales de la aplicación.
 * 
 * @returns {JSX.Element} El componente principal de la aplicación.
 */
const App: React.FC = () => {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <AuthProvider>
        <Router>
          <Navbar />
          {/* En esta sección se agregan las rutas de las páginas a usar */}
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/educator" element={<EducatorPage />} />
            <Route path='/activity-manager' element={<ActivityManagerPage />} />
            <Route path="/activities" element={<ActivitiesPage />} />
            <Route path="/reviews" element={<ReviewsPage />} />
            <Route path="/activities/:id/:imagePath/:useCase" element={<ActivityInfoPage />} />
            <Route
              path="/verify-email"
              element={
                <ProtectedRoute redirectTo="/login">
                  <VerifyEmailPage />
                </ProtectedRoute>
              }
            />
            {/* <Route path="/admin" element={<AdminPage />} /> */}
          </Routes>
        </Router>
      </AuthProvider>
    </ThemeProvider>
  );
};

export default App;