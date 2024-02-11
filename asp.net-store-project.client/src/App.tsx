import { Footer, Nav, AccountForm, Store, Basket, UserList } from './components';
import './App.css';
import { useRoutes } from 'react-router-dom';
import { useEffect, useState } from 'react';

type Identity = "Anonymous" | "User" | "Admin";

function App() {
	const [userIdentity, setUserIdentity] = useState<Identity>("Anonymous");

	const updateUserIdentity = async () => {
		const request = await fetch("/api/account/identity");
		if (!request.ok) setUserIdentity("Anonymous");
		const text = await request.text();
		switch (text) {
			case "User":
			case "Admin":
			case "Anonymous":
				setUserIdentity(text);
				break;
			default:
				setUserIdentity("Anonymous");
		}
	}

	useEffect(() => {
		updateUserIdentity();
	}, []);

	return <>
		<Nav updateUserIdentity={updateUserIdentity} userIdentity={userIdentity} />
		{useRoutes([
			{ path: "/", element: <Store /> },
			{ path: "/store", element: <Store /> },
			{ path: "/basket", element: <Basket /> },
			{ path: "/sign-in", element: <AccountForm updateUserIdentity={updateUserIdentity} newAccount={false} /> },
			{ path: "/sign-up", element: <AccountForm updateUserIdentity={updateUserIdentity} newAccount={true} /> },
			//{ path: "/admin/orders", element: <Orders /> },
			{ path: "/admin/users", element: <UserList /> },
			//{ path: "/admin/items", element: <Items /> },
		])}
		<Footer />
	</>;
}

export default App;