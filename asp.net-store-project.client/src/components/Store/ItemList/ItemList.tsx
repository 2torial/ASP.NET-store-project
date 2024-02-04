import Item from './Item';
import './ItemList.css';

interface ItemListProps {
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

function ItemList({numberOfItems, displayedItems}: ItemListProps) {  
    return <section className="item-list">
        {displayedItems.map(item => <Item {...item} />)}
    </section>;
}

export default ItemList;