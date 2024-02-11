import Item from './Item';
import './ItemList.css';

interface ItemListProps {
	numberOfItems: number;
	displayedItems: Item[];
}
type Item = {
	id: number,
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

function ItemList({ displayedItems }: ItemListProps) {  
    return <section className="item-list">
		{displayedItems.map(item => <Item {...item} key={item.id} />)}
    </section>;
}

export default ItemList;