import { Link } from 'react-router-dom';
import './Order.css';
import { ProductInfo } from '../../../shared/StoreObject/ProductInfo';

interface OrderProps {
    product: ProductInfo;
}

function Order({ product }: OrderProps) {
    return <div className="ordered-product">
        <div className="image-section">
            <Link to="/product" state={{ supplierId: product.supplierId, productId: product.id }}>
                <img src={product.thumbnail !== undefined ? product.thumbnail : "https://placehold.co/150x150"} alt="product" />
            </Link>
        </div>
        <div className="details-section">
            <h3 className="item-name">
                <Link to="/product" state={{ supplierId: product.supplierId, productId: product.id }}>
                    {product.name}
                </Link>
            </h3>
            <h3 className="price">${product.price}</h3>
            <h3 className="quantity">{product.quantity}</h3>
        </div>
    </div>;
}

export default Order;