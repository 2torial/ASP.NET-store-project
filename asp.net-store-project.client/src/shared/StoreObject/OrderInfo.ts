import { ProductInfo } from "./ProductInfo";

export type OrderInfo = {
    id: string;
    customerDetails: CustomerInfo;
    adressDetails: AdressInfo;
    orderStage: string;
    products: ProductInfo[];
};

export type CustomerInfo = {
    label: string;
    parameter: string;
    order: number;
}

export type AdressInfo = {
    region: string;
    city: string;
    postalCode: string;
    streetName: string;
    houseNumber: string;
    apartmentNumber: string;
}