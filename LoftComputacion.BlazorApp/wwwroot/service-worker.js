// Service Worker para entorno de desarrollo

self.addEventListener('install', event => {
    console.log('Service Worker instalado');
    event.waitUntil(
        caches.open('loft-cache-dev').then(cache => {
            return cache.addAll([
                '/',
                '/index.html',
                '/css/app.css',
                '/img/logo.png'
            ]);
        })
    );
});

self.addEventListener('activate', event => {
    console.log('Service Worker activado');
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(name => caches.delete(name))
            );
        })
    );
});

self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request).then(response => {
            return response || fetch(event.request);
        })
    );
});