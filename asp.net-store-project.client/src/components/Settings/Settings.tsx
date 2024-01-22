import './Settings.css'

function Settings() {
    return <section className="settings">
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
    </section>;
}

export default Settings;