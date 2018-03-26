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

@Component({
    templateUrl: './order.component.html'
})
export class OrderComponent implements OnInit {
    order: Order = new Order();

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
            this.order.price = 0;
            data.forEach(product => this.order.price += product.price);
        });
    }

    get currentUser(): User {
        return this.authenticationService.getCurrentUser();
    }

    makeOrder() {
        this.orderService.create(this.order)
            .subscribe((data: Order) => {
                console.log(data);
                this.shoppingCartService.clean();
                this.alertService.info(`Order "${data.id}" was accepted. Please, wait for an email with further instructions.`);
                this.close();
            }, error => {
                console.log(error);
            });
    }

    close() {
        this.activeModal.close();
    }
}