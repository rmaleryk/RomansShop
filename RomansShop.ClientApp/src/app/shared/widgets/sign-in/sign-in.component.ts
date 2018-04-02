import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as shajs from 'sha.js';

import { Product } from '../../models/product';
import { ProductService } from '../../../api/product.service';
import { ShoppingCartService } from '../../../api/shopping-cart.service';
import { OrderComponent } from '../make-order/make-order.component';
import { AuthenticationService } from '../../../api/authentication.service';
import { AlertService } from '../../../api/alert.service';
import { SignUpComponent } from '../sign-up/sign-up.component';
import { User } from '../../models/user';

@Component({
    templateUrl: './sign-in.component.html'
})
export class SignInComponent implements OnInit {
    model: any = {};
    errorMessage: string;

    constructor(private activeModal: NgbActiveModal,
                private modalService: NgbModal,
                private authenticationService: AuthenticationService,
                private alertService: AlertService) {
    }

    ngOnInit() {
    }

    private signInClick() {
        this.authenticationService.login(this.model.email, shajs('sha256').update(this.model.password).digest('hex'))
            .subscribe(
                (user: User) => this.activeModal.close(),
                (error: any) => this.errorMessage = error.error
            );
    }

    private signUpClick() {
        this.activeModal.close();
        this.modalService.open(SignUpComponent);
    }

    private close() {
        this.activeModal.close();
        this.errorMessage = null;
    }
}