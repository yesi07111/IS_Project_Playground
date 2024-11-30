/**
 * Configuración de Vite para un proyecto React.
 * 
 * Este archivo configura Vite, un bundler rápido y moderno, para un proyecto que utiliza React.
 * Utiliza el plugin oficial de React para Vite, que proporciona soporte para características
 * específicas de React, como el refresco rápido de componentes.
 * 
 * - **plugins**: Incluye el plugin de React para Vite.
 */
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
});