import './Item.css';

interface ItemProps {
	name: string;
	price: number;
	images: string[];
	configuration: Record<string, string>;
	//link: string;
}

function Item({name, price, images, configuration}: ItemProps) {
    return <div className="item">
        <div className="image-section">
            <img src={images[0]} alt="" />
        </div>
        <div className="details-section">
            <h3 className="item-name">{name}</h3>
            <ul className="additional-details">
                {Object.entries(configuration).map(([_, value]) => <li>{value}</li>)}
            </ul>
        </div>
        <div className="store-section">
            <h3 className="store-price">${price}</h3>
            <div className="store-options">
                <input type="image" src="https://placehold.co/20x20" />
            </div>
        </div>
    </div>;
}

export default Item;