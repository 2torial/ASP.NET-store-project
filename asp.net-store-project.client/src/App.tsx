import { Footer, Nav, AccountForm, Store } from './components';
import './App.css';
import { useRoutes } from 'react-router-dom';

function App() {
	return <>
		<Nav />
		{useRoutes([
			//...["/", "/store"].map(path => ({ path, element: <Store /> })),
			{ path: "/", element: <Store /> },
			{ path: "/store", element: <Store /> },
			//{ path: "/basket", element: <Basket /> },
			{ path: "/sign-in", element: <AccountForm newAccount={false} /> },
			{ path: "/sign-up", element: <AccountForm newAccount={true} /> },
			//{ path: "/", element: <NoPage /> },
		])}
		<Footer />
	</>;
}

export default App;