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
            <input ref={inputFrom} type="text" className="range-from" id="range-from" name="PriceFrom" onKeyDown={unfocusOnEnter} />
            <input type="text" className="range-separator" value="&ndash;" readOnly />
            <input ref={inputTo} type="text" className="range-to" id="range-to" name="PriceTo" onKeyDown={unfocusOnEnter} />
            <input type="text" className="range-currency" value="$" readOnly />
        </div>
    </div>;
}

export default RangeFilter;