import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as shajs from 'sha.js';

import { Product } from '../../models/product';
import { ProductService } from '../../../api/product.service';
import { ShoppingCartService } from '../../../api/shopping-cart.service';
import { OrderComponent } from '../make-order/make-order.component';
import { AuthenticationService } from '../../../api/authentication.service';
import { AlertService } from '../../../api/alert.service';
import { UserService } from '../../../api/user.service';
import { User } from '../../models/user';
import { UserRights } from '../../enums/user-rights';

@Component({
    templateUrl: './sign-up.component.html'
})
export class SignUpComponent implements OnInit {
    model: any = {};
    errorMessage: string;

    constructor(private activeModal: NgbActiveModal,
                private modalService: NgbModal,
                private authenticationService: AuthenticationService,
                private userService: UserService,
                private alertService: AlertService) {
    }

    ngOnInit() {
    }

    private signUpClick() {
        const user: User = new User(this.model);

        user.password = shajs('sha256').update(user.password).digest('hex');
        user.rights = UserRights.CUSTOMER;

        this.userService.create(user)
            .subscribe(
                (user: User) => {
                    this.signIn();
                    this.activeModal.close();
                }, 
                (error: any) => this.errorMessage = error.error
            );
    }

    private signIn() {
        this.authenticationService.login(this.model.email, shajs('sha256').update(this.model.password).digest('hex'))
            .subscribe(
                (user: User) => {
                    if (!user.id) {
                        this.errorMessage = "Email or password is incorrect!"
                    }
                }
            );
    }

    private close() {
        this.activeModal.close();
        this.errorMessage = null;
    }
}