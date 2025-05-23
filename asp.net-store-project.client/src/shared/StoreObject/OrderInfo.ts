import { ProductInfo } from "./ProductInfo";

export type OrderInfo = {
    id: string;
    supplierId: string;
    supplierName: string;
    productsCost: number;
    transportCost: number;
    products: ProductInfo[];
    customerDetails: CustomerInfo;
    adressDetails: AdressInfo;
    stageHistory: OrderStageInfo[];
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

export type OrderStageInfo = {
    type: string;
    dateOfCreation: string;
    timeOfCreation: string;
}