import { Component, OnInit, OnDestroy } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { CategoryService } from "../../../api/category.service";
import { ProductService } from "../../../api/product.service";
import { Category } from "../../../shared/models/category";
import { Product } from "../../../shared/models/product";
import { EditCategoryComponent } from "../edit-category/edit-category.component";
import { AlertService } from "../../../api/alert.service";

@Component({
  templateUrl: './admin.categories.component.html'
})
export class AdminCategoriesComponent implements OnInit, OnDestroy {
  categories: Category[];
  isLoaded: boolean = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private categoryService: CategoryService,
              private alertService: AlertService,
              private modalService: NgbModal) {
  }

  ngOnInit() {
    this.loadCategories();
  }

  private loadCategories() {
    this.categoryService.getCategories()
      .takeUntil(this.destroy$)
      .subscribe(
        (data: Category[]) => {
          this.categories = data;
          this.isLoaded = true;
      });
  }

  private create() {
    const modalRef = this.modalService.open(EditCategoryComponent);

    modalRef.componentInstance.category = new Category({});
    modalRef.componentInstance.isNewCategory = true;
  }

  private update(category: Category) {
    const updatedCategory = Object.assign({}, category);
    const modalRef = this.modalService.open(EditCategoryComponent);

    modalRef.componentInstance.category = updatedCategory;
    modalRef.componentInstance.isNewCategory = false;
  }

  private delete(id: string) {
    if (confirm("Are you sure to delete?")) {
      this.categoryService.delete(id)
        .subscribe(
          (data: any) => { },
          (error: any) => this.alertService.warning(error.error)
        );
    }
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}