import { Component, OnInit, Input } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { AlertService } from "../../../../api/alert.service";
import { OrderService } from "../../../../api/order.service";
import { Order } from "../../../../shared/models/order";
import { OrderStatus } from "../../../../shared/enums/order-status";

@Component({
    templateUrl: './edit-order.component.html'
})
export class EditOrderComponent implements OnInit {
    @Input() order: Order;
    notifyMessage: string;
    isNotifiable: boolean;
    orderStatuses: string[];

    constructor(private activeModal: NgbActiveModal,
                private orderService: OrderService,
                private alertService: AlertService) {
    }

    ngOnInit() {
        this.orderStatuses = Object.values(OrderStatus);
    }

    private save() {
        this.orderService.update(this.order)
            .subscribe(
                (data: any) => {
                    if (this.isNotifiable) {
                        // send Notification
                    }
                },
                (error: any) => this.alertService.warning(error.error)
            );

        this.close();
    }

    private delete() {
        this.orderService.delete(this.order.id)
            .subscribe(
                (data: any) => { },
                (error: any) => this.alertService.warning(error.error)
            );
            
        this.close();
    }

    private close() {
        this.activeModal.close();
    }
}