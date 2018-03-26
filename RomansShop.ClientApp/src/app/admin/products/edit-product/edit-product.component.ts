import { Component, OnInit, Input } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { FormGroup, FormBuilder, Validators, FormControl } from "@angular/forms";

import { Product } from "../../../shared/product";
import { ProductService } from "../../../api/product.service";
import { Category } from "../../../shared/category";
import { CategoryService } from "../../../api/category.service";

@Component({
    templateUrl: './edit-product.component.html'
})
export class EditProductComponent implements OnInit {
    categories: Category[];
    productId: string;
    isLoaded: boolean;
    productForm: FormGroup;

    constructor(
        private productService: ProductService,
        private categoryService: CategoryService,
        private router: Router,
        private activeRoute: ActivatedRoute,
        private formBuilder: FormBuilder) {

        this.productId = activeRoute.snapshot.params["id"];

        this.productForm = this.formBuilder.group({
            "name": ["", [Validators.required, Validators.maxLength(30)]],
            "categoryId": [""],
            "price": ["", [Validators.required]],
            "description": ["", [Validators.maxLength(255)]],
        });
    }

    ngOnInit() {
        if (this.productId != null) {
            this.loadProduct(this.productId);
        } else {
            this.isLoaded = true;
        }

        this.loadCategories();
    }

    loadCategories() {
        this.categoryService.getCategories()
            .subscribe((data: Category[]) => this.categories = data);
    }

    loadProduct(id: string) {
        this.productService.getById(id)
            .subscribe((data: Product) => {
                this.productForm.controls["name"].setValue(data.name);
                this.productForm.controls["categoryId"].setValue(data.categoryId);
                this.productForm.controls["price"].setValue(data.price);
                this.productForm.controls["description"].setValue(data.description);
                this.isLoaded = true;
            });
    }

    save() {
        let product: Product = new Product();
        product.id = this.productId;
        product.name = this.productForm.controls["name"].value;
        product.categoryId = this.productForm.controls["categoryId"].value;
        product.price = this.productForm.controls["price"].value;
        product.description = this.productForm.controls["description"].value;

        if (this.productId == null) {
            this.productService.create(product).subscribe(data => this.router.navigateByUrl("admin/products"));
        } else {
            this.productService.update(product).subscribe(data => this.router.navigateByUrl("admin/products"));
        }
    }
}