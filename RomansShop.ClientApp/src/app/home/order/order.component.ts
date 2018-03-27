import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { Product } from '../../shared/models/product';
import { ProductService } from '../../api/product.service';
import { ShoppingCartService } from '../../api/shopping-cart.service';
import { Order } from '../../shared/models/order';
import { AlertService } from '../../api/alert.service';
import { User } from '../../shared/models/user';
import { AuthenticationService } from '../../api/authentication.service';
import { OrderService } from '../../api/order.service';

@Component({
    templateUrl: './order.component.html'
})
export class OrderComponent implements OnInit {
    order: Order = new Order({});

    constructor(private activeModal: NgbActiveModal,
                private shoppingCartService: ShoppingCartService,
                private alertService: AlertService,
                private authenticationService: AuthenticationService,
                private orderService: OrderService) {
    }

    ngOnInit() {
        if (this.currentUser) {
            this.order.userId = this.currentUser.id;
            this.order.customerName = this.currentUser.fullName;
            this.order.customerEmail = this.currentUser.email;
        }

        this.shoppingCartService.getCartItems().subscribe((data: Product[]) => {
            this.order.products = data;
            
            if (data.length > 0) {
                this.order.price = data
                    .map((prod) => prod.price)
                    .reduce((prev, curr) => prev + curr);
            }
        });
    }

    get currentUser(): User {
        return this.authenticationService.getCurrentUser();
    }

    makeOrder() {
        this.orderService.create(this.order)
            .subscribe((data: Order) => {
                this.shoppingCartService.clean();
                this.alertService.info(`Order "${data.id}" was accepted. Please, wait for an email with further instructions.`);
                this.close();
            }, error => {
                this.alertService.info(error.error);
            });
    }

    close() {
        this.activeModal.close();
    }
}