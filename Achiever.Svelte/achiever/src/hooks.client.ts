// src/hooks.client.js
export async function handle({ event, resolve }) {
    const session = event.cookies.get('.AspNetCore.Identity.Application');
    if (!session && (!event.url.pathname.startsWith('/register') || !event.url.pathname.startsWith('/login'))) {
      return Response.redirect('/login', 302);
    }
    return resolve(event);
  }

  export const handleFetch: import("@sveltejs/kit").HandleFetch = async ({ event, request, fetch }) => {
    if (request.url.startsWith('https://achiever-api.azurewebsites.net')) {
      request.headers.set('cookie', event.request.headers.get('cookie'));
    }
  
    return fetch(request);
  };