import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { CategoryService } from "../../../api/category.service";
import { ProductService } from "../../../api/product.service";
import { Category } from "../../../shared/models/category";
import { Product } from "../../../shared/models/product";
import { EditCategoryComponent } from "../edit-category/edit-category.component";
import { AlertService } from "../../../api/alert.service";

@Component({
  templateUrl: './admin.categories.component.html'
})
export class AdminCategoriesComponent implements OnInit {
  categories: Category[];
  isLoaded: boolean = false;

  constructor(private categoryService: CategoryService,
              private alertService: AlertService,
              private modalService: NgbModal) {
  }

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getCategories()
      .subscribe((data: Category[]) => {
        this.categories = data;
        this.isLoaded = true;
      });
  }

  create() {
    let modalRef = this.modalService.open(EditCategoryComponent);

    modalRef.componentInstance.category = new Category({});
    modalRef.componentInstance.isNewCategory = true;
  }

  update(category: Category) {
    let updatedCategory = Object.assign({}, category);
    let modalRef = this.modalService.open(EditCategoryComponent);

    modalRef.componentInstance.category = updatedCategory;
    modalRef.componentInstance.isNewCategory = false;
  }

  delete(id: string) {
    if (confirm("Are you sure to delete?")) {
      this.categoryService.delete(id).subscribe(data => { },
        error => this.alertService.warning(error.error));
    }
  }
}