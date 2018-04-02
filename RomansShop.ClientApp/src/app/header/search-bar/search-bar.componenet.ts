import { Component, Input, Output, EventEmitter } from "@angular/core";
import { NgbTypeaheadConfig } from "@ng-bootstrap/ng-bootstrap";
import { Observable } from "rxjs/Observable";
import { of } from 'rxjs/observable/of';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';

import { Product } from "../../shared/models/product";
import { ProductService } from "../../api/product.service";

@Component({
	selector: 'search-bar',
	templateUrl: './search-bar.component.html',
	styleUrls: ['./search-bar.component.css'],
	providers: [NgbTypeaheadConfig]
})
export class SearchBarComponent {
	@Input() searchArray: Product[];
	@Output() onSelected = new EventEmitter<Product>();

	private resultFormatter = (value: Product) => value.name;
	private inputFormatter = (value: Product) => "";

	constructor(private config: NgbTypeaheadConfig,
				private productService: ProductService) {
		config.placement = "bottom-left";
	}

	private onItemSelected($event) {
		this.onSelected.emit($event.item);
	}

	private search = (text$: Observable<string>) => 
		text$
			.debounceTime(300)
			.distinctUntilChanged()
			.switchMap(
				(term: string) => {
					return (term.length >= 2)
						? this.productService.searchByName(term.toLocaleLowerCase())
						: of([]);
				}
			);
}