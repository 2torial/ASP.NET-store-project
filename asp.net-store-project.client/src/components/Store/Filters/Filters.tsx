import RangeFilter from './RangeFilter';
import CheckBoxFilter from './CheckBoxFilter';
import './Filters.css'
import React from 'react';

interface FiltersProps {
	priceRange: PriceRange;
    relatedTags: KeyValuePair[];
    updateFilters: () => void;
    resetFilters: () => void;
}
type PriceRange = {
	from: number;
	to: number;
}
type KeyValuePair = {
    key: string;
    value: string[];
}

function Filters({ priceRange, relatedTags, updateFilters, resetFilters }: FiltersProps) {
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
        {relatedTags.map(relatedParameters => <CheckBoxFilter label={relatedParameters.key} options={relatedParameters.value} key={relatedParameters.key} />)}
        <div className="apply-section">
            <input type="submit" onClick={handleSubmit(updateFilters)} className="apply-button" id="apply-filters" value="Apply filters" />
            <input type="submit" onClick={handleSubmit(resetFilters)}  className="default-button" id="reset-filters" value="Return default" />
        </div>
    </form>;
}
 
export default Filters;