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
import { UserService } from '../../api/user.service';
import { User } from '../../shared/models/user';
import { UserRights } from '../../shared/enums/user-rights';

@Component({
    templateUrl: './sign-up.component.html'
})
export class SignUpComponent implements OnInit, OnDestroy {
    model: any = {};
    errorMessage: string;
    destroy$: Subject<boolean> = new Subject<boolean>();

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
            .takeUntil(this.destroy$)
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
            .takeUntil(this.destroy$)    
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

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }
}