import { DeliveryMethod } from "../StoreEnum/DeliveryMethod";
import { ProductInfo } from "./ProductInfo";

// Client-side OrderInfo equivalent
export type OrderInfo = {
    id: string;
    supplierId: string;
    supplierName: string;
    productsCost: number;
    transportCost: number;
    deliveryMethod: DeliveryMethod,
    products: ProductInfo[];
    customerDetails: CustomerInfo;
    adressDetails: AdressInfo;
    stageHistory: OrderStageInfo[];
};

// Client-side CustomerInfo equivalent
export type CustomerInfo = {
    name: string;
    surname: string;
    phoneNumber: string;
    email: string;
}

// Client-side AdressInfo equivalent
export type AdressInfo = {
    region: string;
    city: string;
    postalCode: string;
    streetName: string;
    houseNumber: string;
    apartmentNumber?: string;
}

// Client-side OrderStageInfo equivalent
export type OrderStageInfo = {
    type: string;
    dateOfCreation: string;
    timeStamp: string;
}