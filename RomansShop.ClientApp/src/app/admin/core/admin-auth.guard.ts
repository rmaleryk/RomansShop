import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { User } from '../../shared/models/user';
import { UserRights } from '../../shared/enums/user-rights';

@Injectable()
export class AdminAuthGuard implements CanActivate {
    constructor(private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
        const userRights = Object.values(UserRights);

        if (currentUser!= null && (userRights[currentUser.rights] == UserRights.MODERATOR || 
            userRights[currentUser.rights] == UserRights.ADMINISTRATOR)) {
                return true;
        }
        
        this.router.navigate(['/']);
        return false;
    }
}