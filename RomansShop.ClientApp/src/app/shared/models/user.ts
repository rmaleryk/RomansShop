import { UserRights } from "../enums/user-rights";

export class User {
    id?: string;
    email?: string;
    fullName?: string;
    password?: string;
    rights?: UserRights;

    constructor(data: any) {
        Object.assign(this, data);
    }
}