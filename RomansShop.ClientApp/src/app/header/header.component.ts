import { Component, OnInit } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

import { CategoryService } from '../api/category.service';
import { Category } from '../shared/models/category';
import { ShoppingCartComponent } from '../home/shopping-cart/shopping-cart.component';
import { ShoppingCartService } from '../api/shopping-cart.service';
import { Product } from '../shared/models/product';
import { AlertService } from '../api/alert.service';
import { IAlert } from '../shared/interfaces/alert';
import { SignInComponent } from '../home/sign-in/sign-in.component';
import { AuthenticationService } from '../api/authentication.service';
import { User } from '../shared/models/user';
import { UserRights } from '../shared/enums/user-rights';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  providers: [NgbModal]
})
export class AppHeader implements OnInit {
  isCollapsed = false;
  cartItemsCount: number;
  categories: Category[];
  alerts: IAlert[] = [];

  constructor(private categoryService: CategoryService,
              private shoppingCartService: ShoppingCartService,
              private alertService: AlertService,
              private authenticationService: AuthenticationService,
              private modalService: NgbModal,
              private router: Router) {
  }

  ngOnInit() {
    this.alertService.getAlerts().subscribe((alerts: IAlert[]) => this.alerts = alerts);

    this.loadCategories();
    this.loadCartItemsCount();
  }

  get currentUser(): User {
    return this.authenticationService.getCurrentUser();
  }

  get hasAdminPanel(): boolean {
    return this.currentUser.rights == UserRights.ADMINISTRATOR ||
      this.currentUser.rights == UserRights.MODERATOR
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