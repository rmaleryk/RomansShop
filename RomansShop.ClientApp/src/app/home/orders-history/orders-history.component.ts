import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";

import { Order } from "../../shared/models/order";
import { OrderService } from "../../api/order.service";
import { AuthenticationService } from "../../api/authentication.service";
import { Product } from "../../shared/models/product";
import { OrderStatus } from "../../shared/enums/order-status";

@Component({
  templateUrl: './orders-history.component.html'
})
export class OrdersHistoryComponent implements OnInit {
  orders: Order[];
  orderStatus = OrderStatus;
  isLoaded: boolean = false;

  constructor(private orderService: OrderService,
              private authenticationService: AuthenticationService) {
  }

  ngOnInit() {
    this.loadOrders();
  }

  private loadOrders() {
    const userId = this.authenticationService.getCurrentUser().id;
    this.orderService.getByUserId(userId)
      .subscribe((data: Order[]) => {
        this.orders = data;
        this.isLoaded = true;
      });
  }
}