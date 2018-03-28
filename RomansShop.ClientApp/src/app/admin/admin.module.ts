import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { EditProductComponent } from './products/edit-product/edit-product.component';
import { AdminComponent } from './admin.component';
import { AdminCategoriesComponent } from './categories/categories-list/admin.categories.component';
import { AdminProductsComponent } from './products/products-list/admin.products.component';
import { AdminOrdersComponent } from './orders/orders-list/admin.orders.component';
import { EditCategoryComponent } from './categories/edit-category/edit-category.component';
import { EditUserComponent } from './users/edit-user/edit-user.component';
import { AdminUsersComponent } from './users/users-list/admin.users.component';
import { EditOrderComponent } from './orders/edit-order/edit-order.component';
import { AuthGuard } from '../admin/core/auth.guard';
import { CategoryService } from '../api/category.service';
import { AlertService } from '../api/alert.service';
import { ProductService } from '../api/product.service';
import { UserService } from '../api/user.service';
import { AdminRoutes } from './admin.routes';
import { GuidDirective } from '../shared/directives/guid.directive';

@NgModule({
  declarations: [
    EditProductComponent,
    AdminComponent,
    AdminProductsComponent,
    AdminCategoriesComponent,
    EditCategoryComponent,
    AdminOrdersComponent,
    EditOrderComponent,
    EditUserComponent,
    AdminUsersComponent,
    GuidDirective
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule.forRoot(AdminRoutes),
    NgbModule.forRoot()
  ],
  exports: [ GuidDirective ],
  entryComponents: [
    EditCategoryComponent, 
    EditOrderComponent,
    EditUserComponent
  ],
  providers: [
    AuthGuard,
    CategoryService, 
    ProductService, 
    AlertService,
    UserService
  ],
  bootstrap: [AdminComponent]
})
export class AdminModule { }
