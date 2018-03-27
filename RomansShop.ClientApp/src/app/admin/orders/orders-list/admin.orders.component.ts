import { Component, OnInit, OnDestroy } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { Order } from "../../../shared/models/order";
import { OrderStatus } from "../../../shared/enums/order-status";
import { OrderService } from "../../../api/order.service";
import { AuthenticationService } from "../../../api/authentication.service";
import { Product } from "../../../shared/models/product";
import { EditOrderComponent } from "../edit-order/edit-order.component";

@Component({
  templateUrl: './admin.orders.component.html'
})
export class AdminOrdersComponent implements OnInit, OnDestroy {
  orders: Order[];
  orderStatus = OrderStatus;
  isLoaded: boolean = false;
  selectedOrderStatus: OrderStatus = -1;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private orderService: OrderService,
              private authenticationService: AuthenticationService,
              private modalService: NgbModal) {
  }

  ngOnInit() {
    this.loadOrders();
  }

  private getOrderStatuses(): string[] {
    const keys = Object.keys(OrderStatus);
    return keys.slice(keys.length / 2);
  }

  private loadOrders() {
    if (this.selectedOrderStatus == null || this.selectedOrderStatus == -1) {
      this.orderService.getOrders()
        .takeUntil(this.destroy$)
        .subscribe(
          (data: Order[]) => {
            this.orders = data;
            this.isLoaded = true;
          }
        );
    } else {
      this.orderService.getByStatus(this.selectedOrderStatus)
        .takeUntil(this.destroy$)
        .subscribe(
          (data: Order[]) => {
            this.orders = data;
            this.isLoaded = true;
          }
        );
    }
  }

  private update(order: Order) {
    const updatedOrder = Object.assign({}, order);
    const modalRef = this.modalService.open(EditOrderComponent);
    modalRef.componentInstance.order = updatedOrder;
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}