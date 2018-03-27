export class Category {
    id?: string;
    name?: string;

    constructor(data: any) {
        Object.assign(this, data);
    }
}