import { useEffect, useState } from 'react';
import './ItemList.css';

interface ItemListComponentData {
	items: ItemData[];
}

interface ItemData {
	item: Item;
	isDeleted: boolean;
}
type Item = {
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
	const [items, setItems] = useState<ItemData[]>([]);

	const setAvaliable = (itemId: number) => async () => {
		const response = await fetch(`/api/admin/items/set/avaliable/${itemId}`);
		if (!response.ok) {
			alert("Error while fetching data");
			console.log(await response.json());
			return;
		}
		alert(await response.text());
		window.location.reload();
	}

	const setUnavaliable = (itemId: number) => async () => {
		const response = await fetch(`/api/admin/items/set/unavaliable/${itemId}`);
		if (!response.ok) {
			alert("Error while fetching data");
			console.log(await response.json());
			return;
		}
		alert(await response.text());
		window.location.reload();
	}

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
		<button>Add</button>
        <table>
            <tr>
				<th>Item ID</th>
				<th>Status</th>
                <th>Name</th>
                <th>Price</th>
                <th>Images</th>
                <th>Specification</th>
                <th>Panel</th>
            </tr>
			{items.map((itemData, i) => {
				const item = itemData.item;
				const isDeleted = itemData.isDeleted;
				return <tr key={i}>
					<td>{item.id}</td>
					<td>{isDeleted
						? "REMOVED"
						: "AVALIABLE"}
					</td>
					<td>{item.name}</td>
					<td>{item.price}</td>
					<td>
						{item.gallery.map((image, i) => <img src={image} key={i} />)}
					</td>
					<td>
						<ul>
							{item.specification.map((config, i) => <li key={i}>
								{config.label}: {config.parameter}
							</li>)}
						</ul>
					</td>
					<td>
						<button>Edit</button>
						{isDeleted
							? <button onClick={setAvaliable(item.id)}>Set as Avaliable</button>
							: <button onClick={setUnavaliable(item.id)}>Set as Unavaliable</button>}
					</td>
				</tr>;
			})}
        </table>
	</main>;
}

export default ItemList;