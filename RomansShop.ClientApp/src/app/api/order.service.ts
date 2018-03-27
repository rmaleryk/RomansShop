import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Subject } from "rxjs/Subject";
import 'rxjs/add/operator/takeUntil';

import { Order } from '../shared/models/order';
import { OrderStatus } from '../shared/enums/order-status';
import { AppSettings } from '../shared/constants/app-settings';

@Injectable()
export class OrderService implements OnDestroy {
    private resourceUrl = AppSettings.API_ENDPOINT + "/orders";
    private orders$: BehaviorSubject<Order[]> = new BehaviorSubject<Order[]>([]);
    destroy$: Subject<boolean> = new Subject<boolean>();

    constructor(private http: HttpClient) {
        this.loadOrders();
    }

    getOrders(): Observable<Order[]> {
        return this.orders$.asObservable();
    }

    getById(id: string): Observable<Order> {
        return this.http.get(`${this.resourceUrl}/${id}`)
            .map((data: any) => new Order(data));
    }

    getByUserId(id: string): Observable<Order[]> {
        return this.http.get(`${AppSettings.API_ENDPOINT}/users/${id}/orders`)
            .map((data: any[]) => this.createOrders(data));
    }

    getByStatus(status: OrderStatus): Observable<Order[]> {
        return this.http.get(`${this.resourceUrl}/status/${status}`)
            .map((data: any[]) => this.createOrders(data));
    }

    create(order: Order): Observable<Order> {
        return this.http.post(this.resourceUrl, order)
            .map((data: any) => new Order(data))
            .do(() => this.loadOrders());
    }

    update(order: Order): Observable<Order> {
        return this.http.put(`${this.resourceUrl}/${order.id}`, order)
            .map((data: any) => new Order(data))
            .do(() => this.loadOrders());
    }

    delete(id: string): Observable<string> {
        return this.http.delete(`${this.resourceUrl}/${id}`, { responseType: "text" })
            .do(() => this.loadOrders());
    }

    private loadOrders() {
        this.http.get<Order[]>(this.resourceUrl)
            .map((data: any[]) => this.createOrders(data))
            .takeUntil(this.destroy$)
            .subscribe(
                (data: any) => this.orders$.next(data)
            );
    }

    private createOrders(ordersData: any): Order[] {
        return ordersData.map(order => new Order(order));
    }

    ngOnDestroy() {
        this.destroy$.next(true);
        this.destroy$.unsubscribe();
    }
}