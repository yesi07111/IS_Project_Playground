import Swal from "sweetalert2";
import { authService } from "./authService";

export const passwordService = {
    evaluatePasswordStrength: (password: string) => {
        let strength = 0;
        if (/[A-Z]/.test(password)) strength++;
        if (/[a-z]/.test(password)) strength++;
        if (/[0-9]/.test(password)) strength++;
        if (/[^a-zA-Z0-9]/.test(password)) strength++;
        if (password.length >= 8) strength++;

        if (strength <= 2) return { label: 'Baja', color: 'red' };
        if (strength <= 4) return { label: 'Media', color: '#DAA520' };
        return { label: 'Fuerte', color: 'green' };
    },

    checkPasswordMatch: (password: string, confirmPassword: string) => {
        return password === confirmPassword ? 'Coinciden.' : 'NO coinciden.';
    },

    ResetPassword: async (): Promise<boolean> => {
        const { value: formValues } = await Swal.fire({
            title: 'Introduce tus datos',
            html:
                '<input id="swal-input1" class="swal2-input" placeholder="Correo electrónico">' +
                '<div style="position: relative;">' +
                '<input id="swal-input2" type="password" class="swal2-input" placeholder="Nueva contraseña">' +
                '<button type="button" id="toggle-password-visibility" style="position: absolute; right: 40px; top: 50%; transform: translateY(-50%); background: transparent; border: none; cursor: pointer; width: 30px; height: 30px; border-radius: 50%; outline: none;">🙈</button>' +
                '</div>' +
                '<div style="position: relative;">' +
                '<input id="swal-input3" type="password" class="swal2-input" placeholder="Confirma tu contraseña">' +
                '<button type="button" id="toggle-confirm-password-visibility" style="position: absolute; right: 40px; top: 50%; transform: translateY(-50%); background: transparent; border: none; cursor: pointer; width: 30px; height: 30px; border-radius: 50%; outline: none;">🙈</button>' +
                '</div>',
            focusConfirm: false,
            showCancelButton: true,
            confirmButtonText: 'Aceptar',
            cancelButtonText: 'Cancelar',
            preConfirm: () => {
                const email = (document.getElementById('swal-input1') as HTMLInputElement).value;
                const password = (document.getElementById('swal-input2') as HTMLInputElement).value;
                const confirmPassword = (document.getElementById('swal-input3') as HTMLInputElement).value;

                if (!email || !email.includes('@') || !email.includes('.')) {
                    Swal.showValidationMessage('Por favor, introduce un correo electrónico válido');
                    return false;
                }

                if (password !== confirmPassword) {
                    Swal.showValidationMessage('Las contraseñas no coinciden');
                    return false;
                }

                if (!passwordService.isStrongPassword(password)) {
                    Swal.showValidationMessage('La contraseña no es fuerte, debe tener al menos 6 caracteres, incluir un número, una letra mayúscula y una letra minúscula');
                    return false;
                }

                return { email, password };
            },
            didOpen: () => {
                const togglePasswordVisibility = document.getElementById('toggle-password-visibility');
                const toggleConfirmPasswordVisibility = document.getElementById('toggle-confirm-password-visibility');
                const passwordInput = document.getElementById('swal-input2') as HTMLInputElement;
                const confirmPasswordInput = document.getElementById('swal-input3') as HTMLInputElement;

                const toggleVisibility = () => {
                    const isPasswordVisible = passwordInput.type === 'password';
                    const newType = isPasswordVisible ? 'text' : 'password';
                    const newButtonText = isPasswordVisible ? '👁️' : '🙈';

                    passwordInput.type = newType;
                    confirmPasswordInput.type = newType;
                    togglePasswordVisibility!.textContent = newButtonText;
                    toggleConfirmPasswordVisibility!.textContent = newButtonText;
                };

                togglePasswordVisibility?.addEventListener('click', toggleVisibility);
                toggleConfirmPasswordVisibility?.addEventListener('click', toggleVisibility);
            }
        });

        if (formValues) {
            const { email, password } = formValues;
            localStorage.setItem('pendingVerificationEmail', email);
            localStorage.setItem('newPassword', password);

            try {
                if (email) {
                    const response = await authService.sendResetPasswordCode(email);
                    localStorage.setItem("fullCode", response.token);
                    console.log('Email de reseteo de contraseña enviado correctamente.');
                    return true;
                }
            } catch (error) {
                console.error('Error enviando email de reseteo de contraseña.', error);
                return false;
            }
        }

        return false;
    },

    isStrongPassword: (password: string) => {
        return (
            password.length >= 6 &&
            /[0-9]/.test(password) &&
            /[A-Z]/.test(password) &&
            /[a-z]/.test(password)
        );
    }
};