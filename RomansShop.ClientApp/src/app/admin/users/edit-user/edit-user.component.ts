import { Component, OnInit, Input } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { AlertService } from "../../../api/alert.service";
import { User } from "../../../shared/models/user";
import { UserService } from "../../../api/user.service";
import { UserRights } from "../../../shared/enums/user-rights";

@Component({
    templateUrl: './edit-user.component.html'
})
export class EditUserComponent implements OnInit {
    @Input() user: User;

    constructor(private activeModal: NgbActiveModal,
                private userService: UserService,
                private alertService: AlertService) {
    }

    ngOnInit() {
    }

    save() {
        this.userService.update(this.user).subscribe(data => { },
            error => this.alertService.warning(error.error));
        this.close();
    }

    delete() {
        this.userService.delete(this.user.id).subscribe(data => { },
            error => this.alertService.warning(error.error));
        this.close();
    }

    close() {
        this.activeModal.close();
    }

    getUserRights(): Array<string> {
        var keys = Object.keys(UserRights);
        return keys.slice(keys.length / 2);
    }
}