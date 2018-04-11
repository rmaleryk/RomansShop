import { ISorter } from "../../shared/interfaces/sorter";
import { ISorterSettings } from "../../shared/interfaces/sorter-settings";
import { SortingMode } from "../../shared/enums/sorting-mode";

export class BubbleSorter implements ISorter {
    sort(sortable: any[], sorterSettings: ISorterSettings): any[] {
        const array: any[] = Object.assign([], sortable);
        let isSwapped: boolean;

        for (let i = 0; i < array.length; i++) {
            isSwapped = false;

            for (let j = 0; j < array.length - i - 1; j++) {
                let comparer: boolean = 
                    (sorterSettings.sortingMode == SortingMode.ASC)
                        ? array[j+1][sorterSettings.fieldName] < array[j][sorterSettings.fieldName]
                        : array[j+1][sorterSettings.fieldName] > array[j][sorterSettings.fieldName];

                if(comparer) {
                    this.swap(array, j+1, j);
                    isSwapped = true;
                }
            }

            if(!isSwapped) {
                break;
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