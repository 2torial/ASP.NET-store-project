import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { FormID, collectData } from '../../shared/FormDataCollection';
import { DeliveryMethod, deliveryMethodLabel } from '../../shared/StoreEnum/DeliveryMethod';
import { ProductInfo } from '../../shared/StoreObject/ProductInfo';
import './Basket.css';

interface BasketComponentData {
    products: ProductInfo[];
}

function Basket() {
    const [errors, setErrors] = useState({
        "CustomerDetails.Name": [],
        "CustomerDetails.Surname": [],
        "CustomerDetails.PhoneNumber": [],
        "CustomerDetails.Email": [],
        "AdressDetails.Region": [],
        "AdressDetails.City": [],
        "AdressDetails.PostalCode": [],
        "AdressDetails.StreetName": [],
        "AdressDetails.HouseNumber": [],
        "AdressDetails.ApartmentNumber": [],

        other: [],
    });
    const navigate = useNavigate();
    const [products, setProducts] = useState<ProductInfo[]>([]);

    const groupedProducts = Object.values(products.reduce(
        (acc: { [key: string]: ProductInfo[] }, prod) => {
            if (!acc[prod.supplierId])
                acc[prod.supplierId] = [];
            acc[prod.supplierId].push(prod);
            return acc;
        }, {}));
    const supplierIds = groupedProducts.map(prods => prods[0].supplierId);

    const [productsPrice, setProductsPrice] = useState(0);
    const [deliveryCost, setDeliveryCost] = useState(0);

    const reload = async () => {
        const response = await fetch('/api/basket');
        const data: BasketComponentData = await response.json();
        console.log(data);
        if (response.ok) {
            setProducts(data.products);
            updatePrices(data.products);
        } else alert(await response.text());
    }

    const deliveryCostOf = (deliveryMethod: DeliveryMethod | null) =>
        deliveryMethod === DeliveryMethod.Standard
            ? 5 : deliveryMethod === DeliveryMethod.Express
                ? 25 : 0;
        
    const readOrderData = (supplierId: string): [string[], DeliveryMethod | null] => {
        const data = collectData(`sup-${supplierId}`);
        const selectedIds = data.getAll("SelectedBasketIds").map(entry => entry.toString());
        const deliveryMethod = parseInt(data.get("DeliveryMethod")!.toString()) as DeliveryMethod;

        return [selectedIds, selectedIds.length > 0 ? deliveryMethod : null];
    }  

    const updatePrices = (selectedProducts: ProductInfo[] | undefined = undefined) => {
        let productsCost: number = 0;
        let deliveryCost: number = 0;

        if (selectedProducts === undefined) {
            [productsCost, deliveryCost] = supplierIds.reduce((acc, supId) => {
                const [selectedIds, deliveryMethod] = readOrderData(supId);
                return [products.reduce((cost, prod) => selectedIds.includes(prod.basketId)
                    ? cost + prod.price * prod.quantity
                    : 0, acc[0]),
                acc[1] + deliveryCostOf(deliveryMethod)];
            }, [0, 0]);
        } else {
            [productsCost, deliveryCost] = [
                selectedProducts.reduce((acc, prod) => acc + prod.price * prod.quantity, 0),
                [...selectedProducts.reduce((acc, prod) => {
                    acc.add(prod.supplierId);
                    return acc;
                }, new Set<string>())].length * deliveryCostOf(DeliveryMethod.Standard)
            ];
        }

        setProductsPrice(productsCost);
        setDeliveryCost(deliveryCost);
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
        const summaryData = collectData(FormID.Summary);

        const data = {
            orders: supplierIds
                .map(supId => readOrderData(supId))
                .filter(data => data[0].length > 0)
                .map(data => {
                    const prods = products.filter(prod => data[0].includes(prod.basketId));
                    return {
                        productBasketIds: prods.map(prod => prod.basketId),
                        deliveryMethod: data[1]!
                    };
                }),
            customerDetails: {
                name: summaryData.get("Name")!.toString(),
                surname: summaryData.get("Surname")!.toString(),
                phoneNumber: summaryData.get("PhoneNumber")!.toString(),
                email: summaryData.get("Email")!.toString()
            },
            adressDetails: {
                region: summaryData.get("Region")!.toString(),
                city: summaryData.get("City")!.toString(),
                postalCode: summaryData.get("PostalCode")!.toString(),
                streetName: summaryData.get("StreetName")!.toString(),
                houseNumber: summaryData.get("HouseNumber")!.toString(),
                apartmentNumber: summaryData.get("ApartmentNumber")?.toString(),
            }
        };

        console.log(data);
        console.log(JSON.stringify(data));
        const response = await fetch('/api/basket/summary', {
            method: "post",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data),
        });
        if (response.ok) {
            navigate("/store");
        } else response.json().then(data => setErrors(data.errors));
    };

    useEffect(() => {
        reload();
    }, []);

    useEffect(() => {
        console.log(errors);
        if (errors.other?.length > 0)
            alert(errors.other.join("\n"));
    }, [errors]);

    if (products.length == 0) return <main>Your basket is empty.</main>

    return <main className="basket">
        <div className="products">
            {groupedProducts.map(prods => <form className="supplier-cart" id={`sup-${prods[0].supplierId}`}>
                <h1>Products from {prods[0].supplierName}</h1>
                <select onChange={() => updatePrices()} name="DeliveryMethod">
                    <option value={DeliveryMethod.Standard}>{deliveryMethodLabel.get(DeliveryMethod.Standard)}</option>
                    <option value={DeliveryMethod.Express}>{deliveryMethodLabel.get(DeliveryMethod.Express)}</option>
                </select>
                {prods.map(group => group).map(prod => <div className="basketed-item">
                    <input type="checkbox" name="SelectedBasketIds" onChange={() => updatePrices()} value={prod.basketId} defaultChecked />
                    <div className="image-section">
                        <Link to="/product" state={{ supplierId: prod.supplierId, productId: prod.id }}>
                            <img src={prod.thumbnail} alt="product" />
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
            </form>)}
        </div>
        <form onSubmit={summarize} className="input-section" id={FormID.Summary}>
            <table className="grid-wide">
                <tr><th colSpan={2}>Summary</th></tr>
                <tr><td>Products cost</td><td>${productsPrice}</td></tr>
                <tr><td>Delivery cost</td><td>${deliveryCost}</td></tr>
                <tr><td>Payment method</td><td>{"???"}</td></tr>
                <tr><td>Total</td><td>${productsPrice + deliveryCost}</td></tr>
            </table>
            <div>
                <label htmlFor="name">Name</label>
                <input id="name" type="text" name="Name" required />
                {!!errors["CustomerDetails.Name"] && errors["CustomerDetails.Name"].map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
            </div>
            <div>
                <label htmlFor="surname">Surname</label>
                <input id="surname" type="text" name="Surname" required />
                {!!errors["CustomerDetails.Surname"] && errors["CustomerDetails.Surname"].map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
            </div>
            <div>
                <label htmlFor="phone-number">Phone number</label>
                <input id="phone-number" type="text" name="PhoneNumber" required />
                {!!errors["CustomerDetails.PhoneNumber"] && errors["CustomerDetails.PhoneNumber"].map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
            </div>
            <div>
                <label htmlFor="email">E-mail</label>
                <input id="email" type="text" name="Email" required />
                {!!errors["CustomerDetails.Email"] && errors["CustomerDetails.Email"].map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
            </div>
            <div>
                <label htmlFor="region">Region</label>
                <input id="region" type="text" name="Region" required />
                {!!errors["AdressDetails.Region"] && errors["AdressDetails.Region"].map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
            </div>
            <div>
                <label htmlFor="city">City</label>
                <input id="city" type="text" name="City" required />
                {!!errors["AdressDetails.City"] && errors["AdressDetails.City"].map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
            </div>
            <div>
                <label htmlFor="postal-code">Postal code</label>
                <input id="postal-code" type="text" name="PostalCode" required />
                {!!errors["AdressDetails.PostalCode"] && errors["AdressDetails.PostalCode"].map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
            </div>
            <div>
                <label htmlFor="street-name">Street name</label>
                <input id="street-name" type="text" name="StreetName" required />
                {!!errors["AdressDetails.StreetName"] && errors["AdressDetails.StreetName"].map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
            </div>
            <div>
                <label htmlFor="house-number">House number</label>
                <input id="house-number" type="text" name="HouseNumber" required />
                {!!errors["AdressDetails.HouseNumber"] && errors["AdressDetails.HouseNumber"].map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
            </div>
            <div>
                <label htmlFor="apartment-number">Apartment number</label>
                <input id="apartment-number" type="text" name="ApartmentNumber" />
                {!!errors["AdressDetails.ApartmentNumber"] && errors["AdressDetails.ApartmentNumber"].map((msg, i) => <span className="error-message" key={i}>{msg}</span>)}
            </div>
            <div className="submit-button grid-wide">
                <input type="submit" value="Submit" />
            </div>
        </form>
    </main>;
}

export default Basket;