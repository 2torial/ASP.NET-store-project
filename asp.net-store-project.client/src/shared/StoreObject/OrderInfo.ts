import { ProductInfo } from "./ProductInfo";

export type OrderInfo = {
    id: string;
    supplierId: string;
    supplierName: string;
    products: ProductInfo[];
    customerDetails: CustomerInfo;
    adressDetails: AdressInfo;
    stage: string;
};

export type CustomerInfo = {
    name: string;
    surname: string;
    phoneNumber: string;
    email: string;
}

export type AdressInfo = {
    region: string;
    city: string;
    postalCode: string;
    streetName: string;
    houseNumber: string;
    apartmentNumber: string;
}