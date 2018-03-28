import { Routes } from "@angular/router";

import { AdminComponent } from "./admin.component";
import { EditProductComponent } from "./products/edit-product/edit-product.component";
import { AdminProductsComponent } from "./products/products-list/products-list.component";
import { AdminCategoriesComponent } from "./categories/admin.categories.component";
import { AuthGuard } from "../admin/core/auth.guard";
import { AdminOrdersComponent } from "./orders/admin.orders.component";
import { AdminUsersComponent } from "./users/admin.users.component";

export const AdminRoutes: Routes = [
    { path: 'admin', component: AdminProductsComponent, canActivate: [AuthGuard] },
    { path: 'admin/products', component: AdminProductsComponent, canActivate: [AuthGuard] },
    { path: 'admin/products/create', component: EditProductComponent, canActivate: [AuthGuard] },
    { path: 'admin/products/edit/:id', component: EditProductComponent, canActivate: [AuthGuard] },
    { path: 'admin/categories', component: AdminCategoriesComponent, canActivate: [AuthGuard] },
    { path: 'admin/orders', component: AdminOrdersComponent, canActivate: [AuthGuard] },
    { path: 'admin/users', component: AdminUsersComponent, canActivate: [AuthGuard] },
    { path: 'admin/**', redirectTo: '/' }
];