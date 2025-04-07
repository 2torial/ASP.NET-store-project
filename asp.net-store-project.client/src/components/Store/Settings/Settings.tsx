import Paginator from './Paginator';
import SelectList from './SelectList';
import './Settings.css';
import { ProductCategory, productCategoryLabel } from '../../../shared/StoreEnum/StoreProductCategory';
import { SortingMethod, sortingMethodLabel } from '../../../shared/StoreEnum/StoreSortingMethod';
import { SortingOrder, sortingOrderLabel } from '../../../shared/StoreEnum/StoreSortingOrder';
import CombinedSelectList from './CombinedSelectList';

interface SettingsProps {
    category: ProductCategory;
    pageSize: number;
    pageCount: number;
    pageIndex: number;
    sortingMethod: SortingMethod;
    sortingOrder: SortingOrder;
    updateStorePage: () => void;
}

function Settings({ category, pageCount, pageIndex, sortingMethod, sortingOrder, updateStorePage } : SettingsProps) {
    const categorySelect = {
        label: "Category",
        id: "category",
        name: "Category",
        options: [...productCategoryLabel.entries()].map((kvp) => ({ label: kvp[1], value: kvp[0].toString() })),
        icons: undefined,
        selectedOption: { label: productCategoryLabel.get(category) as string, value: category.toString() },
        updateStorePage: updateStorePage,
    }

    const sortSelect = {
        label: "Sort by",
        id: "sortby",
        nameA: "SortBy",
        nameB: "OrderBy",
        optionsA: [...sortingMethodLabel.entries()].map((kvp) => ({ label: kvp[1], value: kvp[0].toString() })),
        optionsB: [...sortingOrderLabel.entries()].map((kvp) => ({ label: kvp[1], value: kvp[0].toString() })),
        selectedOptionA: { label: sortingMethodLabel.get(sortingMethod) as string, value: sortingMethod.toString() },
        selectedOptionB: { label: sortingOrderLabel.get(sortingOrder) as string, value: sortingOrder.toString() },
        updateStorePage: updateStorePage,
    }

    const pageSelect = {
        pages: pageCount,
        selectedPageIndex: pageIndex,
        updateStorePage: updateStorePage,
    }

    const viewSelect = {
        label: undefined,
        id: "view",
        name: "View",
        options: ["Gallery", "List"].map(view => ({ label: view, value: view })),
        icons: { "Gallery": "https://placehold.co/20x20", "List": "https://placehold.co/20x20" },
        selectedOption: { label: "Gallery", value: "Gallery" },
        updateStorePage: updateStorePage,
    }

    return <form className="settings" id="settings">
        <SelectList {...categorySelect} />
        <CombinedSelectList {...sortSelect} />
        <Paginator {...pageSelect} />
        <SelectList {...viewSelect} />        
    </form>;
}

export default Settings;