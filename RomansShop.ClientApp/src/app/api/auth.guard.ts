import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { User } from '../shared/user';
import { UserRights } from '../shared/user-rights';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        let currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
        if (currentUser!= null && (currentUser.rights == UserRights.Moderator || currentUser.rights == UserRights.Administrator)) {
            return true;
        }
        
        this.router.navigate(['/']);
        return false;
    }
}