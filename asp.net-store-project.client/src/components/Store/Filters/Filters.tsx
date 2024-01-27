import RangeFilter from './RangeFilter';
import CheckBoxFilter from './CheckBoxFilter';
import './Filters.css'
import React from 'react';

interface FiltersProps {
    from: number;
    to: number;
    specifications: Record<string, string[]>;
    updateFilters: () => void;
    resetFilters: () => void;
}

function Filters({ from, to, specifications, updateFilters, resetFilters }: FiltersProps) {
    const handleSubmit = (handler: () => void) => {
        return (event: React.MouseEvent) => {
            event.preventDefault();
            handler();
        }
    }

    console.log("a", from, to);

    return <form className="filters" id="filters">
        <div className="title-section">
            <h2>Filters</h2>
            <input type="button" value="&#x2716;" />
        </div>
        <RangeFilter from={from} to={to} />
        {Object.entries(specifications).map(([label, configs]) => <CheckBoxFilter label={label} options={configs} />)}
        <div className="apply-section">
            <input type="submit" onClick={handleSubmit(updateFilters)} className="apply-button" id="apply-filters" value="Apply filters" />
            <input type="submit" onClick={handleSubmit(resetFilters)}  className="default-button" id="reset-filters" value="Return default" />
        </div>
    </form>;
}
 
export default Filters;