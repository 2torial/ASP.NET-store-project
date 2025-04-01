export enum ProductCategory {
	Headset,
	Microphone,
	Laptop,
	PersonalComputer
}

export const productCategoryLabel = new Map<ProductCategory, string>([
	[ProductCategory.Headset, "Headsets"],
	[ProductCategory.Microphone, "Microphones"],
	[ProductCategory.Laptop, "Laptop/Notebooks/Ultrabooks"],
	[ProductCategory.PersonalComputer, "Personal Computer"]
]);