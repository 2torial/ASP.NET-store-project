import { useEffect, useState } from 'react';
import './OrderList.css';
import { OrderInfo } from '../../shared/StoreObject/OrderInfo';
import Order from './Order';
import AdressDetails from './AdressDetails';
import CustomerDetails from './CustomerDetails';

interface OrderListComponentData {
    orders: OrderInfo[];
}

function OrderList() {
    const [orders, setOrders] = useState<OrderInfo[] | undefined>(undefined);

    useEffect(() => {
        collectOrdersData();
    }, []);

    const collectOrdersData = async () => {
		const response = await fetch('/api/orders');
		if (!response.ok) {
			alert("Error while fetching data");
			console.log(await response.json());
			return;
        }
        const data: OrderListComponentData = await response.json();
        console.log(data);
        setOrders(data.orders);
    };

    orders?.forEach((order) => console.log(order.stageHistory[0]));

    return <main className="orders">{orders === undefined
        ? <p>Page is loading...</p>
        : orders.map((order, i) => <div className="order">
            <div className="order-description" key={i}>
                <table>
                    <tr>
                        <th>Shop</th>
                        <th>Issued</th>
                        <th>Order Cost</th>
                        <th>Transport Cost</th>
                        <th>Stage</th>
                        <th>Last Updated</th>
                    </tr>
                    <tr>
                        <td>{order.supplierName}</td>
                        <td>{`${order.stageHistory[0]?.dateOfCreation} (${order.stageHistory[0]?.timeOfCreation})`}</td>
                        <td>${order.productsCost}</td>
                        <td>${order.transportCost}</td>
                        <td>{order.stageHistory[order.stageHistory.length - 1]?.type}</td>
                        <td>{`${order.stageHistory[order.stageHistory.length - 1]?.dateOfCreation} (${order.stageHistory[order.stageHistory.length - 1]?.timeOfCreation})`}</td>
                    </tr>
                </table>
            </div>
            <div className="order-content">
                <div className="ordered-products">
                    {order.products.map(prod => <Order product={prod} />)}
                </div>
                <div className="order-details">
                    <CustomerDetails details={order.customerDetails} />
                    <AdressDetails details={order.adressDetails} />
                </div>
            </div>
        </div>)}
	</main>;
}

export default OrderList;