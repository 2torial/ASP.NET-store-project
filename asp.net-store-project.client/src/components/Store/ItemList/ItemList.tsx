import Item from './Item';
import './ItemList.css';

interface ItemListProps {
	numberOfRecords: number;
	records: Item[];
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

function ItemList({ records }: ItemListProps) {  
    return <section className="item-list">
		{records.map(item => <Item {...item} key={item.id} />)}
    </section>;
}

export default ItemList;