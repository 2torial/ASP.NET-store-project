import { OrderInfo } from '../../../shared/StoreObject/OrderInfo';
import OrderedItemList from './OrderedItemList';
import './Order.css';

interface OrderProps {
    order: OrderInfo;
}

function Order({ order }: OrderProps) {
    const toggleSection = (event: React.MouseEvent<HTMLElement>) => {
        (event.target as HTMLElement).parentElement!.classList.toggle("expanded");
    }

    return <div className="order-section drop-down-section expanded">
        <p className="drop-down-label" onClick={toggleSection}>
            {order.supplierName} | {order.stage}
        </p>
        <div className="range-setting drop-down-content">
            <OrderedItemList products={order.products} />
        </div>
    </div>;
}

export default Order;