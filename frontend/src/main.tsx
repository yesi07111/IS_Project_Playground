import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import App from './App';

/**
 * Punto de entrada principal de la aplicación.
 * 
 * Este archivo configura el renderizado de la aplicación React en el DOM,
 * utilizando el modo estricto de React para ayudar a identificar problemas
 * potenciales en la aplicación. También registra un Service Worker para
 * mejorar el rendimiento y la capacidad de trabajo sin conexión.
 */

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
);

/**
 * Registro de Service Worker para el manejo de caché.
 * 
 * Este bloque de código verifica si el navegador soporta Service Workers y, de ser así,
 * registra un Service Worker desde '../public/cacheService.js' cuando la página se carga.
 * El Service Worker se utiliza para gestionar el caché de recursos, como imágenes, mejorando
 * el rendimiento y la capacidad de trabajo sin conexión de la aplicación.
 * 
 * - Si el registro es exitoso, se imprime en la consola el alcance del Service Worker.
 * - Si el registro falla, se imprime un mensaje de error en la consola.
 */
if ('serviceWorker' in navigator) {
  window.addEventListener('load', () => {
    navigator.serviceWorker
      .register('/serviceWorker.js')
      .then((registration) => {
        console.log('Service Worker registered with scope:', registration.scope);
      })
      .catch((error) => {
        console.error('Service Worker registration failed:', error);
      });
  });
}