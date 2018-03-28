import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { User } from '../shared/models/user';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser: User = JSON.parse(localStorage.getItem('currentUser'));

        if (currentUser != null) {
                return true;
        }
        
        this.router.navigate(['/']);
        return false;
    }
}