import { useEffect, useRef } from 'react';
import './RangeFilter.css'

interface RangeFilterProps {
    from: number;
    to: number;
}

function RangeFilter({ from, to }: RangeFilterProps) {
    const inputFrom = useRef(null);
    const inputTo = useRef(null);

    useEffect(() => {
        (inputFrom.current! as HTMLInputElement).value = String(from);
        (inputTo.current! as HTMLInputElement).value = String(to);
    });

    const toggleSection = (event: React.MouseEvent<HTMLElement>) => {
        (event.target as HTMLElement).parentElement!.classList.toggle("expanded");
    }

    const unfocusOnEnter = (event: React.KeyboardEvent) => {
        if (event.key === 'Enter')
            (event.target as HTMLInputElement).blur();
    }

    return <div className="filter-section drop-down-section expanded">
        <label className="drop-down-label" onClick={toggleSection} htmlFor="price-from">Price</label>
        <div className="range-setting drop-down-content">
            <span className="range-input">
                <input ref={inputFrom} type="text" id="price-from" name="PriceFrom" onKeyDown={unfocusOnEnter} />
            </span>
            <span className="range-dash">&ndash;</span>
            <span className="range-input">
                <input ref={inputTo} type="text" id="price-to" name="PriceTo" onKeyDown={unfocusOnEnter} />$
            </span>
        </div>
    </div>;
}

export default RangeFilter;