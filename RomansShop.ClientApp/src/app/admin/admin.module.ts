import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { EditProductComponent } from './products/edit-product/edit-product.component';
import { AdminComponent } from './admin.component';
import { AdminCategoriesComponent } from './categories/admin.categories.component';
import { AdminProductsComponent } from './products/products-list/products-list.component';
import { AdminOrdersComponent } from './orders/admin.orders.component';
import { EditCategoryComponent } from './shared/widgets/edit-category/edit-category.component';
import { EditUserComponent } from './shared/widgets/edit-user/edit-user.component';
import { AdminUsersComponent } from './users/admin.users.component';
import { EditOrderComponent } from './shared/widgets/edit-order/edit-order.component';
import { AdminAuthGuard } from '../admin/core/admin-auth.guard';
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
    AdminAuthGuard,
    CategoryService, 
    ProductService, 
    AlertService,
    UserService
  ],
  bootstrap: [AdminComponent]
})
export class AdminModule { }
