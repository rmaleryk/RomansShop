import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as shajs from 'sha.js';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { Product } from '../../shared/models/product';
import { ProductService } from '../../api/product.service';
import { ShoppingCartService } from '../../api/shopping-cart.service';
import { OrderComponent } from '../order/order.component';
import { AuthenticationService } from '../../api/authentication.service';
import { AlertService } from '../../api/alert.service';
import { SignUpComponent } from '../sign-up/sign-up.component';

@Component({
    templateUrl: './sign-in.component.html'
})
export class SignInComponent implements OnInit, OnDestroy {
    model: any = {};
    errorMessage: string;
    destroy$: Subject<boolean> = new Subject<boolean>();

    constructor(private activeModal: NgbActiveModal,
                private modalService: NgbModal,
                private authenticationService: AuthenticationService,
                private alertService: AlertService) {
    }

    ngOnInit() {
    }

    private signInClick() {
        this.authenticationService.login(this.model.email, shajs('sha256').update(this.model.password).digest('hex'))
            .takeUntil(this.destroy$)    
            .subscribe(
                (user: any) => this.activeModal.close(),
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

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }
}