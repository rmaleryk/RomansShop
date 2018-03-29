import { ISorter } from "../../shared/interfaces/sorter";
import { ISorterSettings } from "../../shared/interfaces/sorter-settings";
import { SortingMode } from "../../shared/enums/sorting-mode";

export class MergeSorter implements ISorter {
    sorterSettings: ISorterSettings;

    sort(sortable: any[], sorterSettings: ISorterSettings): any[] {
        const array: any[] = Object.assign([], sortable);
        this.sorterSettings = sorterSettings;

        return this.mergeSort(array);
    }

    private mergeSort(array: any[]): any[] {

        if(array.length == 1)
            return array;
        
        let middlePoint = Math.round(array.length / 2);

        return this.merge(this.mergeSort(array.slice(0, middlePoint)), this.mergeSort(array.slice(middlePoint, array.length)));
    }

    private merge(left: any[], right: any[]): any[] {
        let rightIndex = 0;
        let leftIndex = 0;

        const merged: any[] = [];

        for (let i = 0; i < left.length + right.length; i++) {
            if(rightIndex < right.length && leftIndex < left.length) {
                let comparer: boolean = 
                    (this.sorterSettings.sortingMode == SortingMode.ASC)
                        ? left[leftIndex][this.sorterSettings.fieldName] > right[rightIndex][this.sorterSettings.fieldName]
                        : left[leftIndex][this.sorterSettings.fieldName] < right[rightIndex][this.sorterSettings.fieldName];

                if(comparer) {
                    merged[i] = right[rightIndex++];
                } else {
                    merged[i] = left[leftIndex++];
                }

            } else {
                if(rightIndex < right.length) {
                    merged[i] = right[rightIndex++];
                } else {
                    merged[i] = left[leftIndex++];
                }
            }
        }

        return merged;
    }
}