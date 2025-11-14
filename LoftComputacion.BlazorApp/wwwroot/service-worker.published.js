// In production, the service worker will cache resources to enable offline support.

self.addEventListener('install', event => {
    console.log('Service Worker (published) instalado');
    event.waitUntil(
        caches.open('loft-cache-v2').then(cache => {
            return cache.addAll([
                '/',
                '/index.html',
                '/manifest.json',
                '/css/app.css',
                '/img/logo.png'
                // ⚠️ Podés agregar más recursos estáticos si querés
            ]);
        })
    );
});

self.addEventListener('activate', event => {
    console.log('Service Worker (published) activado');
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