import { createAuth0Client, Auth0Client } from "@auth0/auth0-spa-js";
import { isAuthenticated, user } from "./store";
// import { AuthConfig } from "./auth-config";

async function createClient() {
  return await createAuth0Client({
    domain: 'dev-e7eyj4xg.eu.auth0.com',
    clientId: 'eFHpoMFFdC7GXIfi9xe6VrZ5Z07xKl11',
    authorizationParams: {
      audience: 'https://localhost:32678/api',
      scope: 'openid profile email read:rooms write:rooms',
      redirect_uri: window.location.origin
    }
  });
}

async function login() {
  const client = await createClient();
  await client.loginWithPopup();

  isAuthenticated.set(await client.isAuthenticated());
  user.set(await client.getUser());
}

async function logout() {
  const client = await createClient();
  await client.logout();
}

async function middleware(func: Function) {
  try {
    await func();
  }
  catch(error) {
    console.log(error);
  }
}

async function getToken() {
  const client = await createClient();
  return await client.getTokenSilently();
}

const auth = {
  createClient,
  login: () => middleware(login),
  logout: () => middleware(logout),
  getToken
};

export default auth;