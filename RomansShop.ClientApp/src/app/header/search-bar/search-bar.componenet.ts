import { Component, Input, Output, EventEmitter } from "@angular/core";
import { NgbTypeaheadConfig } from "@ng-bootstrap/ng-bootstrap";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';

import { Product } from "../../shared/models/product";

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

	constructor(config: NgbTypeaheadConfig) {
		config.placement = "bottom-left";
	}

	private onItemSelected($event) {
		this.onSelected.emit($event.item);
	}

	private search = (text$: Observable<string>) =>
		text$
			.debounceTime(300)
			.distinctUntilChanged()
			.map((term: string) => {
				if (term.length >= 2) {
					return this.searchArray
						.filter((element: Product) => 
							element.name
							.toLowerCase()
							.startsWith(term.toLocaleLowerCase()))
						.splice(0, 10);
				}
			});
}