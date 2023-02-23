import { User } from "@auth0/auth0-angular";

export interface Room {
    id: string;
    name: string;
    user: User;
}
