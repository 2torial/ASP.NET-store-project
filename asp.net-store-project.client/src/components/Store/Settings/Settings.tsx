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

function Settings({ categories, selectedCategory, pages, selectedPage, sortingMethods, selectedSortingMethod, updateSettings } : SettingsProps) {
    const categorySelect = {
        label: "Category",
        id: "category",
        name: "Category",
        options: categories.map(category => ({ label: category.label, value: category.type })),
        icons: undefined,
        selectedOption: { label: selectedCategory.label, value: selectedCategory.type },
        updateSettings: updateSettings,
    }

    const sortSelect = {
        label: "Sort by",
        id: "sortby",
        name: "SortBy",
        options: sortingMethods.map(method => ({ label: method, value: method })),
        icons: undefined,
        selectedOption: { label: selectedSortingMethod, value: selectedSortingMethod },
        updateSettings: updateSettings,
    }

    const pageSelect = {
        pages: pages,
        selectedPage: selectedPage,
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