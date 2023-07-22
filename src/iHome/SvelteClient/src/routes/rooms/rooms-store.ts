import { writable, type Writable } from "svelte/store";
import type { RoomModel } from "../../models/room";
import type { WidgetModel } from "../../models/widget";

export const rooms: Writable<RoomModel[]> = writable([]);
export const widgets: Writable<WidgetModel[]> = writable([]);