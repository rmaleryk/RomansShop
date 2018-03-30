import { Component, OnInit, Input, OnDestroy, HostListener } from '@angular/core';
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
import { SortingMode } from '../../shared/enums/sorting-mode';
import { ISorterSettings } from '../../shared/interfaces/sorter-settings';
import { AppSettings } from '../../shared/constants/app-settings';

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
    sorterOptions = [
                    	{ name: "A-z", settings: { fieldName: 'name', sortingMode: SortingMode.ASC } },
                        { name: "Price low to high", settings: { fieldName: 'price', sortingMode: SortingMode.ASC } },
                        { name: "Price high to low", settings: { fieldName: 'price', sortingMode: SortingMode.DESC } }
					];
					
	selectedOption = this.sorterOptions[0].settings;
	
	constructor(private productService: ProductService,
                private categoryService: CategoryService,
                private shoppingCartService: ShoppingCartService,
                private activeRoute: ActivatedRoute,
                private router: Router,
                private pageConfig: NgbPaginationConfig) {
    }

    ngOnInit() {
        this.pageConfig.pageSize = AppSettings.PAGE_SIZE;
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

    onSortChanged(sorterSettings: ISorterSettings) {
		this.products = this.productService.sort(this.products, sorterSettings);
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
            .subscribe(
                (data: Product[]) => {
                    this.products = data;
                    this.isLoaded = true;
                }
            );
    }

    private loadByCategoryId(categoryId: string) {
        this.productService.getByCategoryId(categoryId)
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