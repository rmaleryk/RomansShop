import { Component, OnInit, Input, OnDestroy } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FormGroup, FormBuilder, Validators, FormControl } from "@angular/forms";
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { Product } from "../../../shared/models/product";
import { ProductService } from "../../../api/product.service";
import { Category } from "../../../shared/models/category";
import { CategoryService } from "../../../api/category.service";

@Component({
    templateUrl: './edit-product.component.html'
})
export class EditProductComponent implements OnInit, OnDestroy {
    categories: Category[];
    productId: string;
    isLoaded: boolean;
    productForm: FormGroup;
    destroy$: Subject<boolean> = new Subject<boolean>();

    constructor(private productService: ProductService,
                private categoryService: CategoryService,
                private router: Router,
                private activeRoute: ActivatedRoute,
                private formBuilder: FormBuilder) {
    }

    ngOnInit() {
        this.productId = this.activeRoute.snapshot.params["id"];
        
        this.productForm = this.formBuilder.group({
            "name": ["", [Validators.required, Validators.maxLength(30)]],
            "categoryId": [""],
            "price": ["", [Validators.required]],
            "description": ["", [Validators.maxLength(255)]],
        });

        if (this.productId != null) {
            this.loadProduct(this.productId);
        } else {
            this.isLoaded = true;
        }

        this.loadCategories();
    }

    private loadCategories() {
        this.categoryService.getCategories()
            .takeUntil(this.destroy$)
            .subscribe(
                (data: Category[]) => this.categories = data
            );
    }

    private loadProduct(id: string) {
        this.productService.getById(id)
            .takeUntil(this.destroy$)
            .subscribe(
                (data: Product) => {
                    this.productForm.controls["name"].setValue(data.name);
                    this.productForm.controls["categoryId"].setValue(data.categoryId);
                    this.productForm.controls["price"].setValue(data.price);
                    this.productForm.controls["description"].setValue(data.description);
                    this.isLoaded = true;
                }
            );
    }

    private save() {
        const product: Product = new Product({});
        product.id = this.productId;
        product.name = this.productForm.controls["name"].value;
        product.categoryId = this.productForm.controls["categoryId"].value;
        product.price = this.productForm.controls["price"].value;
        product.description = this.productForm.controls["description"].value;

        if (this.productId == null) {
            this.productService.create(product)
                .takeUntil(this.destroy$)
                .subscribe(
                    (data: any) => this.router.navigateByUrl("admin/products")
                );
        } else {
            this.productService.update(product)
                .takeUntil(this.destroy$)
                .subscribe(
                    (data: any) => this.router.navigateByUrl("admin/products")
                );
        }
    }

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }
}