export enum SortingMethod {
	ByName,
	ByPrice,
}

export const sortingMethodLabel = new Map<SortingMethod, string>([
	[SortingMethod.ByName, "Name"],
	[SortingMethod.ByPrice, "Price"]
]);