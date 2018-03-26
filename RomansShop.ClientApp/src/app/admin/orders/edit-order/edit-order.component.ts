import { Component, OnInit, Input } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { AlertService } from "../../../api/alert.service";
import { OrderService } from "../../../api/order.service";
import { Order } from "../../../shared/order";
import { OrderStatus } from "../../../shared/order-status";

@Component({
    templateUrl: './edit-order.component.html'
})
export class EditOrderComponent implements OnInit {
    @Input() order: Order;
    notifyMessage: string;
    isNotifiable: boolean;

    constructor(
        public activeModal: NgbActiveModal,
        private orderService: OrderService,
        private alertService: AlertService) {
    }

    ngOnInit() {
    }

    save() {
        this.orderService.update(this.order).subscribe(data => { 
            if(this.isNotifiable) {
                // send Notification
            }
        },
            error => this.alertService.warning(error.error));
        this.close();
    }

    delete() {
        this.orderService.delete(this.order.id).subscribe(data => { },
            error => this.alertService.warning(error.error));
        this.close();
    }

    close() {
        this.activeModal.close();
    }

    getOrderStatuses(): Array<string> {
        var keys = Object.keys(OrderStatus);
        return keys.slice(keys.length / 2);
    }
}