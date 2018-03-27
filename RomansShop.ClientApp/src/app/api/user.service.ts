import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { User } from '../shared/models/user';
import { UserRights } from '../shared/enums/user-rights';
import { AppSettings } from '../shared/constants/app-settings';

@Injectable()
export class UserService {
    private url = AppSettings.API_ENDPOINT + "/users";
    private users$: BehaviorSubject<User[]> = new BehaviorSubject<User[]>([]);

    constructor(private http: HttpClient) { 
        this.loadUsers();
    }

    getUsers(): Observable<User[]> {
        return this.users$.asObservable();
    }

    getById(id: number): Observable<User> {
        return this.http.get(`${this.url}/${id}`)
            .map((data: any) => new User(data));
    }

    getByRights(rights: UserRights): Observable<User[]> {
        return this.http.get(`${this.url}/groups/${rights}`)
            .map((data: any[]) => this.createUsers(data))
    }

    create(user: User): Observable<User> {
        return this.http.post(this.url, user)
            .map((data: any) => new User(data))
            .do(() => this.loadUsers());
    }

    update(user: User): Observable<User> {
        return this.http.put(`${this.url}/${user.id}`, user)
            .map((data: any) => new User(data))
            .do(() => this.loadUsers());
    }

    delete(id: string): Observable<string> {
        return this.http.delete(`${this.url}/${id}`, { responseType: "text" })
            .do(() => this.loadUsers());
    }

    private loadUsers() {
        this.http.get<User[]>(this.url)
            .map((data: any[]) => this.createUsers(data))
            .subscribe((data: User[]) => this.users$.next(data));
    }

    private createUsers(usersData: any): User[] {
        return usersData.map(user => new User(user));
    }
}