import { Component, OnInit, Input, OnDestroy } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FormGroup, FormBuilder, Validators, FormControl } from "@angular/forms";

import { Product } from "../../../shared/models/product";
import { ProductService } from "../../../api/product.service";
import { Category } from "../../../shared/models/category";
import { CategoryService } from "../../../api/category.service";

@Component({
    templateUrl: './edit-product.component.html'
})
export class EditProductComponent implements OnInit {
    categories: Category[];
    productId: string;
    isLoaded: boolean;
    productForm: FormGroup;

    constructor(private productService: ProductService,
                private categoryService: CategoryService,
                private router: Router,
                private activeRoute: ActivatedRoute,
                private formBuilder: FormBuilder) {
    }

    ngOnInit() {
        this.productId = this.activeRoute.snapshot.params["id"];
        
        if (this.productId != null) {
            this.loadProduct(this.productId);
        } else {
            this.buildForm({});
            this.isLoaded = true;
        }

        this.loadCategories();
    }

    private loadCategories() {
        this.categoryService.getCategories()
            .subscribe(
                (data: Category[]) => this.categories = data
            );
    }

    private loadProduct(id: string) {
        this.productService.getById(id)
            .subscribe(
                (data: Product) => {
                    this.buildForm(data);
                    this.isLoaded = true;
                }
            );
    }

    private save() {
        const product: Product = new Product({
            id: this.productId,
            name: this.productForm.controls["name"].value,
            categoryId: this.productForm.controls["categoryId"].value,
            price: this.productForm.controls["price"].value,
            description: this.productForm.controls["description"].value
        });

        (product.id == null ? this.productService.create(product) : this.productService.update(product))
            .subscribe(
                (data: any) => this.router.navigateByUrl("admin/products")
            );
    }

    private buildForm(product: any) {
        this.productForm = this.formBuilder
            .group({
                "name": [product.name, [Validators.required, Validators.maxLength(30)]],
                "categoryId": [product.categoryId],
                "price": [product.price, [Validators.required]],
                "description": [product.description, [Validators.maxLength(255)]]
            });
    }
}