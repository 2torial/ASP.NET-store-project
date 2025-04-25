import { SortingMethod } from '../../../shared/StoreEnum/StoreSortingMethod';
import { SortingOrder } from '../../../shared/StoreEnum/StoreSortingOrder';
import { ProductInfo } from '../../../shared/StoreObject/ProductInfo';
import Item from './Item';
import './ItemList.css';

interface ItemListProps {
	products: ProductInfo[];
	sortBy: SortingMethod;
	orderBy: SortingOrder;
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
		}).map(product => <Item product={product} key={product.supplierId + product.id} />)}
    </section>;
}

export default ItemList;