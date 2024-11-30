/**
 * Service Worker para el manejo de caché de imágenes dinámicas.
 *
 * Este script de Service Worker gestiona el caché de imágenes para mejorar el rendimiento
 * de la aplicación al almacenar y servir imágenes desde el caché en lugar de realizar
 * solicitudes de red repetidas. Utiliza un caché denominado 'dynamic-image-cache-v1'.
 *
 * - **install event**: Durante la instalación del Service Worker, se precargan y almacenan
 *   en caché un conjunto de imágenes especificadas en `urlsToCache`. Esto asegura que las
 *   imágenes estén disponibles sin conexión y se carguen rápidamente.
 *
 * - **fetch event**: Intercepta las solicitudes de red para imágenes. Si una imagen solicitada
 *   ya está en el caché, se devuelve directamente desde allí. Si no está en el caché, se realiza
 *   una solicitud de red para obtener la imagen, que luego se almacena en el caché para futuras
 *   solicitudes. Esto permite que las imágenes se sirvan rápidamente y reduce la carga en la red.
 */
const CACHE_NAME = 'dynamic-image-cache-v1';

self.addEventListener('install', (event) => {
  const urlsToCache = [
    '/assets/images/home-bg.jpg',
    '/assets/images/activities/art-workshop-1.jpg',
    '/assets/images/activities/kids-sport.jpg',
    '/assets/images/activities/kids-science.jpg',
    '/assets/images/decorative/hand-print.png',
    '/assets/images/decorative/kindergarten.png',
  ];

  event.waitUntil(
    caches.open(CACHE_NAME).then((cache) => {
      return cache.addAll(urlsToCache);
    })
  );
});

self.addEventListener('fetch', (event) => {
  if (event.request.destination === 'image') {
    event.respondWith(
      caches.match(event.request).then((response) => {
        if (response) {
          return response;
        }

        return fetch(event.request).then((fetchResponse) => {
          return caches.open(CACHE_NAME).then((cache) => {
            cache.put(event.request, fetchResponse.clone());
            return fetchResponse;
          });
        });
      })
    );
  }
});