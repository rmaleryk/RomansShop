import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { Product } from '../../shared/models/product';
import { ProductService } from '../../api/product.service';
import { ShoppingCartService } from '../../api/shopping-cart.service';
import { Order } from '../../shared/models/order';
import { AlertService } from '../../api/alert.service';
import { User } from '../../shared/models/user';
import { AuthenticationService } from '../../api/authentication.service';
import { OrderService } from '../../api/order.service';
import { UserService } from '../../api/user.service';

@Component({
    templateUrl: './user-settings.component.html'
})
export class UserSettingsComponent implements OnInit, OnDestroy {
    user: User;
    currentUser: User;
    destroy$: Subject<boolean> = new Subject<boolean>();

    constructor(private alertService: AlertService,
                private authenticationService: AuthenticationService,
                private userService: UserService) {
    }

    ngOnInit() {
        this.authenticationService.getCurrentUser()
            .takeUntil(this.destroy$)
            .subscribe(
                (user: User) => { 
                    this.currentUser = user;
                    this.user = this.currentUser;
                }
            );
    }

    private save() {
        this.userService.update(this.user)
            .subscribe(
                (user: User) => {
                    this.alertService.info("Your profile has been successfully updated.", 2000)
                    localStorage.setItem('currentUser', JSON.stringify(user));

                },
                (error: any) => {
                    this.alertService.info(error.error);
                    this.user = this.currentUser;
                }
            );
    }

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }
}