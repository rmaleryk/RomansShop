import { Component, OnInit } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

import { CategoryService } from './api/category.service';
import { Category } from './shared/category';
import { ShoppingCartComponent } from './home/shopping-cart/shopping-cart.component';
import { ShoppingCartService } from './api/shopping-cart.service';
import { Product } from './shared/product';
import { AlertService } from './api/alert.service';
import { IAlert } from './shared/alert';
import { SignInComponent } from './home/sign-in/sign-in.component';
import { AuthenticationService } from './api/authentication.service';
import { User } from './shared/user';
import { UserRights } from './shared/user-rights';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [NgbModal]
})
export class AppComponent implements OnInit {
  public isCollapsed = false;
  cartItemsCount: number;
  categories: Category[];
  alerts: IAlert[];

  constructor(
    private categoryService: CategoryService,
    private shoppingCartService: ShoppingCartService,
    private alertService: AlertService,
    private authenticationService: AuthenticationService,
    private modalService: NgbModal,
    private router: Router) {
    this.alerts = new Array<IAlert>();
    alertService.getAlerts().subscribe((alerts: IAlert[]) => this.alerts = alerts);
  }

  ngOnInit(): void {
    this.loadCategories();
    this.loadCartItemsCount();
  }

  get currentUser(): User {
    return this.authenticationService.getCurrentUser();
  }

  get hasAdminPanel(): boolean {
    return this.currentUser.rights == UserRights.Administrator ||
      this.currentUser.rights == UserRights.Moderator
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigateByUrl("/")
  }

  loadCartItemsCount() {
    this.shoppingCartService.getCartItems().subscribe((data: Product[]) => {
      if (data != null) {
        this.cartItemsCount = data.length;
      }
    });
  }

  loadCategories() {
    this.categoryService.getCategories()
      .subscribe((data: Category[]) => this.categories = data);
  }

  openShoppingCart() {
    this.modalService.open(ShoppingCartComponent);
  }

  openSignInForm() {
    this.modalService.open(SignInComponent);
  }

  closeAlert(alert: IAlert) {
    this.alertService.closeAlert(alert);
  }
}