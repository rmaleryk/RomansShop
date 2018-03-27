import { Component, OnInit, OnDestroy } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { AuthenticationService } from "../../../api/authentication.service";
import { EditUserComponent } from "../edit-user/edit-user.component";
import { User } from "../../../shared/models/user";
import { UserRights } from "../../../shared/enums/user-rights";
import { UserService } from "../../../api/user.service";

@Component({
  templateUrl: './admin.users.component.html'
})
export class AdminUsersComponent implements OnInit, OnDestroy {
  users: User[];
  userRights = UserRights;
  selectedUserRights: UserRights = -1;
  isLoaded: boolean = false;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private userService: UserService,
              private authenticationService: AuthenticationService,
              private modalService: NgbModal) {
  }

  ngOnInit() {
    this.loadUsers();
  }

  private getUserRights(): string[] {
    const keys = Object.keys(UserRights);
    return keys.slice(keys.length / 2);
  }

  private loadUsers() {
    if (this.selectedUserRights == null || this.selectedUserRights == -1) {
      this.userService.getUsers()
        .takeUntil(this.destroy$)
        .subscribe(
          (data: User[]) => {
            this.users = data;
            this.isLoaded = true;
          }
        );
    } else {
      this.userService.getByRights(this.selectedUserRights)
        .takeUntil(this.destroy$)
        .subscribe(
          (data: User[]) => {
            this.users = data;
            this.isLoaded = true;
          }
        );
    }
  }

  private update(user: User) {
    const updatedUser = Object.assign({}, user);
    const modalRef = this.modalService.open(EditUserComponent);
    modalRef.componentInstance.user = updatedUser;
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}