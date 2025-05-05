import { OrderInfo } from '../../../shared/StoreObject/OrderInfo';
import AdressDetails from './AdressDetails';
import CustomerDetails from './CustomerDetails';
import './Order.css';

interface OrderProps {
    order: OrderInfo;
}

function Order({ order }: OrderProps) {
    const toggleSection = (event: React.MouseEvent<HTMLElement>) => {
        (event.target as HTMLElement).parentElement!.classList.toggle("expanded");
    }

    return <section className="order-section drop-down-section">
        <div className="order-label" onClick={toggleSection}>
            <p>DATE_PLACEHOLDER</p>
            <p>{order.supplierName}</p>
            <p>{order.stage}</p>
            <p>{order.id}</p>
        </div>
        <div className="order-content drop-down-content">
            <div className="order-products">
                {order.products.map((prod, i) => <div className="order-product" key={`${order.id}:${i}`}>
                    <img src={prod.gallery === null ? "https://placehold.co/50x50" : prod.gallery[0]} alt="product image" />
                    <p>{prod.name}</p>
                    <p>{prod.price}</p>
                </div>)}
            </div>
            <div className="order-details">
                <CustomerDetails details={order.customerDetails} />
                <AdressDetails details={order.adressDetails} />
            </div>
        </div>
    </section>;
}

export default Order;