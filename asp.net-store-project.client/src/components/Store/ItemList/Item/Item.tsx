import './Item.css';

interface ItemProps {
    id: number,
	name: string;
	price: number;
	gallery: string[];
	specification: Configuration[];
	pageLink?: string;
}
type Configuration = {
	label: string;
	parameter: string;
}

function Item({ id, name, price, gallery, specification, pageLink }: ItemProps) {
    const addItem = (event: React.MouseEvent) => {
        event.preventDefault();
        const data = new FormData();
        data.append("Id", id.toString());
        const response = await fetch('/api/basket/item/add', {
            method: "post",
            body: data
        });
        
    }
    return <div className="item">
        <div className="image-section">
            <img src={gallery.length > 0 ? gallery[0] : ""} alt="" />
        </div>
        <div className="details-section">
            <h3 className="item-name">{name}</h3>
            <ul className="additional-details">
                {specification.map(config => <li>{`${config.label}: ${config.parameter}`}</li>)}
            </ul>
        </div>
        <div className="store-section">
            <h3 className="store-price">${price}</h3>
            <div className="store-options">
                <input onClick={addItem} type="image" src="https://placehold.co/20x20" />
            </div>
        </div>
    </div>;
}

export default Item;