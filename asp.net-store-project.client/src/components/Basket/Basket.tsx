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

    const groupedProducts = Object.values(products.reduce(
        (acc: { [key: string]: ProductInfo[] }, prod) => {
            if (!acc[prod.supplierId]) {
                acc[prod.supplierId] = [];
            }
            acc[prod.supplierId].push(prod);
            return acc;
        }, {}));

    const productsPrice = products.reduce((acc, { price, quantity }) => acc + price * quantity, 0);
    const transportPrice = groupedProducts.length * 5;

    return <main className="basket">
        <div className="products">
            {groupedProducts.map(prods => <div className="supplier-cart">
                <h3>Products from {prods[0].supplierName}</h3>
                {prods.map(group => group).map(prod => <div className="basketed-item">
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
            </div>)}
        </div>
        <form onSubmit={summarize} className="input-section" id={FormID.Summary}>
            <table className="grid-wide">
                <tr><td>Products cost</td><td>${productsPrice}</td></tr>
                <tr><td>Transport cost</td><td>${transportPrice}</td></tr>
                <tr><td>Payment method</td><td>{"???"}</td></tr>
                <tr><td>Total</td><td>{productsPrice + transportPrice}</td></tr>
            </table>
            <div>
                <label htmlFor="name">Name</label>
                <input id="name" type="text" name="Name" />
            </div>
            <div>
                <label htmlFor="surname">Surname</label>
                <input id="surname" type="text" name="Surname" />
            </div>
            <div>
                <label htmlFor="phone-number">Phone number</label>
                <input id="phone-number" type="text" name="PhoneNumber" />
            </div>
            <div>
                <label htmlFor="email">E-mail</label>
                <input id="email" type="text" name="Email" />
            </div>
            <div>
                <label htmlFor="region">Region</label>
                <input id="region" type="text" name="Region" />
            </div>
            <div>
                <label htmlFor="city">City</label>
                <input id="city" type="text" name="City" />
            </div>
            <div>
                <label htmlFor="postal-code">Postal code</label>
                <input id="postal-code" type="text" name="PostalCode" />
            </div>
            <div>
                <label htmlFor="street-name">Street name</label>
                <input id="street-name" type="text" name="StreetName" />
            </div>
            <div>
                <label htmlFor="house-number">House number</label>
                <input id="house-number" type="text" name="HouseNumber" />
            </div>
            <div>
                <label htmlFor="apartment-number">Apartment number</label>
                <input id="apartment-number" type="text" name="ApartmentNumber" />
            </div>
            <div className="submit-button grid-wide">
                <input type="submit" value="Submit" />
            </div>
        </form>
    </main>;
}

export default Basket;