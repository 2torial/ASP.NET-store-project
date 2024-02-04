import { useEffect, useState } from 'react';
import './Store.css';
import Filters from './Filters';
import Settings from './Settings';
import ItemList from './ItemList';

enum FormID {
	Filters = "filters",
	Settings = "settings",
}

interface StoreComponentData {
	settings: StoreSettings;
	filters: StoreFilters;
	items: StoreItems;
}

interface StoreSettings {
	categories: Category[];
	selectedCategory: Category;
	pages: number;
	selectedPage: number;
	sortingMethods: string[];
	selectedSortingMethod: string;
}
type Category = {
	type: string;
	label: string;
}

interface StoreFilters {
	priceRange: ValueRange;
    configurations: PossibleConfiguration[];
}
type ValueRange = {
	from: number;
	to: number;
}
type PossibleConfiguration = {
	label: string;
	parameters: string[];
}

interface StoreItems {
	numberOfItems: number;
	displayedItems: Item[];
}
type Item = {
	name: string;
	price: number;
	gallery: string[];
	specification: Configuration[];
	pageLink?: string;
}
type Configuration = {
	label: string;
	parameter: string;
}

export function Store() {
	const [settings, setSettings] = useState<StoreSettings>();
	const [filters, setFilters] = useState<StoreFilters>();
	const [items, setItems] = useState<StoreItems>();

	const collectData = (...ids: string[]) : FormData => {
		let data = undefined;
		for (const id of ids) {
			const form: HTMLFormElement | null = document.querySelector(`form#${id}`);
			if (form !== null) {
				if (data === undefined) data = new FormData(form);
				else for (const [name, value] of new FormData(form).entries()) {
					data.append(name, value);
				}
			}
		}
		return data !== undefined ? data : new FormData();
	};
	
	const reloadStorePage = (async (formData: FormData) => {
		const response = await fetch('/api/reload', {
			method: "post",
			body: formData
		});
		const data: StoreComponentData = await response.json();
		setSettings(data.settings);
		setFilters(data.filters);
		setItems(data.items);
	});

	const updateFilters = (async () => {
		reloadStorePage(collectData(FormID.Filters, FormID.Settings));
	});

	const updateSettings = (async () => {
		reloadStorePage(collectData(FormID.Settings));
	});

	useEffect(() => {
		reloadStorePage(new FormData());
	}, []);

	if (settings === undefined || filters === undefined || items === undefined)
		return <main><p>Store component is loading</p></main>;
	
	const filterProps = {
		...filters,
		updateFilters,
		resetFilters: updateSettings
	}
	const settingsProps = {
		...settings,
		updateSettings
	}
	return <main id="store">
		<Filters {...filterProps}/>
		<Settings {...settingsProps} />
		<ItemList {...items} />
	</main>;
}

export default Store;