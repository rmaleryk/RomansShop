import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { Order } from '../shared/order';
import { OrderStatus } from '../shared/order-status';


@Injectable()
export class OrderService {

    private url = "http://localhost:50725/api";
    private resourceUrl = this.url + "/orders";
    private orders$: BehaviorSubject<Order[]> = new BehaviorSubject<Order[]>([]);

    constructor(private http: HttpClient) {
        this.loadOrders();
    }

    getOrders(): Observable<Order[]> {
        return this.orders$.asObservable();
    }

    getById(id: string): Observable<Order> {
        return this.http.get(`${this.resourceUrl}/${id}`)
            .map((resp: any) => new Order(resp.id, resp.userId, resp.customerEmail, resp.customerName, resp.products, resp.address, resp.price, resp.status));
    }

    getByUserId(id: string): Observable<Order[]> {
        return this.http.get(`${this.url}/users/${id}/orders`)
            .map((data: any) => data.map(function (resp: any) {
                return new Order(resp.id, resp.userId, resp.customerEmail, resp.customerName, resp.products, resp.address, resp.price, resp.status);
            }));
    }

    getByStatus(status: OrderStatus): Observable<Order[]> {
        return this.http.get(`${this.resourceUrl}/status/${status}`)
            .map((data: any) => data.map(function (resp: any) {
                return new Order(resp.id, resp.userId, resp.customerEmail, resp.customerName, resp.products, resp.address, resp.price, resp.status);
            }));
    }

    create(order: Order): Observable<Order> {
        console.log(JSON.stringify(order));
        return this.http.post(this.resourceUrl, order)
            .map((resp: any) => new Order(resp.id, resp.userId, resp.customerEmail, resp.customerName, resp.products, resp.address, resp.price, resp.status))
            .do(() => this.loadOrders());
    }

    update(order: Order): Observable<Order> {
        return this.http.put(`${this.resourceUrl}/${order.id}`, order)
            .map((resp: any) => new Order(resp.id, resp.userId, resp.customerEmail, resp.customerName, resp.products, resp.address, resp.price, resp.status))
            .do(() => this.loadOrders());
    }

    delete(id: string): Observable<string> {
        return this.http.delete(`${this.resourceUrl}/${id}`, { responseType: "text" })
            .do(() => this.loadOrders());
    }

    private loadOrders() {
        this.http.get<Order[]>(this.resourceUrl)
            .map((data: any) => data.map(function (resp: any) {
                return new Order(resp.id, resp.userId, resp.customerEmail, resp.customerName, resp.products, resp.address, resp.price, resp.status);
            }))
            .subscribe((data: Order[]) => this.orders$.next(data));
    }
}