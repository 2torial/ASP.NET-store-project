import Item from './Item';
import './ItemList.css';

interface ItemListProps {
	products: Product[];
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
}

function ItemList({ products }: ItemListProps) {  
    return <section className="item-list">
		{products.map(product => <Item {...product} key={product.id} />)}
    </section>;
}

export default ItemList;