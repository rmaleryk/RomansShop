import { ISorter } from "../../shared/interfaces/sorter";
import { ISorterSettings } from "../../shared/interfaces/sorter-settings";
import { SortingMode } from "../../shared/enums/sorting-mode";

export class SelectionSorter implements ISorter {
    sort(sortable: any[], sorterSettings: ISorterSettings): any[] {
        const array: any[] = Object.assign([], sortable);
        let control;

        for (let i = 0; i < array.length - 1; i++) {
            control = i;

            for (let j = i + 1; j < array.length; j++) {
                let comparer: boolean = 
                    (sorterSettings.sortingMode == SortingMode.ASC)
                        ? array[j][sorterSettings.fieldName] < array[control][sorterSettings.fieldName]
                        : array[j][sorterSettings.fieldName] > array[control][sorterSettings.fieldName];

                if(comparer) {
                    control = j;
                }
            }

            if(control != i) {
                this.swap(array, i, control);
            }
        }

        return array;
    }

    private swap(array: any[], indexA: number, indexB: number) {
        const tmp = array[indexA];
        array[indexA] = array[indexB];
        array[indexB] = tmp;
    }
}