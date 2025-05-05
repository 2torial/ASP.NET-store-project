import { ProductInfo } from '../../../../shared/StoreObject/ProductInfo';
import OrderedItem from './OrderedItem';
import './OrderedItemList.css';

interface OrderedItemListProps {
	products: ProductInfo[];
}

function OrderedItemList({ products }: OrderedItemListProps) {  
    return <section className="item-list">{products.map(product =>
        <OrderedItem product={product} key={product.supplierId + product.id} />)}
    </section>;
}

export default OrderedItemList;