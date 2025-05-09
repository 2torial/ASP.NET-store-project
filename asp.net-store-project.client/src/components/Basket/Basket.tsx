import { useEffect, useState } from 'react';
import './Basket.css'
import { FormID, collectData } from '../../shared/FormDataCollection';
import { Link, useNavigate } from 'react-router-dom';
import { ProductInfo } from '../../shared/StoreObject/ProductInfo';

interface BasketComponentData {
    products: ProductInfo[];
}

function Basket() {
    const navigate = useNavigate();
    const [products, setProducts] = useState<ProductInfo[]>([]);

    const reload = async () => {
        const response = await fetch('/api/basket');
        const data: BasketComponentData = await response.json();
        console.log(data);
        setProducts(data.products);
    }

    const addItem = (prod: ProductInfo) => async () => {
        const response = await fetch(`/api/basket/add/${prod.supplierId}/${prod.id}`);
        alert(await response.text());
        reload();
    }

    const removeItem = (prod: ProductInfo) => async () => {
        const response = await fetch(`/api/basket/remove/${prod.supplierId}/${prod.id}`);
        alert(await response.text());
        reload();
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
        reload();
    }, []);

    if (products.length == 0) return <main>Your basket is empty.</main>

    return <main className="basket">
        {products.map(prod => <div className="basketed-item">
            <p>
                <Link to="/product" state={{ supplierId: prod.supplierId, productId: prod.id }}>
                    {prod.name}
                </Link>
            </p>
            <p>{prod.quantity}</p>
            <input type="button" onClick={addItem(prod)} value="Add" />
            <input type="button" onClick={removeItem(prod)} value="Remove" />
            {<img src={prod.thumbnail !== undefined ? prod.thumbnail : "https://placehold.co/150x150"} alt="product" />}
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
            E-mail* <input type="text" name="Email" />
            <input type="submit" value="Submit" />
        </form>
    </main>;
}

export default Basket;