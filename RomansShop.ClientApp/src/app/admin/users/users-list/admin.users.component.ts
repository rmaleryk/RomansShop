import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

import { AuthenticationService } from "../../../api/authentication.service";
import { EditUserComponent } from "../edit-user/edit-user.component";
import { User } from "../../../shared/user";
import { UserRights } from "../../../shared/user-rights";
import { UserService } from "../../../api/user.service";

@Component({
  templateUrl: './admin.users.component.html'
})
export class AdminUsersComponent implements OnInit {
  users: User[];
  userRights = UserRights;
  selectedUserRights: UserRights = -1;
  isLoaded: boolean = false;

  constructor(
    private userService: UserService,
    private authenticationService: AuthenticationService,
    private modalService: NgbModal) {
  }

  ngOnInit() {
    this.loadUsers();
  }

  getUserRights(): Array<string> {
    var keys = Object.keys(UserRights);
    return keys.slice(keys.length / 2);
  }

  loadUsers() {
    if (this.selectedUserRights == null || this.selectedUserRights == -1) {
      this.userService.getUsers()
        .subscribe((data: User[]) => {
          this.users = data;
          this.isLoaded = true;
        });
    } else {
      this.userService.getByRights(this.selectedUserRights)
        .subscribe((data: User[]) => {
          this.users = data;
          this.isLoaded = true;
        });
    }
  }

  update(user: User) {
    let updatedUser = Object.assign({}, user);
    let modalRef = this.modalService.open(EditUserComponent);
    modalRef.componentInstance.user = updatedUser;
  }
}