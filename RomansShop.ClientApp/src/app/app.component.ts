import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { IAlert } from './shared/interfaces/alert';
import { AlertService } from './api/alert.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: []
})
export class AppComponent implements OnInit, OnDestroy {
  alerts: IAlert[] = [];
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private alertService: AlertService) {
  }

  ngOnInit() {
    this.alertService.getAlerts()
      .takeUntil(this.destroy$)
      .subscribe(
        (alerts: IAlert[]) => this.alerts = alerts
      );
  }

  private closeAlert(alert: IAlert) {
    this.alertService.closeAlert(alert);
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}