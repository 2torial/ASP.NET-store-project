import { useEffect, useState } from 'react';
import './ItemList.css';

interface ItemListComponentData {
	items: Item[];
}

interface Item {
	id: number;
	name: string;
	price: number;
	gallery: string[];
	specification: Configuration[];
	pageLink?: string;
}
type Configuration = {
	label: string;
	parameter: string;
}

function ItemList() {
	const [items, setItems] = useState<Item[]>([]);

    useEffect(() => {
		collectItemsData();
    }, []);

	const collectItemsData = async () => {
		const response = await fetch('/api/admin/items');
		if (!response.ok) {
			alert("Error while fetching data");
			console.log(await response.json());
			return;
		}
		const data: ItemListComponentData = await response.json();
		setItems(data.items);
		console.log(data.items);
	};

	return <main className="items">
		{items.map(item => <div>
			{item.name}
		</div>)}
	</main>;
}

export default ItemList;