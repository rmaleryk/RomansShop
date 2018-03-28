import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { Product } from '../../models/product';
import { ProductService } from '../../../api/product.service';
import { ShoppingCartService } from '../../../api/shopping-cart.service';
import { OrderComponent } from '../make-order/make-order.component';

@Component({
    templateUrl: './shopping-cart.component.html'
})
export class ShoppingCartComponent implements OnInit, OnDestroy {
    cartItems: Product[] = [];
    totalPrice: number = 0;
    destroy$: Subject<boolean> = new Subject<boolean>();

    constructor(private activeModal: NgbActiveModal,
                private modalService: NgbModal,
                private productService: ProductService,
                private shoppingCartService: ShoppingCartService) {
    }

    ngOnInit() {
        this.shoppingCartService.getCartItems()
            .takeUntil(this.destroy$)   
            .subscribe(
                (data: Product[]) => {
                    this.cartItems = data;
                    this.calculateTotalCost();
                }
            );
    }

    private addToCart(product: Product) {
        this.shoppingCartService.addCartItem(product);
    }

    private delete(index: number) {
        this.cartItems.splice(index, 1);
        this.shoppingCartService.deleteCartItem(index);

        this.calculateTotalCost();
    }

    private calculateTotalCost() {
        if (this.cartItems.length > 0) {
            this.totalPrice = this.cartItems
                .map((prod) => prod.price)
                .reduce((prev, curr) => prev + curr);
        }
    }

    private openOrderForm() {
        this.activeModal.close();
        this.modalService.open(OrderComponent);
    }

    private close() {
        this.activeModal.close();
    }

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }
}