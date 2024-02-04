import { useEffect, useState } from 'react';
import './Store.css';
import Filters from './Filters';
import Settings from './Settings';
import ItemList from './ItemList';
import { useLocation } from 'react-router-dom';

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
	const {pathname, search} = useLocation()
	const [settings, setSettings] = useState<StoreSettings>();
	const [filters, setFilters] = useState<StoreFilters>();
	const [items, setItems] = useState<StoreItems>();

	useEffect(() => {
		reloadStorePage(new FormData());
	}, []);

	useEffect(() => {
		if (pathname !== "/store") return;
		const queryParams = new URLSearchParams(search);
		if (!queryParams.has("search")) return;
		const data = collectData(FormID.Filters, FormID.Settings);
		data.append("SearchBar", queryParams.get("search")!)
		reloadStorePage(data);
	}, [location])
	
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