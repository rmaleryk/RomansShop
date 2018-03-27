export class Product {
    id?: string;
    name?: string;
    categoryName: string = "No category";
    categoryId?: string;
    price?: number;
    description?: string;

    constructor(data: any) {
        Object.assign(this, data);
    }
}