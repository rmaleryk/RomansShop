import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { CategoryService } from '../api/category.service';
import { Category } from '../shared/models/category';
import { ShoppingCartComponent } from '../shared/widgets/shopping-cart/shopping-cart.component';
import { ShoppingCartService } from '../api/shopping-cart.service';
import { Product } from '../shared/models/product';
import { SignInComponent } from '../shared/widgets/sign-in/sign-in.component';
import { AuthenticationService } from '../api/authentication.service';
import { User } from '../shared/models/user';
import { UserRights } from '../shared/enums/user-rights';
import { ProductService } from '../api/product.service';

@Component({
	selector: 'app-header',
	templateUrl: './header.component.html',
	styleUrls: ['./header.component.css'],
	providers: [NgbModal]
})
export class AppHeader implements OnInit, OnDestroy {
	isCollapsed = false;
	cartItemsCount: number;
	categories: Category[];
	products: Product[];
	currentUser: User;
	hasAdminPanel: boolean;
	destroy$: Subject<boolean> = new Subject<boolean>();

	constructor(private categoryService: CategoryService,
				private productService: ProductService,
				private shoppingCartService: ShoppingCartService,
				private authenticationService: AuthenticationService,
				private modalService: NgbModal,
				private router: Router) {
	}

	ngOnInit() {
		this.loadCategories();
		this.loadProducts();
		this.loadCartItemsCount();
		const userRights = Object.values(UserRights);

		this.authenticationService.getCurrentUser()
			.takeUntil(this.destroy$)
			.subscribe(
				(user: User) => {
					this.currentUser = user;

					if (this.currentUser != null) {
						this.hasAdminPanel = userRights[this.currentUser.rights] == UserRights.ADMINISTRATOR ||
							userRights[this.currentUser.rights] == UserRights.MODERATOR;
					}
				}
			);
	}

	private onItemSelected($event) {
		if($event.id != null) {
			this.router.navigateByUrl(`/products/${$event.id}`);
		}
	}

	private logout() {
		this.authenticationService.logout();
		this.router.navigateByUrl("/")
	}

	private loadCartItemsCount() {
		this.shoppingCartService.getCartItems()
			.takeUntil(this.destroy$)
			.subscribe(
				(data: Product[]) => {
					if (data != null) {
						this.cartItemsCount = data.length;
					}
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

	private loadProducts() {
		this.productService.getProducts()
			.takeUntil(this.destroy$)
			.subscribe(
				(data: Product[]) => this.products = data
			);
	}

	private openShoppingCart() {
		this.modalService.open(ShoppingCartComponent);
	}

	private openSignInForm() {
		this.modalService.open(SignInComponent);
	}

	ngOnDestroy() {
		this.destroy$.next(true);
		this.destroy$.unsubscribe();
	}
}