import { ProductInfo } from '../../../../../shared/StoreObject/ProductInfo';
import './OrderedItem.css';

interface OrderedItemProps {
    product: ProductInfo;
}

function OrderedItem({ product }: OrderedItemProps) {
    return <div className="item">
        <div className="image-section">
            <img src={product.gallery !== null && product.gallery.length > 0
                ? product.gallery[0] : "https://placehold.co/150x150"} alt="" />
        </div>
        <div className="details-section">
            <h3 className="item-name">{product.name}</h3>
            <ul className="additional-details">
                
            </ul>
        </div>
        <div className="store-section">
            <h3 className="store-price">${product.price}</h3>
            <div className="store-options">
                
            </div>
        </div>
    </div>;
}

export default OrderedItem;