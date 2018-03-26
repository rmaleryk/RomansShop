import { Component, OnInit, Input } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { NgbPaginationConfig } from '@ng-bootstrap/ng-bootstrap';

import { ProductService } from '../../api/product.service';
import { CategoryService } from '../../api/category.service';
import { Product } from '../../shared/product';
import { Category } from '../../shared/category';
import { ShoppingCartService } from '../../api/shopping-cart.service';

@Component({
    templateUrl: './product-grid.component.html',
    styleUrls: ['./product-grid.component.css'],
    providers: [NgbPaginationConfig]
})
export class ProductGridComponent implements OnInit {
    products: Product[];
    productsPage: Product[];
    categoryName: string;
    pageNumber: number = 1;
    isLoaded: boolean = false;
    categoryId: string;

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

        this.activeRoute.params.subscribe(params => {
            this.categoryId = params["categoryId"];

            if (this.categoryId == null) {
                this.categoryName = "All products";
                this.loadAllProducts();
            } else {
                this.setCategoryNameById(this.categoryId);
                this.loadByCategoryId(this.categoryId);
            }
         });
    }

    loadAllProducts() {
        this.productService.getProducts()
            .subscribe((data: Product[]) => {
                this.products = data;
                this.productsPage = this.products.slice(0, this.pageConfig.pageSize);
                this.isLoaded = true;
            });
    }

    loadProductsPage() {
        this.productService.getRange(this.pageConfig.pageSize*(this.pageNumber-1) + 1, this.pageConfig.pageSize)
            .subscribe((data: Product[]) => {
                this.products = data;
                this.isLoaded = true;
            });
    }

    loadByCategoryId(categoryId: string) {
        this.productService.getByCategoryId(categoryId)
            .subscribe((data: Product[]) => {
                this.products = data;
                this.productsPage = this.products.slice(0, this.pageConfig.pageSize);
                this.isLoaded = true;
            });
    }

    setCategoryNameById(categoryId: string) {
        this.categoryService.getById(categoryId)
            .subscribe((data: Category) => {
                this.categoryName = data.name;
            }, error => {;
                this.categoryName = "Category not found!";
            });
    }

    addToCart(product: Product) {
        this.shoppingCartService.addCartItem(product);
    }

    pageChange() {
        let startIndex = this.pageConfig.pageSize * (this.pageNumber-1);
        let endIndex = this.pageConfig.pageSize * this.pageNumber;
        this.productsPage = this.products.slice(startIndex, endIndex);
    }
}