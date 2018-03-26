import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Product } from '../../shared/product';
import { ProductService } from '../../api/product.service';
import { ShoppingCartService } from '../../api/shopping-cart.service';
import { OrderComponent } from '../order/order.component';

@Component({
    templateUrl: './shopping-cart.component.html'
})
export class ShoppingCartComponent implements OnInit {
    cartItems: Product[];
    totalPrice: number = 0;

    constructor(
        public activeModal: NgbActiveModal,
        private modalService: NgbModal,
        private productService: ProductService,
        private shoppingCartService: ShoppingCartService) {
    }

    ngOnInit() {
        this.shoppingCartService.getCartItems().subscribe((data: Product[]) => {
            this.cartItems = data;
            this.setTotalCost();
        });
    }

    addToCart(product: Product) {
        this.shoppingCartService.addCartItem(product);
    }

    delete(index: number) {
        this.cartItems.splice(index, 1);
        this.shoppingCartService.deleteCartItem(index);

        this.setTotalCost();
    }

    setTotalCost() {
        this.totalPrice = 0;
        this.cartItems.forEach(product => this.totalPrice += product.price);
    }

    openOrderForm() {
        this.activeModal.close();
        this.modalService.open(OrderComponent);
    }

    close() {
        this.activeModal.close();
    }
}