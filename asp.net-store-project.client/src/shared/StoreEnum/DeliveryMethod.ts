// Client-side DeliveryMethod equivalent
export enum DeliveryMethod {
	Standard,
	Express
}

export const deliveryMethodLabel = new Map<DeliveryMethod, string>([
	[DeliveryMethod.Standard, "Standard Delivery 5"],
	[DeliveryMethod.Express, "Express Delivery 25"]
]);