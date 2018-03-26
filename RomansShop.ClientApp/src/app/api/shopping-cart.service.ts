import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';

import { Product } from '../shared/models/product';

@Injectable()
export class ShoppingCartService {
    readonly shoppingCartStorageName: string = "cart";
    private cartItems$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>([]);
    
    constructor() {
        this.loadCartItems();
    }

    getCartItems(): Observable<Product[]> {
        return this.cartItems$.asObservable();
    }

    addCartItem(product: Product) {
        let products: Product[] = JSON.parse(localStorage.getItem(this.shoppingCartStorageName));
        
        if(products == null) {
            products = new Array<Product>();
        }

        products.push(product);

        localStorage.setItem(this.shoppingCartStorageName, JSON.stringify(products));
        this.loadCartItems();
    }

    deleteCartItem(index: number) {
        let products: Product[] = JSON.parse(localStorage.getItem(this.shoppingCartStorageName));
        products.splice(index, 1);

        localStorage.setItem(this.shoppingCartStorageName, JSON.stringify(products));
        this.loadCartItems();
    }

    clean() {
        localStorage.removeItem(this.shoppingCartStorageName);
        this.cartItems$.next([]);
    }

    private loadCartItems(){
        let cartItems: Product[] = JSON.parse(localStorage.getItem(this.shoppingCartStorageName));
        
        if(cartItems == null) {
            cartItems = new Array<Product>();
        }

        this.cartItems$.next(cartItems);
    }
}