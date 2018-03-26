import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { User } from '../shared/models/user';
import { UserRights } from '../shared/enums/user-rights';

@Injectable()
export class UserService {
    private url = "http://localhost:50725/api/users";
    private users$: BehaviorSubject<User[]> = new BehaviorSubject<User[]>([]);

    constructor(private http: HttpClient) { 
        this.loadUsers();
    }

    getUsers(): Observable<User[]> {
        return this.users$.asObservable();
    }

    getById(id: number): Observable<User> {
        return this.http.get(`${this.url}/${id}`)
            .map((userResponse: any) => new User(userResponse.id, userResponse.email, userResponse.fullName, null, userResponse.rights));
    }

    getByRights(rights: UserRights): Observable<User[]> {
        return this.http.get(`${this.url}/groups/${rights}`)
            .map((data: any) => data.map(function (resp: any) {
                return new User(resp.id, resp.email, resp.fullName, null, resp.rights);
            }));
    }

    create(user: User): Observable<User> {
        return this.http.post(this.url, user)
            .map((userResponse: any) => new User(userResponse.id, userResponse.email, userResponse.fullName, null, userResponse.rights))
            .do(() => this.loadUsers());
    }

    update(user: User): Observable<User> {
        return this.http.put(`${this.url}/${user.id}`, user)
            .map((userResponse: any) => new User(userResponse.id, userResponse.email, userResponse.fullName, null, userResponse.rights))
            .do(() => this.loadUsers());
    }

    delete(id: string): Observable<string> {
        return this.http.delete(`${this.url}/${id}`, { responseType: "text" })
        .do(() => this.loadUsers());
    }

    private loadUsers() {
        this.http.get<User[]>(this.url)
            .map((data: any) => data.map(function (userResponse: any) {
                return new User(userResponse.id, userResponse.email, userResponse.fullName, null, userResponse.rights);
            }))
            .subscribe((data: User[]) => this.users$.next(data));
    }
}