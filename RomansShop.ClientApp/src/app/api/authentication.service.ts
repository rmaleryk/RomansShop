import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/of';

import { User } from "../shared/models/user";
import { UserRights } from "../shared/enums/user-rights";
import { AlertService } from "./alert.service";
import { AppSettings } from "../shared/constants/app-settings";

@Injectable()
export class AuthenticationService {
    private url = AppSettings.API_ENDPOINT + "/authenticate";

    constructor(private http: HttpClient,
                private alertService: AlertService) {
    }

    login(email: string, password: string): Observable<User> {
        return this.http.post<any>(this.url, { email: email, password: password })
            .map((user: User) => {
                if (user) {
                    localStorage.setItem('currentUser', JSON.stringify(user));
                }
                return user;
            })
            .do((user: User) => {
                if (user.id) {
                    this.alertService.info(`Hello! You are logged in as ${user.fullName}.`, 2000);
                }
            });
    }

    logout() {
        localStorage.removeItem('currentUser');
        this.alertService.info("You have successfully logged out of your account.", 2000);
    }

    getCurrentUser(): User {
        let currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
        return currentUser;
    }
}