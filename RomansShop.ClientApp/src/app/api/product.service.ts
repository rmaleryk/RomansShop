import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import 'rxjs/add/operator/map';

import { Product } from '../shared/models/product';
import { AppSettings } from '../shared/constants/app-settings';
import { SortingMode } from '../shared/enums/sorting-mode';
import { ISorter } from '../shared/interfaces/sorter';
import { SorterFactory } from '../core/sorter/sorter-factory';
import { ISorterSettings } from '../shared/interfaces/sorter-settings';

@Injectable()
export class ProductService {
    private resourceUrl = AppSettings.API_ENDPOINT + "/products";
    private products$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>([]);

    constructor(private http: HttpClient) {
        this.loadProducts();
    }

    getProducts(): Observable<Product[]> {
        return this.products$.asObservable();
    }

    sort(products: Product[], sorterSettings: ISorterSettings): Product[] {
        const sorter: ISorter = SorterFactory.getSorter();
        return sorter.sort(products, sorterSettings);
    }

    getById(id: string): Observable<Product> {
        return this.http.get(`${this.resourceUrl}/${id}`)
            .map((data: any) => new Product(data));
    }

    getByCategoryId(id: string): Observable<Product[]> {
        return this.http.get(`${AppSettings.API_ENDPOINT}/categories/${id}/products`)
            .map((data: any[]) => this.createProducts(data));
    }

    getRange(startIndex: number, offset: number): Observable<Product[]> {
        return this.http.get(`${this.resourceUrl}/page?startIndex=${startIndex}&offset=${offset}`)
            .map((data: any[]) => this.createProducts(data));
    }

    searchByName(name: string): Observable<Product[]> {
        return this.http.get(`${this.resourceUrl}/search?name=${name}`)
            .map((data: any[]) => this.createProducts(data));
    }

    create(product: Product): Observable<Product> {
        return this.http.post(this.resourceUrl, product)
            .map((data: any) => new Product(data))
            .do(() => this.loadProducts());
    }

    update(product: Product): Observable<Product> {
        return this.http.put(`${this.resourceUrl}/${product.id}`, product)
            .map((data: any) => new Product(data))
            .do(() => this.loadProducts());
    }

    delete(id: string): Observable<string> {
        return this.http.delete(`${this.resourceUrl}/${id}`, { responseType: "text" })
            .do(() => this.loadProducts());
    }

    private loadProducts() {
        this.http.get(this.resourceUrl)
            .map((data: any[]) => this.createProducts(data))
            .subscribe(
                (data: Product[]) => this.products$.next(data)
            );
    }

    private createProducts(productsData: any): Product[] {
        return productsData.map(product => new Product(product));
    }
}