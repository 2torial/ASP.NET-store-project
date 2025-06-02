// Client-side PageSize equivalent
export enum PageSize {
	Take20,
	Take50,
	Take100
}

export const pageSizeLabel = new Map<PageSize, string>([
	[PageSize.Take20, "20"],
	[PageSize.Take50, "50"],
	[PageSize.Take100, "100"]
]);