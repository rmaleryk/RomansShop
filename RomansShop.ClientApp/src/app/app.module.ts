import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { ProductGridComponent } from './home/product-grid/product-grid.component';
import { ShoppingCartComponent } from './home/shopping-cart/shopping-cart.component';
import { AppRoutes } from './app.routes';
import { CategoryService } from './api/category.service';
import { ProductService } from './api/product.service';
import { AlertService } from './api/alert.service';
import { ShoppingCartService } from './api/shopping-cart.service';
import { OrderComponent } from './home/order/order.component';
import { SignInComponent } from './home/sign-in/sign-in.component';
import { AuthenticationService } from './api/authentication.service';
import { UserService } from './api/user.service';
import { SignUpComponent } from './home/sign-up/sign-up.component';
import { OrderService } from './api/order.service';
import { OrdersHistoryComponent } from './home/orders-history/orders-history.component';
import { UserSettingsComponent } from './home/user-settings/user-settings.component';
import { AdminModule } from './admin/admin.module';

@NgModule({
  declarations: [
    AppComponent,
    ProductGridComponent,
    ShoppingCartComponent,
    OrderComponent,
    SignInComponent,
    SignUpComponent,
    OrdersHistoryComponent,
    UserSettingsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule.forRoot(AppRoutes),
    NgbModule.forRoot(),
    AdminModule
  ],
  entryComponents: [
    ShoppingCartComponent, 
    OrderComponent,
    SignInComponent,
    SignUpComponent,
  ],
  providers: [
    CategoryService, 
    ProductService, 
    AlertService, 
    ShoppingCartService,
    AuthenticationService,
    UserService,
    OrderService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }