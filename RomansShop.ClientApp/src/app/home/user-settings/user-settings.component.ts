import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { Product } from '../../shared/product';
import { ProductService } from '../../api/product.service';
import { ShoppingCartService } from '../../api/shopping-cart.service';
import { Order } from '../../shared/order';
import { AlertService } from '../../api/alert.service';
import { User } from '../../shared/user';
import { AuthenticationService } from '../../api/authentication.service';
import { OrderService } from '../../api/order.service';
import { UserService } from '../../api/user.service';

@Component({
    templateUrl: './user-settings.component.html'
})
export class UserSettingsComponent implements OnInit {
    user: User;

    constructor(
        private alertService: AlertService,
        private authenticationService: AuthenticationService,
        private userService: UserService) {
        this.user = this.currentUser;
    }

    ngOnInit() {
    }

    get currentUser(): User {
        return this.authenticationService.getCurrentUser();
    }

    save() {
        this.userService.update(this.user).subscribe(data => {
            this.alertService.info("Your profile has been successfully updated.", 2000)
            localStorage.setItem('currentUser', JSON.stringify(this.user));

        }, error => {
            this.alertService.info(error.error);
            this.user = this.currentUser;
        });
    }

}