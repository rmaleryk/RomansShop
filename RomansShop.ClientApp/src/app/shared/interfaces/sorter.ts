import { ISorterSettings } from "./sorter-settings";

export interface ISorter {
    sort(sortable: any[], sorterSettings: ISorterSettings): any[];
}