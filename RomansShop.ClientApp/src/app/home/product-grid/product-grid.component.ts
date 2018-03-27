import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { NgbPaginationConfig } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { ProductService } from '../../api/product.service';
import { CategoryService } from '../../api/category.service';
import { Product } from '../../shared/models/product';
import { Category } from '../../shared/models/category';
import { ShoppingCartService } from '../../api/shopping-cart.service';

@Component({
    templateUrl: './product-grid.component.html',
    styleUrls: ['./product-grid.component.css'],
    providers: [NgbPaginationConfig]
})
export class ProductGridComponent implements OnInit, OnDestroy {
    products: Product[];
    productsPage: Product[];
    categoryName: string;
    pageNumber: number = 1;
    isLoaded: boolean = false;
    categoryId: string;
    destroy$: Subject<boolean> = new Subject<boolean>();

    constructor(private productService: ProductService,
                private categoryService: CategoryService,
                private shoppingCartService: ShoppingCartService,
                private activeRoute: ActivatedRoute,
                private router: Router,
                private pageConfig: NgbPaginationConfig) {
    }

    ngOnInit() {
        this.pageConfig.pageSize = 6;
        this.categoryId = this.activeRoute.snapshot.params["categoryId"];

        this.activeRoute.params
            .takeUntil(this.destroy$)
            .subscribe(
                (params: any) => {
                    this.categoryId = params["categoryId"];

                    if (this.categoryId == null) {
                        this.categoryName = "All products";
                        this.loadAllProducts();
                    } else {
                        this.setCategoryNameById(this.categoryId);
                        this.loadByCategoryId(this.categoryId);
                    }
                }
            );
    }

    private loadAllProducts() {
        this.productService.getProducts()
            .takeUntil(this.destroy$)
            .subscribe(
                (data: Product[]) => {
                    this.products = data;
                    this.productsPage = this.products.slice(0, this.pageConfig.pageSize);
                    this.isLoaded = true;
                }
            );
    }

    private loadProductsPage() {
        this.productService.getRange(this.pageConfig.pageSize*(this.pageNumber-1) + 1, this.pageConfig.pageSize)
            .takeUntil(this.destroy$)
            .subscribe(
                (data: Product[]) => {
                    this.products = data;
                    this.isLoaded = true;
                }
            );
    }

    private loadByCategoryId(categoryId: string) {
        this.productService.getByCategoryId(categoryId)
            .takeUntil(this.destroy$)
            .subscribe(
                (data: Product[]) => {
                    this.products = data;
                    this.productsPage = this.products.slice(0, this.pageConfig.pageSize);
                    this.isLoaded = true;
                }
            );
    }

    private setCategoryNameById(categoryId: string) {
        this.categoryService.getById(categoryId)
            .takeUntil(this.destroy$)
            .subscribe(
                (data: Category) => this.categoryName = data.name,
                (error: any) => this.categoryName = "Category not found!"
            );
    }

    private addToCart(product: Product) {
        this.shoppingCartService.addCartItem(product);
    }

    private pageChange() {
        const startIndex = this.pageConfig.pageSize * (this.pageNumber-1);
        const endIndex = this.pageConfig.pageSize * this.pageNumber;
        this.productsPage = this.products.slice(startIndex, endIndex);
    }

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }
}