import { writable, type Writable } from "svelte/store";
import type { RoomModel } from "../models/room";
import type { User } from "@auth0/auth0-spa-js";

export const rooms: Writable<RoomModel[]> = writable([]);

export const isAuthenticated = writable(false);
export const user: Writable<User> = writable({});