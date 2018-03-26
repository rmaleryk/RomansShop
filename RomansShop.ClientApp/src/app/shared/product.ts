export class Product {
    constructor(
        public id?: string,
        public name?: string,
        public categoryName: string = "No category",
        public categoryId?: string,
        public price?: number,
        public description?: string) {
    }
}