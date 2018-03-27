import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { debounceTime } from 'rxjs/operator/debounceTime';

import { IAlert } from '../shared/interfaces/alert';

@Injectable()
export class AlertService {
    private alerts$: BehaviorSubject<IAlert[]> = new BehaviorSubject<IAlert[]>([]);

    constructor() {
    }

    getAlerts(): Observable<IAlert[]> {
        return this.alerts$.asObservable();
    }

    info(message: string, timer?: number) {
        this.showAlert(message, "secondary", timer);
    }

    warning(message: string, timer?: number) {
        this.showAlert(message, "secondary", timer);
    }

    private showAlert(message: string, type: string, timer: number) {
        const alerts = this.alerts$.getValue();
        const newLength = alerts.push({ type: type, message: message });

        if (timer != null) {
            debounceTime.call(this.alerts$, timer).subscribe((alerts: IAlert[]) => {
                if (alerts[newLength - 1] != null) {
                    alerts[newLength - 1].message = null;
                }
            });
        }

        this.alerts$.next(alerts);
    }

    closeAlert(alert: IAlert) {
        const alerts = this.alerts$.getValue();
        const index: number = alerts.indexOf(alert);
        alerts.splice(index, 1);

        this.alerts$.next(alerts);
    }
}