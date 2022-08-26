// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  BackendUrl: "https://localhost:7223/",
  ApiSuffix: "api/Rooms",
  HubSuffix: "roomsHub",
  ApiScopes: "openid profile email read:rooms write:rooms",
  Auth0Domain:"dev-e7eyj4xg.eu.auth0.com",
  ClientId: "eFHpoMFFdC7GXIfi9xe6VrZ5Z07xKl11"
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
