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
	viablePriceRange: PriceRange;
	priceRange: PriceRange;
	groupedTags: { [label: string]: ProductTag[] };
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

const configMockupFilters = (formData: FormData) => {
	formData.append("PriceFrom", "0");
	formData.append("PriceTo", "99999999");
}
const configMockupSettings = (formData: FormData) => {
	formData.append("Category", ProductCategory.Laptop.toString());
	formData.append("SortBy", SortingMethod.ByPrice.toString());
	formData.append("OrderBy", SortingOrder.Ascending.toString());
	formData.append("PageSize", PageSize.Take20.toString());
	formData.append("PageIndex", "1");
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
			reloadStorePage([], (formData) => {
				configMockupFilters(formData);
				configMockupSettings(formData);
			});
		} else {
			const queryParams = new URLSearchParams(location.search);
			if (!queryParams.has("search")) return;
			data = collectData(FormID.Filters, FormID.Settings);
			data.append("SearchBar", queryParams.get("search") ?? "")
			reloadStorePage([FormID.Settings, FormID.Filters])
		} 
	}, [location])
	
	const reloadStorePage = async (formsToLoad: FormID[] = [], configMockup: ((a: FormData) => void) = (_ => void _)) => {
		const formData = collectData(...formsToLoad);

		configMockup(formData);

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
		setProducts(data.products);
		setFilters(data.filters);
	};

	const updateStorePage = async () =>
		reloadStorePage([FormID.Settings, FormID.Filters]);

	const resetStoreFilters = async () => 
		reloadStorePage([FormID.Settings], configMockupFilters);

	if (settings === undefined || filters === undefined || products === undefined)
		return <main><p>Store component is loading</p></main>;
	
	const filterProps = {
		...filters,
		updateStorePage,
		resetStoreFilters
	}
	const settingsProps = {
		...settings,
		updateStorePage
	}
	return <main id="store">
		<Filters {...filterProps} />
		<Settings {...settingsProps} />
		<ItemList products={products} />
	</main>;
}

export default Store;