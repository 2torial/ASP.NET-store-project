import { SortingMethod } from '../../../shared/StoreEnum/StoreSortingMethod';
import { SortingOrder } from '../../../shared/StoreEnum/StoreSortingOrder';
import Item from './Item';
import './ItemList.css';

interface ItemListProps {
	products: Product[];
	sortBy: SortingMethod;
	orderBy: SortingOrder;
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

function ItemList({ products, sortBy, orderBy }: ItemListProps) {  
    return <section className="item-list">
		{products.sort((a, b) => {
			const order = orderBy === SortingOrder.Ascending ? 1 : -1;
			switch (sortBy) {
				case SortingMethod.ByName:
					return order * a.name.localeCompare(b.name);
				case SortingMethod.ByPrice:
					return order * (a.price - b.price)
			}
		}).map(product => <Item {...product} key={product.id} />)}
    </section>;
}

export default ItemList;