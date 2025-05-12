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
            <div className="image-section">
                <Link to="/product" state={{ supplierId: prod.supplierId, productId: prod.id }}>
                    <img src={prod.thumbnail !== undefined ? prod.thumbnail : "https://placehold.co/150x150"} alt="product" />
                </Link>
            </div>
            <div className="details-section">
                <h3 className="item-name">
                    <Link to="/product" state={{ supplierId: prod.supplierId, productId: prod.id }}>
                        {prod.name}
                    </Link>
                </h3>
                <h3 className="price">${prod.price}</h3>
                <span className="quantity">
                    <span onClick={addItem(prod)} className="plus-icon fa fa-plus-square" />
                    <span>{prod.quantity}</span>
                    <span onClick={removeItem(prod)} className="minus-icon fa fa-minus-square" />
                </span>
            </div>
        </div>)}
        <form onSubmit={summarize} className="summary" id={FormID.Summary}>
            <div className="input-section">
                <span><label htmlFor="region">Region</label></span>
                <input id="region" type="text" name="Region" />
                <span><label htmlFor="city">City</label></span>
                <input id="city" type="text" name="City" />
                <span><label htmlFor="postal-code">Postal code</label></span>
                <input id="postal-code" type="text" name="PostalCode" />
                <span><label htmlFor="street-name">Street name</label></span>
                <input id="street-name" type="text" name="StreetName" />
                <span><label htmlFor="house-number">House number</label></span>
                <input id="house-number" type="text" name="HouseNumber" />
                <span><label htmlFor="apartment-number">Apartment number</label></span>
                <input id="apartment-number" type="text" name="ApartmentNumber" />
                <span><label htmlFor="name">Name</label></span>
                <input id="name" type="text" name="Name" />
                <span><label htmlFor="surname">Surname</label></span>
                <input id="surname" type="text" name="Surname" />
                <span><label htmlFor="phone-number">Phone number</label></span>
                <input id="phone-number" type="text" name="PhoneNumber" />
                <span><label htmlFor="email">E-mail</label></span>
                <input id="email" type="text" name="Email" />
                <span></span>
                <input className="submit-button" type="submit" value="Submit" />
            </div>
        </form>
    </main>;
}

export default Basket;