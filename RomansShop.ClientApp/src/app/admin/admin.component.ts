import { Component, OnInit, Input } from "@angular/core";

@Component({
  selector: 'admin-panel',
  templateUrl: './admin.component.html'
})
export class AdminComponent implements OnInit {
  @Input() selectedTab: string;

  constructor() {
  }

  ngOnInit() {
  }
}