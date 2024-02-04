import './RangeFilter.css'

interface RangeFilterProps {
    from: number;
    to: number;
}

function RangeFilter({ from, to }: RangeFilterProps) {
    const toggleSection = (event: React.MouseEvent<HTMLElement>) => {
        (event.target as HTMLElement).parentElement!.classList.toggle("expanded");
    }

    return <div className="filter-section drop-down-section expanded">
        <label className="drop-down-label" onClick={toggleSection} htmlFor="price-from">Price</label>
        <div className="range-setting drop-down-content">
            <span className="range-input">
                <input type="text" id="price-from" name="PriceFrom" value={from} />
            </span>
            <span className="range-dash">&ndash;</span>
            <span className="range-input">
                <input type="text" id="price-to" name="PriceTo" value={to} />$
            </span>
        </div>
    </div>;
}

export default RangeFilter;