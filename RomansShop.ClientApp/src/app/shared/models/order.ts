import { Product } from "./product";
import { OrderStatus } from "../enums/order-status";

export class Order {
    id?: string;
    userId?: string;
    customerEmail?: string;
    customerName?: string;
    products: Product[] = [];
    address?: string;
    price?: number;
    date?: Date;
    status?: OrderStatus;

    constructor(data: any) {
        Object.assign(this, data);
    }
}