import { Component, OnInit, OnDestroy } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { Order } from "../../shared/models/order";
import { OrderService } from "../../api/order.service";
import { AuthenticationService } from "../../api/authentication.service";
import { Product } from "../../shared/models/product";
import { OrderStatus } from "../../shared/enums/order-status";
import { User } from "../../shared/models/user";

@Component({
  templateUrl: './orders-history.component.html'
})
export class OrdersHistoryComponent implements OnInit, OnDestroy {
  orders: Order[];
  orderStatuses: string[];
  isLoaded: boolean = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private orderService: OrderService,
              private authenticationService: AuthenticationService) {
  }

  ngOnInit() {
    this.loadOrders();
    this.orderStatuses = Object.values(OrderStatus);
  }

  private loadOrders() {
    this.authenticationService.getCurrentUser()
      .takeUntil(this.destroy$)
      .subscribe(
        (user: User) => {
          if(user != null) {
            this.orderService.getByUserId(user.id)
              .subscribe(
                (data: Order[]) => {
                  this.orders = data;
                  this.isLoaded = true;
                }
              );
          }
        }
      );
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}