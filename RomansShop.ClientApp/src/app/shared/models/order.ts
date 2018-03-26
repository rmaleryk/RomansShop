import { Product } from "./product";
import { OrderStatus } from "../enums/order-status";

export class Order {
    constructor(public id?: string,
                public userId?: string,
                public customerEmail?: string,
                public customerName?: string,
                public products: Product[] = [],
                public address?: string,
                public price?: number,
                public status?: OrderStatus) {
    }
}