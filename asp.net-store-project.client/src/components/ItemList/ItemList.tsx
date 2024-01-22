import { useEffect, useState } from "react";

import Item from './Item';
import './ItemList.css';

function ItemList() {
    interface StoreItem {
        name: string;
        imageUrl: string;
        price: number;
        info: string[];
    }
    
    const [itemList, setItemList] = useState<StoreItem[]>();
    
    useEffect(() => {
        populateItemList();
    }, []);

    async function populateItemList() {
        const response = await fetch('itemlist');
        const data = await response.json();
        setItemList(data);
    }
    
    return <section className="item-list">
        {itemList === undefined
            ? <p><em>Failed to download item list</em></p>
            : itemList.map(item => <Item details={item} />)
        }
    </section>;
}

export default ItemList;