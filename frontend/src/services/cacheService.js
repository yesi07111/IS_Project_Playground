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