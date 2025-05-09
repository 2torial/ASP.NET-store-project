import { Link } from 'react-router-dom';
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

    order.products.map((prod, i) => console.log(prod, i));

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
                    <Link to="/product" state={{ supplierId: prod.supplierId, productId: prod.id }}>
                        {<img src={prod.thumbnail !== undefined ? prod.thumbnail : "https://placehold.co/150x150"} alt="product" />}
                    </Link>
                    <Link to="/product" state={{ supplierId: prod.supplierId, productId: prod.id }}>
                        <p>{prod.name}</p>
                    </Link>
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