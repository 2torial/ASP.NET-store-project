import { Footer, Nav, AccountForm, Store } from './components';
import './App.css';
import { Routes, Route } from 'react-router-dom';

function App() {
	return <>
		<Nav />
		<Routes>
			<Route index element={<Store />} />
			{/*<Route path='basket' element={<Basket />} />*/}
			<Route path="sign-in" element={<AccountForm newAccount={false} />} />
			<Route path="sign-up" element={<AccountForm newAccount={true} />} />
			{/*<Route path="*" element={<NoPage />} />*/}
		</Routes>
		<Footer />
	</>;
}

export default App;