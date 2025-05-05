import { useEffect, useState } from 'react';
import './OrderList.css';
import { OrderInfo } from '../../shared/StoreObject/OrderInfo';
import Order from './Order';

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

    return <main className="orders">{orders === undefined
        ? <p>Page is loading...</p>
        : orders.map((order, i) => <Order order={order} key={i} />)}
	</main>;
}

export default OrderList;