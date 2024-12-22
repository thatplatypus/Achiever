// src/hooks.server.js
export async function handle({ event, resolve }) {
    const session = event.cookies.get('.AspNetCore.Identity.Application');
    if (!session && event.url.pathname.startsWith('/register')) {
      return Response.redirect('/login', 302);
    }
    return resolve(event);
  }