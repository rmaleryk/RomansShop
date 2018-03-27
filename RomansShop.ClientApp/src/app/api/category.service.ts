import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';

import { Category } from '../shared/models/category';
import { AppSettings } from '../shared/constants/app-settings';

@Injectable()
export class CategoryService {
    private url = AppSettings.API_ENDPOINT + "/categories";
    private categories$: BehaviorSubject<Category[]> = new BehaviorSubject<Category[]>([]);

    constructor(private http: HttpClient) {
        this.loadCategories();
    }

    getCategories(): Observable<Category[]> {
        return this.categories$.asObservable();
    }

    getById(id: string): Observable<Category> {
        return this.http.get(`${this.url}/${id}`)
            .map((data: any) => new Category(data));
    }

    create(category: Category): Observable<Category> {
        return this.http.post(this.url, category)
            .map((data: any) => new Category(data))
            .do(() => this.loadCategories());
    }

    update(category: Category): Observable<Category> {
        return this.http.put(`${this.url}/${category.id}`, category)
            .map((data: any) => new Category(data))
            .do(() => this.loadCategories());
    }

    delete(id: string): Observable<string> {
        return this.http.delete(`${this.url}/${id}`, { responseType: "text" })
            .do(() => this.loadCategories());
    }

    private loadCategories() {
        this.http.get(this.url)
            .map((data: any[]) => this.createCategories(data))
            .subscribe((data: Category[]) => this.categories$.next(data));
    }

    private createCategories(categoriesData: any): Category[] {
        return categoriesData.map(category => new Category(category));
    }
}