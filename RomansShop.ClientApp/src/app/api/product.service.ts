import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import 'rxjs/add/operator/map';

import { Product } from '../shared/models/product';

@Injectable()
export class ProductService {
    private url = "http://localhost:50725/api";
    private resourceUrl = this.url + "/products";
    private products$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>([]);

    constructor(private http: HttpClient) {
        this.loadProducts();
    }

    getProducts(): Observable<Product[]> {
        return this.products$.asObservable();
    }

    getById(id: string): Observable<Product> {
        return this.http.get(`${this.resourceUrl}/${id}`)
            .map((data: any) => new Product(data.id, data.name, null, data.categoryId, data.price));
    }

    getByCategoryId(id: string): Observable<Product[]> {
        return this.http.get(`${this.url}/categories/${id}/products`)
            .map((data: any) => data.map(function (productResponse: any) {
                return new Product(productResponse.id, productResponse.name, null, productResponse.categoryId, productResponse.price);
            }));
    }

    getRange(startIndex: number, offset: number): Observable<Product[]> {
        return this.http.get(`${this.resourceUrl}/page?startIndex=${startIndex}&offset=${offset}`)
            .map((data: any) => data.map(function (productResponse: any) {
                return new Product(productResponse.id, productResponse.name, null, productResponse.categoryId, productResponse.price);
            }));
    }

    create(product: Product): Observable<Product> {
        return this.http.post(this.resourceUrl, product)
            .map((data: any) => new Product(data.id, data.name, null, data.categoryId, data.price))
            .do(() => this.loadProducts());
    }

    update(product: Product): Observable<Product> {
        return this.http.put(`${this.resourceUrl}/${product.id}`, product)
            .map((data: any) => new Product(data.id, data.name, null, data.categoryId, data.price))
            .do(() => this.loadProducts());
    }

    delete(id: string): Observable<string> {
        return this.http.delete(`${this.resourceUrl}/${id}`, { responseType: "text" })
            .do(() => this.loadProducts());
    }

    private loadProducts() {
        this.http.get(this.resourceUrl)
            .map((data: any) => data.map(function (productResponse: any) {
                return new Product(productResponse.id, productResponse.name, null, productResponse.categoryId, productResponse.price);
            }))
            .subscribe((data: Product[]) => this.products$.next(data));
    }
}