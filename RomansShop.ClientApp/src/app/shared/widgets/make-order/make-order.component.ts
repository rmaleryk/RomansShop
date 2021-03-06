import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { Product } from '../../models/product';
import { ProductService } from '../../../api/product.service';
import { ShoppingCartService } from '../../../api/shopping-cart.service';
import { Order } from '../../models/order';
import { AlertService } from '../../../api/alert.service';
import { User } from '../../models/user';
import { AuthenticationService } from '../../../api/authentication.service';
import { OrderService } from '../../../api/order.service';

@Component({
    templateUrl: './make-order.component.html'
})
export class OrderComponent implements OnInit, OnDestroy {
    order: Order = new Order({});
    destroy$: Subject<boolean> = new Subject<boolean>();

    constructor(private activeModal: NgbActiveModal,
                private shoppingCartService: ShoppingCartService,
                private alertService: AlertService,
                private authenticationService: AuthenticationService,
                private orderService: OrderService) {
    }

    ngOnInit() {
        this.authenticationService.getCurrentUser()
            .takeUntil(this.destroy$)
            .subscribe(
                (user: User) => {
                    if (user) {
                        this.order.userId = user.id;
                        this.order.customerName = user.fullName;
                        this.order.customerEmail = user.email;
                    }
                }
            );

        this.shoppingCartService.getCartItems()
            .takeUntil(this.destroy$)
            .subscribe(
                (data: Product[]) => {
                    this.order.products = data;
                    
                    if (data.length > 0) {
                        this.order.price = data
                            .map((prod) => prod.price)
                            .reduce((prev, curr) => prev + curr);
                    }
                }
            );
    }

    private makeOrder() {
        this.orderService.create(this.order)
            .subscribe(
                (data: Order) => {
                    this.shoppingCartService.clean();
                    this.alertService.info(`Order "${data.id}" was accepted. Please, wait for an email with further instructions.`);
                    this.close();
                }, 
                (error: any) => {
                    this.alertService.info(error.error);
                }
            );
    }

    private close() {
        this.activeModal.close();
    }

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }
}