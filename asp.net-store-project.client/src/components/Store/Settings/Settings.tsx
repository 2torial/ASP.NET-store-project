import Paginator from './Paginator';
import SelectList from './SelectList';
import './Settings.css'

type IconMap = {[id: string]: string}

interface StoreSettingsProps {
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

function Settings({
    categories, 
    selectedCategory, 
    sortingMethods, 
    selectedSortingMethod,
    pages,
    selectedPage,
    viewModes,
    viewModeIcons,
    selectedViewMode
} : StoreSettingsProps) {
    const categorySelect = {
        label: "Category",
        id: "category",
        name: "Category",
        options: categories,
        icons: undefined,
        selectedOption: selectedCategory
    }

    const sortSelect = {
        label: "Sort by",
        id: "sortby",
        name: "SortBy",
        options: sortingMethods,
        icons: undefined,
        selectedOption: selectedSortingMethod
    }

    const pageSelect = {
        pages: pages,
        selectedPage: selectedPage
    }

    const viewSelect = {
        label: undefined,
        id: "view",
        name: "View",
        options: viewModes,
        icons: viewModeIcons,
        selectedOption: selectedViewMode
    }

    return <form className="settings" id="settings">
        <SelectList {...categorySelect} />
        <SelectList {...sortSelect} />
        <Paginator {...pageSelect} />
        <SelectList {...viewSelect} />        
    </form>;
}

export default Settings;