import { Component, OnInit, Input } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { AlertService } from "../../../api/alert.service";
import { OrderService } from "../../../api/order.service";
import { Order } from "../../../shared/models/order";
import { OrderStatus } from "../../../shared/enums/order-status";

@Component({
    templateUrl: './edit-order.component.html'
})
export class EditOrderComponent implements OnInit {
    @Input() order: Order;
    notifyMessage: string;
    isNotifiable: boolean;

    constructor(private activeModal: NgbActiveModal,
                private orderService: OrderService,
                private alertService: AlertService) {
    }

    ngOnInit() {
    }

    private save() {
        this.orderService.update(this.order).subscribe(data => {
            if (this.isNotifiable) {
                // send Notification
            }
        },
            error => this.alertService.warning(error.error));
        this.close();
    }

    private delete() {
        this.orderService.delete(this.order.id).subscribe(data => { },
            error => this.alertService.warning(error.error));
        this.close();
    }

    private close() {
        this.activeModal.close();
    }

    private getOrderStatuses(): string[] {
        const keys = Object.keys(OrderStatus);
        return keys.slice(keys.length / 2);
    }
}