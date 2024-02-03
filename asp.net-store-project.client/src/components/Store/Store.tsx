import { useEffect, useState } from 'react';
import './Store.css';
import Filters from './Filters';
import Settings from './Settings';
import ItemList from './ItemList';

enum FormID {
	Filters = "filters",
	Settings = "settings",
}

type IconMap = {[id: string]: string}

interface StoreSettings {
	categories: string[];
	selectedCategory: string;
	sortingMethods: string[];
	selectedSortingMethod: string;
	pages: number;
	selectedPage: number;
	viewModes: string[];
	viewModeIcons: IconMap;
	selectedViewMode: string;
}

interface StoreFilters {
	range: { from: number; to: number };
    specifications: Record<string, string[]>;
}

interface StoreItem {
	name: string;
	price: number;
	images: string[];
	configuration: Record<string, string>;
	//link: string;
}

interface StoreBundle {
	settings: StoreSettings;
	filters: StoreFilters;
	items: StoreItem[];
}

export function Store() {
	const [settings, setSettings] = useState<StoreSettings>();
	const [filters, setFilters] = useState<StoreFilters>();
	const [items, setItems] = useState<StoreItem[]>();

	const collectData = (id: string) : FormData => {
		const data = new FormData();
		const form: HTMLFormElement | null = document.querySelector(`form#${id}`);
		if (form !== null) {
			for (const [name, value] of new FormData(form).entries()) {
				data.append(name, value);
			}
		}
		return data;
	};

	const joinedData = (formData1: FormData, formData2: FormData): FormData => {
		for (const pair of formData2)
			formData1.append(pair[0], pair[1]);
		return formData1
	}
	
	const reloadStorePage = (async (formData: FormData) => {
		const response = await fetch('/api/reload', {
			method: "post",
			body: formData
		});
		const data: StoreBundle = await response.json();
		setSettings(data.settings);
		setFilters(data.filters);
		setItems(data.items);
	});

	const updateFilters = (async () => {
		reloadStorePage(joinedData(collectData(FormID.Filters), collectData(FormID.Settings)));
	});

	const updateSettings = (async () => {
		reloadStorePage(collectData(FormID.Settings));
	});

	useEffect(() => {
		reloadStorePage(new FormData());
	}, []);

	if (settings === undefined || filters === undefined || items === undefined)
		return <main><p>Store component is loading</p></main>;

	const settingsProps = {
		...settings,
		updateSettings: updateSettings
	}

	return <main id="store">
		<Filters from={filters.range.from} to={filters.range.to} specifications={filters.specifications} updateFilters={updateFilters} resetFilters={updateSettings} />
		<Settings {...settingsProps}  />
		<ItemList items={items} />
	</main>;
}

export default Store;