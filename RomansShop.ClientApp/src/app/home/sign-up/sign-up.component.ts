import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as shajs from 'sha.js';

import { Product } from '../../shared/product';
import { ProductService } from '../../api/product.service';
import { ShoppingCartService } from '../../api/shopping-cart.service';
import { OrderComponent } from '../order/order.component';
import { AuthenticationService } from '../../api/authentication.service';
import { AlertService } from '../../api/alert.service';
import { UserService } from '../../api/user.service';
import { User } from '../../shared/user';
import { UserRights } from '../../shared/user-rights';

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

    signUpClick() {
        let user: User = new User(null, this.model.email, this.model.name, shajs('sha256').update(this.model.password).digest('hex'), UserRights.Customer);
        this.userService.create(user).subscribe((user: User) => {
            this.signIn();
            this.activeModal.close();
        }, error => {
            this.errorMessage = error.error;
        });
    }

    private signIn() {
        this.authenticationService.login(this.model.email, shajs('sha256').update(this.model.password).digest('hex')).subscribe(user => {
            if (!user.id) {
                this.errorMessage = "Email or password is incorrect!"
            }
        });
    }

    close() {
        this.activeModal.close();
        this.errorMessage = null;
    }
}