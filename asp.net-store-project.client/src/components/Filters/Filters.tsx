import './Filters.css'

function Filters() {
    return <section className="filters">
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
    </section>;
}
 
export default Filters;