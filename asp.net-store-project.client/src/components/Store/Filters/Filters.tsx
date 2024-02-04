import RangeFilter from './RangeFilter';
import CheckBoxFilter from './CheckBoxFilter';
import './Filters.css'
import React from 'react';

interface FiltersProps {
	priceRange: ValueRange;
    configurations: PossibleConfiguration[];
    updateFilters: () => void;
    resetFilters: () => void;
}
type ValueRange = {
	from: number;
	to: number;
}
type PossibleConfiguration = {
	label: string;
	parameters: string[];
}

function Filters({ priceRange, configurations, updateFilters, resetFilters }: FiltersProps) {
    const handleSubmit = (handler: () => void) => {
        return (event: React.MouseEvent) => {
            event.preventDefault();
            handler();
        }
    }

    return <form className="filters" id="filters">
        <div className="title-section">
            <h2>Filters</h2>
            <input type="button" value="&#x2716;" />
        </div>
        <RangeFilter from={priceRange.from} to={priceRange.to} />
        {configurations.map(config => <CheckBoxFilter label={config.label} options={config.parameters} />)}
        <div className="apply-section">
            <input type="submit" onClick={handleSubmit(updateFilters)} className="apply-button" id="apply-filters" value="Apply filters" />
            <input type="submit" onClick={handleSubmit(resetFilters)}  className="default-button" id="reset-filters" value="Return default" />
        </div>
    </form>;
}
 
export default Filters;