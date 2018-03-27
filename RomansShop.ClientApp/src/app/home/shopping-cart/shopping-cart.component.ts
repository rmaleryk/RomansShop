import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Product } from '../../shared/models/product';
import { ProductService } from '../../api/product.service';
import { ShoppingCartService } from '../../api/shopping-cart.service';
import { OrderComponent } from '../order/order.component';

@Component({
    templateUrl: './shopping-cart.component.html'
})
export class ShoppingCartComponent implements OnInit {
    cartItems: Product[] = [];
    totalPrice: number = 0;

    constructor(private activeModal: NgbActiveModal,
                private modalService: NgbModal,
                private productService: ProductService,
                private shoppingCartService: ShoppingCartService) {
    }

    ngOnInit() {
        this.shoppingCartService.getCartItems().subscribe((data: Product[]) => {
            this.cartItems = data;
            this.calculateTotalCost();
        });
    }

    addToCart(product: Product) {
        this.shoppingCartService.addCartItem(product);
    }

    delete(index: number) {
        this.cartItems.splice(index, 1);
        this.shoppingCartService.deleteCartItem(index);

        this.calculateTotalCost();
    }

    calculateTotalCost() {
        this.totalPrice = this.cartItems
            .map((prod) => prod.price)
            .reduce((prev, curr) => prev + curr);
    }

    openOrderForm() {
        this.activeModal.close();
        this.modalService.open(OrderComponent);
    }

    close() {
        this.activeModal.close();
    }
}