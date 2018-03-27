import { Component, OnInit, Input, OnDestroy } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { AlertService } from "../../../api/alert.service";
import { User } from "../../../shared/models/user";
import { UserService } from "../../../api/user.service";
import { UserRights } from "../../../shared/enums/user-rights";

@Component({
    templateUrl: './edit-user.component.html'
})
export class EditUserComponent implements OnInit, OnDestroy {
    @Input() user: User;
    destroy$: Subject<boolean> = new Subject<boolean>();

    constructor(private activeModal: NgbActiveModal,
                private userService: UserService,
                private alertService: AlertService) {
    }

    ngOnInit() {
    }

    private save() {
        this.userService.update(this.user)
            .takeUntil(this.destroy$)    
            .subscribe(
                (data: any) => { },
                (error: any) => this.alertService.warning(error.error)
            );

        this.close();
    }

    private delete() {
        this.userService.delete(this.user.id)
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

    private getUserRights(): string[] {
        const keys = Object.keys(UserRights);
        return keys.slice(keys.length / 2);
    }

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }
}