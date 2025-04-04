import Paginator from './Paginator';
import SelectList from './SelectList';
import './Settings.css';
import { ProductCategory, productCategoryLabel } from '../../../shared/StoreEnum/StoreProductCategory';
import { convertEnumToChoosable } from '../../../shared/InputChoosable';
import { SortingMethod, sortingMethodLabel } from '../../../shared/StoreEnum/StoreSortingMethod';
import { SortingOrder } from '../../../shared/StoreEnum/StoreSortingOrder';

interface SettingsProps {
    category: ProductCategory;
    pageSize: number;
    pageCount: number;
    pageIndex: number;
    sortingMethod: SortingMethod;
    sortingOrder: SortingOrder;
    updateSettings: () => void;
}

function Settings({ category, pageCount, pageIndex, sortingMethod, updateSettings } : SettingsProps) {
    const categorySelect = {
        label: "Category",
        id: "category",
        name: "Category",
        options: Object.values(ProductCategory).map(cat => convertEnumToChoosable(cat as ProductCategory, productCategoryLabel)),
        icons: undefined,
        selectedOption: convertEnumToChoosable(category as ProductCategory, productCategoryLabel),
        updateSettings: updateSettings,
    }

    const sortSelect = {
        label: "Sort by",
        id: "sortby",
        name: "SortBy",
        options: Object.values(SortingMethod).map(met => convertEnumToChoosable(met as SortingMethod, sortingMethodLabel)),
        icons: undefined,
        selectedOption: convertEnumToChoosable(sortingMethod as SortingMethod, sortingMethodLabel),
        updateSettings: updateSettings,
    }

    const pageSelect = {
        pages: pageCount,
        selectedPageIndex: pageIndex,
        updateSettings: updateSettings,
    }

    const viewSelect = {
        label: undefined,
        id: "view",
        name: "View",
        options: ["Gallery", "List"].map(view => ({ label: view, value: view })),
        icons: { "Gallery": "https://placehold.co/20x20", "List": "https://placehold.co/20x20" },
        selectedOption: { label: "Gallery", value: "Gallery" },
        updateSettings: updateSettings,
    }

    return <form className="settings" id="settings">
        <SelectList {...categorySelect} />
        <SelectList {...sortSelect} />
        <Paginator {...pageSelect} />
        <SelectList {...viewSelect} />        
    </form>;
}

export default Settings;