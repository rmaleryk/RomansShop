import { Component, OnInit, OnDestroy } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { CategoryService } from "../../../api/category.service";
import { ProductService } from "../../../api/product.service";
import { Category } from "../../../shared/models/category";
import { Product } from "../../../shared/models/product";

@Component({
  templateUrl: './products-list.component.html'
})
export class AdminProductsComponent implements OnInit, OnDestroy {
  products: Product[];
  categories: Category[];
  selectedCategoryId: string;
  isLoaded: boolean = false;
  destroy$: Subject<boolean> = new Subject<boolean>();
  
  constructor(private productService: ProductService,
              private categoryService: CategoryService) {
  }

  ngOnInit() {
    this.loadProducts();
    this.loadCategories();
  }

  private loadProducts() {
    ((this.selectedCategoryId == null || this.selectedCategoryId == "undefined") ? 
      this.productService.getProducts().takeUntil(this.destroy$) : 
      this.productService.getByCategoryId(this.selectedCategoryId))
        .subscribe(
          (data: Product[]) => {
            this.products = data;
            this.isLoaded = true;
          }
        );
  }

  private loadCategories() {
    this.categoryService.getCategories()
      .takeUntil(this.destroy$)
      .subscribe(
        (data: Category[]) => this.categories = data
      );
  }

  private delete(id: string) {
    if (confirm("Are you sure to delete?")) {
      this.productService.delete(id)
        .subscribe(
          data => this.loadProducts()
        );
    }
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}