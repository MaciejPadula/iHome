import { Auth0Client, createAuth0Client, type Auth0ClientOptions } from "@auth0/auth0-spa-js";
import AuthConfig from "./auth-config";
import { isAuthenticated, user } from "./store";

async function createClient() {
  const options: Auth0ClientOptions = {
    domain: AuthConfig.domain,
    clientId: AuthConfig.clientId,
    authorizationParams: {
      audience: AuthConfig.audience,
      scope: AuthConfig.scope,
      redirect_uri: window.location.origin
    }
  };

  return await createAuth0Client(options);
}

async function loginWithRedirect() {
  const client = await createClient();
  await client.loginWithRedirect();
}

async function loadUser() {
  const client = await createClient();

  await client.getTokenSilently();

  const loggedUser = await client.getUser();
  if(!loggedUser) {
    isAuthenticated.set(false);
    return;
  }

  isAuthenticated.set(true);
  user.set(loggedUser);
}

function logout(client: Auth0Client) {
  return client.logout();
}

const auth = {
  createClient,
  loginWithRedirect,
  loadUser,
  logout
};

export default auth;