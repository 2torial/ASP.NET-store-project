// Client-side SortingOrder equivalent
export enum SortingOrder {
	Ascending,
	Descending,
}

export const sortingOrderLabel = new Map<SortingOrder, string>([
	[SortingOrder.Ascending, "Ascending"],
	[SortingOrder.Descending, "Descending"]
]);