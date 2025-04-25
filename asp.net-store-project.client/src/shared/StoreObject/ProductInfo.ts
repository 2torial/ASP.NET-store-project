export type ProductInfo = {
    id: string;
    name: string;
    price: number;
    quantity: number;
    supplierId: string;
    thumbnail: string;
    gallery: string[];
    tags: ProductTag[];
    pageContent?: string;
};

export type ProductTag = {
    label: string;
    parameter: string;
    order: number;
}