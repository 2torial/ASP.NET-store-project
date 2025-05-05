import { ProductInfo } from '../../../../shared/StoreObject/ProductInfo';
import './Item.css';

interface ItemProps {
    product: ProductInfo;
}

function Item({ product }: ItemProps) {
    const addItem = (prod: ProductInfo) => async () => {
        const response = await fetch(`/api/basket/add/${prod.supplierId}/${prod.id}`);
        alert(await response.text());
    }

    return <div className="item">
        <div className="image-section">
            <img src={product.gallery.length > 0 ? product.gallery[0] : "https://placehold.co/150x150"} alt="" />
        </div>
        <div className="details-section">
            <h3 className="item-name">{product.name}</h3>
            <ul className="additional-details">
                {product.tags.sort((a, b) => a.label.localeCompare(b.label)).map((config, i) => <li key={i}>{`${config.label}: ${config.parameter}`}</li>)}
            </ul>
        </div>
        <div className="store-section">
            <h3 className="store-price">${product.price}</h3>
            <div className="store-options">
                <span onClick={addItem(product)} className="cart-icon fa fa-cart-plus" />
            </div>
        </div>
    </div>;
}

export default Item;