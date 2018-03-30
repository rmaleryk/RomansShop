import { ISorter } from "../../shared/interfaces/sorter";
import { ISorterSettings } from "../../shared/interfaces/sorter-settings";
import { SortingMode } from "../../shared/enums/sorting-mode";

export class QuickSorter implements ISorter {
    sorterSettings: ISorterSettings;

    sort(sortable: any[], sorterSettings: ISorterSettings): any[] {
        const array: any[] = Object.assign([], sortable);
        this.sorterSettings = sorterSettings;

        return this.qsort(array, 0, array.length-1);
    }

    private qsort(array: any[], start: number, end: number): any[] {
        if(start >= end) {
            return;
        }
        
        let pivot = this.partition(array, start, end);
        this.qsort(array, start, pivot-1);
        this.qsort(array, pivot+1, end);

        return array;
    }

    private partition(array: any[], start: number, end: number): number {
        let marker: number = start;

        for(let i = start; i <= end; i++) {
            let comparer: boolean = 
                    (this.sorterSettings.sortingMode == SortingMode.ASC)
                        ? array[i][this.sorterSettings.fieldName] < array[end][this.sorterSettings.fieldName]
                        : array[i][this.sorterSettings.fieldName] > array[end][this.sorterSettings.fieldName];
            
            if(comparer) {
                this.swap(array, marker++, i);
            }
        }
        
        this.swap(array, marker, end);
        
        return marker;
    }

    private swap(array: any[], indexA: number, indexB: number) {
        const tmp = array[indexA];
        array[indexA] = array[indexB];
        array[indexB] = tmp;
    }
}