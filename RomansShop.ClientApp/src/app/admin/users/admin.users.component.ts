import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

import { AuthenticationService } from "../../api/authentication.service";
import { EditUserComponent } from "../shared/widgets/edit-user/edit-user.component";
import { User } from "../../shared/models/user";
import { UserRights } from "../../shared/enums/user-rights";
import { UserService } from "../../api/user.service";

@Component({
  templateUrl: './admin.users.component.html'
})
export class AdminUsersComponent implements OnInit {
  users: User[];
  userRights: string[];
  selectedUserRights: UserRights | -1 = -1;
  isLoaded: boolean = false;

  constructor(private userService: UserService,
              private authenticationService: AuthenticationService,
              private modalService: NgbModal) {
  }

  ngOnInit() {
    this.loadUsers();
    this.userRights = Object.values(UserRights);
  }

  private loadUsers() {
    ((this.selectedUserRights == null || this.selectedUserRights == -1) ?
      this.userService.getUsers() :
      this.userService.getByRights(this.selectedUserRights))
        .subscribe(
          (data: User[]) => {
            this.users = data;
            this.isLoaded = true;
          }
        );
  }

  private update(user: User) {
    const updatedUser = Object.assign({}, user);
    const modalRef = this.modalService.open(EditUserComponent);
    modalRef.componentInstance.user = updatedUser;
  }
}