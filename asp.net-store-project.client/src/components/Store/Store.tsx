import { useEffect, useState, useRef } from 'react';
import './Store.css';
import Filters from './Filters';
import Settings from './Settings';
import ItemList from './ItemList';
import { useLocation } from 'react-router-dom';
import { FormID, collectData } from '../../shared/FormDataCollection';
import { ProductCategory } from '../../shared/StoreEnum/StoreProductCategory';
import { PageSize } from '../../shared/StoreEnum/StorePageSize';
import { SortingMethod } from '../../shared/StoreEnum/StoreSortingMethod';
import { SortingOrder } from '../../shared/StoreEnum/StoreSortingOrder';

interface StoreComponentData {
	settings: StoreSettings;
	filters: StoreFilters;
	products: Product[];
}

interface StoreSettings {
	category: ProductCategory;
	pageSize: PageSize;
	pageCount: number;
	pageIndex: number;
	sortingMethod: SortingMethod;
	sortingOrder: SortingOrder;
}

interface StoreFilters {
	priceRange: PriceRange;
	relatedTags: { [label: string]: ProductTag[] };
}
type PriceRange = {
	from: number;
	to: number;
}

type Product = {
	id: number,
	name: string;
	price: number;
	tags: ProductTag[];
	gallery: string[];
	thumbnail: string;
	pageLink?: string;
}
type ProductTag = {
	label: string;
	parameter: string;
	order: number;
}

export function Store() {
	const location = useLocation()
	const [settings, setSettings] = useState<StoreSettings>();
	const [filters, setFilters] = useState<StoreFilters>();
	const [products, setProducts] = useState<Product[]>();
	const isFirstLoad = useRef(true);

	useEffect(() => {
		if (location.pathname !== "/store") return;
		let data;
		if (isFirstLoad.current) {
			isFirstLoad.current = false;
			data = new FormData();
			data.append("Category", ProductCategory.Laptop.toString());
			data.append("PriceFrom", "0");
			data.append("PriceTo", "99999999");
			data.append("SortBy", SortingMethod.ByPrice.toString());
			data.append("OrderBy", SortingOrder.Ascending.toString());
			data.append("PageSize", PageSize.Take20.toString());
			data.append("PageIndex", "1");
			console.log(data.get("Category"));
		} else {
			const queryParams = new URLSearchParams(location.search);
			if (!queryParams.has("search")) return;
			data = collectData(FormID.Filters, FormID.Settings);
			data.append("SearchBar", queryParams.get("search") ?? "")
		} 
		reloadStorePage(data);
	}, [location])
	
	const reloadStorePage = (async (formData: FormData) => {
		const response = await fetch('/api/reload', {
			method: "post",
			body: formData
		});
		if (!response.ok) {
			alert("Error while fetching data");
			console.log(await response.json());
			return;
		}
		const data: StoreComponentData = await response.json();
		setSettings(data.settings);
		setFilters(data.filters);
		setProducts(data.products);
	});

	const updateFilters = (async () => {
		reloadStorePage(collectData(FormID.Filters, FormID.Settings));
	});

	const updateSettings = (async () => {
		reloadStorePage(collectData(FormID.Settings));
	});

	if (settings === undefined || filters === undefined || products === undefined)
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
		<ItemList products={ products } />
	</main>;
}

export default Store;