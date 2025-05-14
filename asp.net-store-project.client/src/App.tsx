import { Footer, Nav, AccountForm, Store, Basket, UserList, OrderList } from './components';
import './App.css';
import { useRoutes } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { IdentityPolicy } from './shared/StoreEnum/IdentityPolicy';
import Product from './components/Product';

function App() {
	const [userIdentity, setUserIdentity] = useState<IdentityPolicy>(IdentityPolicy.AnonymousUser);

	const updateUserIdentity = async () => {
		const request = await fetch("/api/account/identity");
		if (!request.ok) setUserIdentity(IdentityPolicy.AnonymousUser);
		const text = await request.text();
		switch (text) {
			case "User":
				setUserIdentity(IdentityPolicy.RegularUser);
				break;
			case "Admin":
				setUserIdentity(IdentityPolicy.AdminUser);
				break;
			case "Anonymous":
			default:
				setUserIdentity(IdentityPolicy.AnonymousUser);
		}
	}

	useEffect(() => {
		updateUserIdentity();
	}, []);

	return <>
		<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
		<Nav updateUserIdentity={updateUserIdentity} userIdentity={userIdentity} />
		{useRoutes([
			{ path: "/", element: <Store /> },
			{ path: "/store", element: <Store /> },
			{ path: "/product", element: <Product /> },
			{ path: "/basket", element: <Basket /> },
			{ path: "/sign-in", element: <AccountForm updateUserIdentity={updateUserIdentity} newAccount={false} /> },
			{ path: "/sign-up", element: <AccountForm updateUserIdentity={updateUserIdentity} newAccount={true} /> },
			{ path: "/orders", element: <OrderList /> },
			{ path: "/admin/users", element: <UserList /> },
		])}
		<Footer />
	</>;
}

export default App;