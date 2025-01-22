import React, { useEffect } from 'react';
import { useGoogleReCaptcha } from 'react-google-recaptcha-v3';

const ReCAPTCHAV3: React.FC = () => {
    const { executeRecaptcha } = useGoogleReCaptcha();

    useEffect(() => {
        const handleReCaptchaVerify = async () => {
            if (!executeRecaptcha) {
                console.log('El ReCaptcha no está disponible ahora.');
                return;
            }

            const action = 'register';
            const token = await executeRecaptcha(action);
            localStorage.setItem("captchaToken", token);
            console.log('CAPTCHA completado con éxito');
        };

        handleReCaptchaVerify();
    }, [executeRecaptcha]);

    return null; // No necesitas renderizar nada para reCAPTCHA v3
};

export default ReCAPTCHAV3;