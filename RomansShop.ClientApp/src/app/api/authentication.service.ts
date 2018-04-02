import { Injectable, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import 'rxjs/add/observable/of';

import { User } from "../shared/models/user";
import { UserRights } from "../shared/enums/user-rights";
import { AlertService } from "./alert.service";
import { AppSettings } from "../shared/constants/app-settings";

@Injectable()
export class AuthenticationService {
    private url = AppSettings.API_ENDPOINT + "/authenticate";
    private currentUser$: BehaviorSubject<User> = new BehaviorSubject<User>({});

    constructor(private http: HttpClient,
                private alertService: AlertService) {
        this.loadCurrentUser();        
    }

    getCurrentUser(): Observable<User> {
        return this.currentUser$.asObservable();
    }

    login(email: string, password: string): Observable<User> {
        return this.http.post<User>(this.url, { email: email, password: password })
            .map((user: User) => {
                if (user != null) {
                    localStorage.setItem('currentUser', JSON.stringify(user));
                }
                return user;
            })
            .do((user: User) => {
                if (user.id != null) {
                    this.alertService.info(`Hello! You are logged in as ${user.fullName}.`, 2000);
                    this.loadCurrentUser();
                }
            });
    }

    logout() {
        localStorage.removeItem('currentUser');
        this.alertService.info("You have successfully logged out of your account.", 2000);
        this.loadCurrentUser();
    }

    private loadCurrentUser() {
        const currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
        this.currentUser$.next(currentUser);
    }
}