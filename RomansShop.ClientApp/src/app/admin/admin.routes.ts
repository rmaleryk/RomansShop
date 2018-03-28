import { Routes } from "@angular/router";

import { AdminComponent } from "./admin.component";
import { EditProductComponent } from "./products/edit-product/edit-product.component";
import { AdminProductsComponent } from "./products/products-list/products-list.component";
import { AdminCategoriesComponent } from "./categories/admin.categories.component";
import { AdminAuthGuard } from "../admin/core/admin-auth.guard";
import { AdminOrdersComponent } from "./orders/admin.orders.component";
import { AdminUsersComponent } from "./users/admin.users.component";

export const AdminRoutes: Routes = [
    { path: 'admin', component: AdminProductsComponent, canActivate: [AdminAuthGuard] },
    { path: 'admin/products', component: AdminProductsComponent, canActivate: [AdminAuthGuard] },
    { path: 'admin/products/create', component: EditProductComponent, canActivate: [AdminAuthGuard] },
    { path: 'admin/products/edit/:id', component: EditProductComponent, canActivate: [AdminAuthGuard] },
    { path: 'admin/categories', component: AdminCategoriesComponent, canActivate: [AdminAuthGuard] },
    { path: 'admin/orders', component: AdminOrdersComponent, canActivate: [AdminAuthGuard] },
    { path: 'admin/users', component: AdminUsersComponent, canActivate: [AdminAuthGuard] },
    { path: 'admin/**', redirectTo: '/' }
];