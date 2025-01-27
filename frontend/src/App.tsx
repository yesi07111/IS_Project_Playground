import React, { useEffect, useState } from 'react';
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
import ResetPasswordPage from './pages/ResetPasswordPage';
import { GoogleOAuthProvider } from '@react-oauth/google';
import UserProfilePage from './pages/UserProfilePage';
import EditUserProfilePage from './pages/EditUserProfilePage';
import { authService } from './services/authService';
import { GoogleReCaptchaProvider } from 'react-google-recaptcha-v3';
import ReservationsPage from './pages/MyReservationPage';
import DefineUsageFrequencyPage from './pages/UsageFrequencyPage';
import ActivityFormPage from './pages/ActivityFormPage';
import ResourcesPage from './pages/ResourcesPage';
import MyReviewPage from './pages/MyReviewPage';
import UsersPage from './pages/UsersPage';
import FacilityPage from './pages/FacilitiesPage';
import FacilitiesPage from './pages/FacilitiesPage';
// import EducatorPage from './pages/EducatorPage';
// import ActivityManagerPage from './pages/ActivityManagerPage';


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

const App: React.FC = () => {
  const [clientId, setClientId] = useState<string>('');
  const [siteKey, setSiteKey] = useState<string>('');
  const [startOnline, setOnlineState] = useState<boolean>(false);
  const [useCaptcha, setUseCaptcha] = useState<boolean>(false);

  useEffect(() => {
    const fetchClientId = async () => {
      try {
        const response = await authService.getGoogleClientId();
        setClientId(response.clientId);
      } catch (error) {
        console.error('Error al obtener el clientId de Google:', error);
      }
    };

    const fetchSiteKey = async () => {
      try {
        const response = await authService.getReCaptchaSiteKey();
        setSiteKey(response.siteKey);
        console.log("ReCaptcha sitekey: ", siteKey)
      } catch (error) {
        console.error('Error al obtener el siteKey de Captcha:', error);
      }
    };

    fetchClientId();
    fetchSiteKey();

    if (clientId.length !== 0) {
      console.log("Iniciando online...")
      setOnlineState(true);
    }

    if (siteKey.length !== 0) {
      console.log("Usando verificación Captcha.")
      setUseCaptcha(true);
    }

    // setOnlineState(false); //Descomentar esta línea fuerza al inicio en modo offline
    // setUseCaptcha(false); //Esta solo fuerza a que no se use captcha

  }, []);

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <GoogleOAuthProvider clientId={clientId}>
        <GoogleReCaptchaProvider reCaptchaKey={siteKey}>
          <AuthProvider>
            <Router>
              <Navbar />
              <Routes>
                <Route path="/" element={<HomePage reload={false} />} />
                <Route path="/register" element={<RegisterPage online={startOnline} useCaptcha={useCaptcha} />} />
                <Route path="/login" element={<LoginPage online={startOnline} useCaptcha={useCaptcha} />} />
                <Route path="/activities" element={<ActivitiesPage reload={false} />} />
                <Route path="/reviews" element={<ReviewsPage reload={false} />} />
                <Route path="/profile/:id" element={<UserProfilePage />} />
                <Route path="/edit-profile/:id" element={<EditUserProfilePage />} />
                <Route path="/activities/:id/:imagePath/:useCase" element={<ActivityInfoPage reload={false} />} />
                <Route path="/my-reservations/:id" element={<ReservationsPage />}></Route>
                <Route path="/my-reviews/:id" element={<MyReviewPage />} />
                <Route path="/resources" element={<ResourcesPage reload={false} />} />
                <Route path="/define-usage-frequency/:resourceId" element={<DefineUsageFrequencyPage />} />
                <Route path="/activity-form" element={<ActivityFormPage />} />
                <Route path="/users" element={<UsersPage reload={false} />} />
                <Route path="/facilities" element={<FacilitiesPage reload={false} />} />
                <Route
                  path="/verify-email"
                  element={
                    <ProtectedRoute redirectTo="/login">
                      <VerifyEmailPage />
                    </ProtectedRoute>
                  }
                />
                <Route
                  path="/reset-password"
                  element={
                    <ProtectedRoute redirectTo="/login">
                      <ResetPasswordPage />
                    </ProtectedRoute>
                  }
                />
              </Routes>
            </Router>
          </AuthProvider>
        </GoogleReCaptchaProvider>
      </GoogleOAuthProvider>
    </ThemeProvider>
  );
};

export default App;