import { Component, OnInit, Input, OnDestroy } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { AlertService } from "../../../api/alert.service";
import { OrderService } from "../../../api/order.service";
import { Order } from "../../../shared/models/order";
import { OrderStatus } from "../../../shared/enums/order-status";

@Component({
    templateUrl: './edit-order.component.html'
})
export class EditOrderComponent implements OnInit, OnDestroy {
    @Input() order: Order;
    notifyMessage: string;
    isNotifiable: boolean;
    destroy$: Subject<boolean> = new Subject<boolean>();

    constructor(private activeModal: NgbActiveModal,
                private orderService: OrderService,
                private alertService: AlertService) {
    }

    ngOnInit() {
    }

    private save() {
        this.orderService.update(this.order)
            .takeUntil(this.destroy$)
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
            .takeUntil(this.destroy$)
            .subscribe(
                (data: any) => { },
                (error: any) => this.alertService.warning(error.error)
            );
            
        this.close();
    }

    private close() {
        this.activeModal.close();
    }

    private getOrderStatuses(): string[] {
        const keys = Object.keys(OrderStatus);
        return keys.slice(keys.length / 2);
    }

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }
}