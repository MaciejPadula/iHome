import { writable, type Writable } from "svelte/store";
import type { User } from "@auth0/auth0-spa-js";

export const loading = writable(true);

export const isAuthenticated = writable(false);
export const user: Writable<User | undefined> = writable({});