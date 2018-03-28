import { Routes } from "@angular/router";

import { ProductGridComponent } from "./home/product-grid/product-grid.component";
import { OrdersHistoryComponent } from "./home/orders-history/orders-history.component";
import { UserSettingsComponent } from "./home/user-settings/user-settings.component";
import { AuthGuard } from "./core/auth.guard";

export const AppRoutes: Routes = [
    { path: '', component: ProductGridComponent },
    { path: 'categories/:categoryId', component: ProductGridComponent },
    { path: 'orders', component: OrdersHistoryComponent, canActivate: [AuthGuard] },
    { path: 'settings', component: UserSettingsComponent, canActivate: [AuthGuard] },
    { path: '**', redirectTo: '/' }
];