import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";

import { CategoryService } from "../../../api/category.service";
import { ProductService } from "../../../api/product.service";
import { Category } from "../../../shared/models/category";
import { Product } from "../../../shared/models/product";

@Component({
  templateUrl: './admin.products.component.html'
})
export class AdminProductsComponent implements OnInit {
  products: Product[];
  categories: Category[];
  selectedCategoryId: string;
  isLoaded: boolean = false;

  constructor(private productService: ProductService,
              private categoryService: CategoryService) {
  }

  ngOnInit() {
    this.loadProducts();
    this.loadCategories();
  }

  loadProducts() {
    if (this.selectedCategoryId == null || this.selectedCategoryId == "undefined") {
      this.productService.getProducts()
        .subscribe((data: Product[]) => {
          this.products = data;
          this.isLoaded = true;
        });
    } else {
      this.productService.getByCategoryId(this.selectedCategoryId)
        .subscribe((data: Product[]) => {
          this.products = data;
          this.isLoaded = true;
        });
    }
  }

  loadCategories() {
    this.categoryService.getCategories()
      .subscribe((data: Category[]) => this.categories = data);
  }

  delete(id: string) {
    if (confirm("Are you sure to delete?")) {
      this.productService.delete(id).subscribe(data => this.loadProducts());
    }
  }
}