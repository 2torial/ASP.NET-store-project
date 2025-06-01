// Client-side ProductInfo equivalent
export type ProductInfo = {
    id: string;
    basketId: string;
    name: string;
    price: number;
    quantity: number;
    supplierId: string;
    supplierName: string;
    thumbnail: string;
    tags: ProductTag[];
    pageContent: string;
};

// Client-side ProductTag equivalent
export type ProductTag = {
    label: string;
    parameter: string;
    order: number;
}