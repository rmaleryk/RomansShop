import { ISorter } from "../../shared/interfaces/sorter";
import { BubbleSorter } from "./bubble-sorter";
import { SelectionSorter } from "./selection-sorter";
import { MergeSorter } from "./merge-sorter";
import { QuickSorter } from "./quick-sorter";

export class SorterFactory {
    static getSorter(): ISorter {
        const sorters: ISorter[] = [ 
            new BubbleSorter(),
            new SelectionSorter(), 
            new MergeSorter(),
            new QuickSorter()
        ];
        
        const randomValue = Math.floor(Math.random() * sorters.length);
        console.log("Sorter:", sorters[randomValue]);

        return sorters[randomValue];
    }
}