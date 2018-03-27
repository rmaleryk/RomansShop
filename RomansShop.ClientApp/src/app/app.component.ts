import { Component, OnInit } from '@angular/core';

import { IAlert } from './shared/interfaces/alert';
import { AlertService } from './api/alert.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: []
})
export class AppComponent implements OnInit {
  alerts: IAlert[] = [];

  constructor(private alertService: AlertService) {
  }

  ngOnInit() {
    this.alertService.getAlerts().subscribe((alerts: IAlert[]) => this.alerts = alerts);
  }

  closeAlert(alert: IAlert) {
    this.alertService.closeAlert(alert);
  } 
}