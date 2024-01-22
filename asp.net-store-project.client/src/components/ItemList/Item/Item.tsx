import './Item.css';

function Item(props) {
    return <div className="item">
        <div className="image-section">
            <img src="https://placehold.co/150/png" alt="" />
        </div>
        <div className="details-section">
            <h3 className="item-name">{props.details.name}</h3>
            <ul className="additional-details">
                {props.details.info.map(piece =>
                    <li>{piece}</li>
                )}
            </ul>
        </div>
        <div className="store-section">
            <h3 className="store-price">${props.details.price}</h3>
            <div className="store-options">
                <input type="image" src="https://placehold.co/20x20" />
            </div>
        </div>
    </div>;
}

export default Item;