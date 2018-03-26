import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

import { Order } from "../../../shared/order";
import { OrderStatus } from "../../../shared/order-status";
import { OrderService } from "../../../api/order.service";
import { AuthenticationService } from "../../../api/authentication.service";
import { Product } from "../../../shared/product";
import { EditOrderComponent } from "../edit-order/edit-order.component";

@Component({
  templateUrl: './admin.orders.component.html'
})
export class AdminOrdersComponent implements OnInit {
  orders: Order[];
  orderStatus = OrderStatus;
  isLoaded: boolean = false;
  selectedOrderStatus: OrderStatus = -1;

  constructor(private orderService: OrderService,
              private authenticationService: AuthenticationService,
              private modalService: NgbModal) {
  }

  ngOnInit() {
    this.loadOrders();
  }

  getOrderStatuses(): Array<string> {
    var keys = Object.keys(OrderStatus);
    return keys.slice(keys.length / 2);
  }

  loadOrders() {
    if (this.selectedOrderStatus == null || this.selectedOrderStatus == -1) {
      this.orderService.getOrders()
        .subscribe((data: Order[]) => {
          this.orders = data;
          this.isLoaded = true;
        });
    } else {
      this.orderService.getByStatus(this.selectedOrderStatus)
        .subscribe((data: Order[]) => {
          this.orders = data;
          this.isLoaded = true;
        });
    }
  }

  update(order: Order) {
    let updatedOrder = Object.assign({}, order);
    let modalRef = this.modalService.open(EditOrderComponent);
    modalRef.componentInstance.order = updatedOrder;
  }
}