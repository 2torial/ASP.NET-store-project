export type ProductInfo = {
    id: string;
    name: string;
    price: number;
    quantity: number;
    supplierId: string;
    supplierName: string;
    thumbnail: string;
    tags: ProductTag[];
    pageContent?: string;
};

export type ProductTag = {
    label: string;
    parameter: string;
    order: number;
}