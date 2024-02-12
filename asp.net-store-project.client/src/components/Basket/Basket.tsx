import { useEffect, useState } from 'react';
import './Basket.css'
import { FormID, collectData } from '../../shared/FormDataCollection';
import { useNavigate } from 'react-router-dom';

interface BasketComponentData {
    items: BasketedItem[];
}
type BasketedItem = {
    id: number,
    quantity: number,
    name: string,
    price: number,
    gallery: string[],
    pageLink?: string,
}

function Basket() {
    const navigate = useNavigate();
    const [items, setItems] = useState<BasketedItem[]>([]);

    const reloadBasket = async () => {
        const response = await fetch('/api/basket');
        const data: BasketComponentData = await response.json();
        console.log(data);
        setItems(data.items);
    }

    const addItem = (id: number) => async () => {
        const response = await fetch(`/api/basket/add/${id}`);
        alert(await response.text());
        reloadBasket();
    }

    const removeItem = (id: number) => async () => {
        const response = await fetch(`/api/basket/remove/${id}`);
        alert(await response.text());
        reloadBasket();
    }

    const summarize = async (event: React.SyntheticEvent) => {
        event.preventDefault();
        const response = await fetch('/api/basket/summary', {
            method: "post",
            body: collectData(FormID.Summary),
        });
        alert(await response.text());
        if (response.ok) navigate("/store");
    };

    useEffect(() => {
        reloadBasket();
    }, []);

    if (items.length == 0) return <main>Your basket is empty.</main>

    return <main className="basket">
        {items.map(item => <div className="basketed-item">
            <p>{item.name}</p>
            <p>{item.quantity}</p>
            <input type="button" onClick={addItem(item.id)} value="Add" />
            <input type="button" onClick={removeItem(item.id)} value="Remove" />
            <img src={item.gallery[0]} />
        </div>)}
        <form onSubmit={summarize} className="summary" id={FormID.Summary}>
            Region* <input type="text" name="Region" />
            City <input type="text" name="City" />
            Postal code* <input type="text" name="PostalCode" />
            Street name <input type="text" name="StreetName" />
            House number* <input type="text" name="HouseNumber" />
            Apartment number* <input type="text" name="ApartmentNumber" />
            Name <input type="text" name="Name" />
            Surname* <input type="text" name="Surname" />
            Phone number <input type="text" name="PhoneNumber" />
            E-mail* <input type="text" name="Mail" />
            <input type="submit" value="Submit" />
        </form>
    </main>;
}

export default Basket;