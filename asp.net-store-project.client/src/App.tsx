import { useEffect, useState } from 'react';
import './App.css';

interface StoreItem {
    name: string;
    imageUrl: string;
    price: number;
    info: string[];
}

function App() {
    const [itemList, setItemList] = useState<StoreItem[]>();

    useEffect(() => {
        populateItemList();
    }, []);

    async function populateItemList() {
        const response = await fetch('itemlist');
        const data = await response.json();
        setItemList(data);
    }

    const items = itemList === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <>
            {itemList.map(item =>
                <div className="item">
                    <div className="image-section">
                        <img src={item.imageUrl} alt="" />
                    </div>
                    <div className="details-section">
                        <h3 className="item-name">{item.name}</h3>
                        <ul className="additional-details">
                            {item.info.map(piece =>
                                <li>{piece}</li>
                            )}
                        </ul>
                    </div>
                    <div className="store-section">
                        <h3 className="store-price">${item.price}</h3>
                        <div className="store-options">
                            <input type="image" src="https://placehold.co/20x20" />
                        </div>
                    </div>
                </div>
            )}
        </>;

    return (
        <div>
            <nav>
                <div className="navigation">
                    <div className="logo-section">
                        <input id="logo" type="image" src="https://placehold.co/120x50" alt="logo" />
                    </div>
                    <div className="empty-space-section"></div>
                    <div className="search-bar-section">
                        <form autoComplete="off">
                            <div className="search-bar">
                                <input className="search-bar-input-area" type="text" name="SearchBar" placeholder="Search" />
                                <input id="search-link" className="search-bar-button-area" type="image" src="https://placehold.co/40x40" alt="magnifier" />
                            </div>
                        </form>
                    </div>
                    <div className="menu-section">
                        <input id="basket-link" type="image" src="https://placehold.co/40x40" alt="basket" />
                        <input id="profile-link" type="image" src="https://placehold.co/40x40" alt="profile" />
                    </div>
                </div>
                <div className="menu">
                    <div className="empty-space"></div>
                    <div className="option-section">
                        <input id="sign-in-link" type="button" value="Sign in" />
                    </div>
                    <div className="option-section">
                        <input id="sign-up-link" type="button" value="Sign up" />
                    </div>
                </div>
            </nav>
            <main>
                <section className="filters">
                    <div className="title-section">
                        <h2>Filters</h2>
                        <input type="button" value="&#x2716;" />
                    </div>
                    <div className="filter-section drop-down-section expanded">
                        <label className="drop-down-label" htmlFor="price-from">Price</label>
                        <div className="range-setting drop-down-content">
                            <span className="range-input">
                                <input type="text" id="price-from" value="0" />
                            </span>
                            <span className="range-dash">&ndash;</span>
                            <span className="range-input">
                                <input type="text" id="price-to" value="999" />$
                            </span>
                        </div>
                    </div>
                    <div className="filter-section drop-down-section">
                        <label className="drop-down-label">Processor</label>
                        <div className="checkbox-list drop-down-content">
                            <span className="checkbox-option">
                                <input type="checkbox" id="Intel Core i3" value="Intel Core i3" />
                                <label htmlFor="Intel Core i3">Intel Core i3</label>
                            </span>
                            <span className="checkbox-option">
                                <input type="checkbox" id="Intel Core i5" value="Intel Core i5" />
                                <label htmlFor="Intel Core i5">Intel Core i5</label>
                            </span>
                            <span className="checkbox-option">
                                <input type="checkbox" id="Intel Core i7" value="Intel Core i7" />
                                <label htmlFor="Intel Core i7">Intel Core i7</label>
                            </span>
                            <span className="checkbox-option">
                                <input type="checkbox" id="Ryzen 3" value="Ryzen 3" />
                                <label htmlFor="Ryzen 3">Ryzen 3</label>
                            </span>
                            <span className="checkbox-option">
                                <input type="checkbox" id="Ryzen 5" value="Ryzen 5" />
                                <label htmlFor="Ryzen 5">Ryzen 5</label>
                            </span>
                            <span className="checkbox-option">
                                <input type="checkbox" id="Ryzen 7" value="Ryzen 7" />
                                <label htmlFor="Ryzen 7">Ryzen 7</label>
                            </span>
                        </div>
                    </div>
                    <div className="filter-section drop-down-section">
                        <label className="drop-down-label">RAM</label>
                        <div className="checkbox-list drop-down-content">
                            <span className="checkbox-option">
                                <input type="checkbox" id="8GB" value="8GB" />
                                <label htmlFor="8GB">8GB</label>
                            </span>
                            <span className="checkbox-option">
                                <input type="checkbox" id="16GB" value="16GB" />
                                <label htmlFor="16GB">16GB</label>
                            </span>
                            <span className="checkbox-option">
                                <input type="checkbox" id="32GB" value="32GB" />
                                <label htmlFor="32GB">32GB</label>
                            </span>
                        </div>
                    </div>
                    <div className="filter-section apply-section">
                        <input type="submit" className="apply-button" value="Apply filters" />
                        <input type="submit" className="default-button" value="Return default" />
                    </div>
                </section>
                <section className="settings">
                    <div className="setting-section">
                        <label htmlFor="category">Category</label>
                        <div className="select-list">
                            <input type="text" className="option" name="Category" id="category" value="Laptop" readOnly />
                            <span className="option" data-value="Any">Any</span>
                            <span className="option" data-value="Headset">Headset</span>
                            <span className="option" data-value="Laptop">Laptop</span>
                        </div>
                    </div>
                    <div className="setting-section">
                        <label htmlFor="sortby">Sort&nbsp;by</label>
                        <div className="select-list">
                            <input type="text" className="option" name="SortBy" id="sortby" value="Name: Ascending" readOnly />
                            <span className="option" data-value="Name: Ascending">Name: Ascending</span>
                            <span className="option" data-value="Name: Descending">Name: Descending</span>
                            <span className="option" data-value="Price: Lowest first">Price: Lowest first</span>
                            <span className="option" data-value="Price: Highest first">Price: Highest first</span>
                        </div>
                    </div>
                    <div className="setting-section">
                        <div className="paginator">
                            <span className="page-changer">&#x25C2;</span>
                            <input type="text" className="page-index idle" name="PageIndex" value="1" data-current-page="1" readOnly />
                            <span className="page-changer">&#x25B8;</span>
                        </div>
                    </div>
                    <div className="setting-section">
                        <div className="select-list iconed-select-list">
                            <input type="text" className="option" name="View" id="view" value="Gallery" readOnly />
                            <span className="option" data-value="Gallery">
                                <p>Gallery</p>
                                <img src="https://placehold.co/20x20" alt="gallery icon" />
                            </span>
                            <span className="option" data-value="List">
                                <p>List</p>
                                <img src="https://placehold.co/20x20" alt="list icon" />
                            </span>
                        </div>
                    </div>
                </section>
                <section className="item-list">
                    {items}
                </section>
            </main>
        </div>
    );
}

export default App;