import { Component, OnInit, Input } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { CategoryService } from "../../../api/category.service";
import { Category } from "../../../shared/category";
import { AlertService } from "../../../api/alert.service";

@Component({
    templateUrl: './edit-category.component.html'
})
export class EditCategoryComponent implements OnInit {
    @Input() isNewCategory: boolean = true;
    @Input() category: Category;

    constructor(
        public activeModal: NgbActiveModal,
        private categoryService: CategoryService,
        private alertService: AlertService) {
    }

    ngOnInit() {
    }

    save() {
        if (this.isNewCategory) {
            this.categoryService.create(this.category).subscribe(data => { },
                error => this.alertService.warning(error.error));
        } else {
            this.categoryService.update(this.category).subscribe(data => { },
                error => this.alertService.warning(error.error));
        }

        this.close();
    }

    close() {
        this.activeModal.close();
    }
}