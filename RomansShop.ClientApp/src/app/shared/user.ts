import { UserRights } from "./user-rights";

export class User {
    constructor(public id?: string,
                public email?: string,
                public fullName?: string,
                public password?: string,
                public rights?: UserRights) {
    }
}