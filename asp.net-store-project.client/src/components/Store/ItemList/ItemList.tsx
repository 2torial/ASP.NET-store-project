import Item from './Item';
import './ItemList.css';

interface StoreItem {
	name: string;
	price: number;
	images: string[];
	configuration: Record<string, string>;
	//link: string;
}

interface ItemListProps {
    items: StoreItem[];
}

function ItemList({items}: ItemListProps) {  
    return <section className="item-list">
        {items.map(item => <Item {...item} />)}
    </section>;
}

export default ItemList;