import type { UserModel } from "./user";

export interface RoomModel {
    id: string;
    name: string;
    user: UserModel;
}