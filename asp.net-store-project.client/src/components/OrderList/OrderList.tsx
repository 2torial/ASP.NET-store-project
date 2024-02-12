import { useEffect, useState } from 'react';
import './OrderList.css';

interface OrderListComponentData {
	orders: Order[];
}

interface Order {
	orderId: number;
    customerDetails: UserData;
    adressDetails: AdressData;
    currentStatus: string;
}
type UserData = {
    customerId: string;
    name: string;
    surname: string;
    phoneNumber: string;
    email: string;
}
type AdressData = {
    region: string;
    city: string;
    postalCode: string;
    streetName: string;
    houseNumber: string;
    apartmentNumber: string;
} 

function UserList() {
    const [orders, setOrders] = useState<Order[]>([]);

    useEffect(() => {
		collectOrdersData();
    }, []);

    const collectOrdersData = async () => {
		const response = await fetch('/api/admin/orders');
		if (!response.ok) {
			alert("Error while fetching data");
			console.log(await response.json());
			return;
		}
		const data: OrderListComponentData = await response.json();
		setOrders(data.orders);
	};

	return <main className="orders">
        <table>
            <tr>
                <th>Order ID</th>
                <th>Username</th>
                <th>Name</th>
                <th>Surname</th>
                <th>Phone Number</th>
                <th>E-Mail</th>
                <th>Region</th>
                <th>City</th>
                <th>Postal Code</th>
                <th>Street Name</th>
                <th>House Number</th>
                <th>Apartment Number</th>
                <th>Status</th>
            </tr>
            {orders.map((order, i) => <tr key={i}>
                <td>{order.orderId}</td>
                <td>{order.customerDetails.customerId}</td>
                <td>{order.customerDetails.name}</td>
                <td>{order.customerDetails.surname}</td>
                <td>{order.customerDetails.phoneNumber}</td>
                <td>{order.customerDetails.email}</td>
                <td>{order.adressDetails.region}</td>
                <td>{order.adressDetails.city}</td>
                <td>{order.adressDetails.postalCode}</td>
                <td>{order.adressDetails.streetName}</td>
                <td>{order.adressDetails.houseNumber}</td>
                <td>{order.adressDetails.apartmentNumber}</td>
                <td>{order.currentStatus}</td>
            </tr>)}
        </table>
	</main>;
}

export default UserList;