import Paginator from './Paginator';
import SelectList from './SelectList';
import './Settings.css'

interface SettingsProps {
	categories: Category[];
	selectedCategory: Category;
	pages: number;
	selectedPage: number;
	sortingMethods: string[];
	selectedSortingMethod: string;
    updateSettings: () => void;
}
type Category = {
	type: string;
	label: string;
}

function Settings({
    categories, 
    selectedCategory, 
    pages,
    selectedPage,
    sortingMethods, 
    selectedSortingMethod,
    updateSettings,
} : SettingsProps) {
    const categorySelect = {
        label: "Category",
        id: "category",
        name: "Category",
        options: categories.map(category => category.label),
        icons: undefined,
        selectedOption: selectedCategory.label
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
        options: ["Gallery", "List"],
        icons: {1: "https://placehold.co/20x20", 2: "https://placehold.co/20x20"},
        selectedOption: "Gallery"
    }

    return <form className="settings" id="settings">
        <SelectList {...categorySelect} />
        <SelectList {...sortSelect} />
        <Paginator {...pageSelect} />
        <SelectList {...viewSelect} />        
    </form>;
}

export default Settings;