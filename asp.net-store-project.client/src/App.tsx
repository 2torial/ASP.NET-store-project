import { Filters, Footer, ItemList, Nav, Settings } from './components';
import './App.css';

function App() {
	return <>
		<Nav />
		<main>
			<Filters />
			<Settings />
			<ItemList />
		</main>
		<Footer />
	</>;
}

export default App;