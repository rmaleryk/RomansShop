import { Component, OnInit, Input, OnDestroy, HostListener } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { ProductService } from '../../api/product.service';
import { Product } from '../../shared/models/product';
import { ShoppingCartService } from '../../api/shopping-cart.service';

@Component({
    templateUrl: './product-details.component.html',
    styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit, OnDestroy {
    product: Product;
    isLoaded: boolean = false;
    productId: string;
	destroy$: Subject<boolean> = new Subject<boolean>();
	
	constructor(private productService: ProductService,
                private shoppingCartService: ShoppingCartService,
                private activeRoute: ActivatedRoute,
                private router: Router) {
    }

    ngOnInit() {
        this.activeRoute.params
            .takeUntil(this.destroy$)
            .subscribe(
                (params: any) => {
                    this.productId = params["productId"];

                    if (this.productId != null) {
                        this.loadProductById(this.productId);
                    } else {
                        this.router.navigateByUrl("/");
                    }
                }
			);
    }

    private loadProductById(id: string) {
        this.productService.getById(id)
            .subscribe(
                (data: Product) => {
                    this.product = data;
                    this.isLoaded = true;
                },
                (error: any) => {
                    this.product = null;
                    this.isLoaded = true;
                }
            );
    }

    private addToCart(product: Product) {
        this.shoppingCartService.addCartItem(product);
    }

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }
}