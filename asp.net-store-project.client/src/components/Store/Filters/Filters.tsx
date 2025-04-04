import RangeFilter from './RangeFilter';
import CheckBoxFilter from './CheckBoxFilter';
import './Filters.css'
import React from 'react';

interface FiltersProps {
	priceRange: PriceRange;
    relatedTags: { [label: string]: ProductTag[] };
    updateFilters: () => void;
    resetFilters: () => void;
}
type PriceRange = {
	from: number;
	to: number;
}
type ProductTag = {
    label: string;
    parameter: string;
    order: number;
}

function Filters({ priceRange, relatedTags, updateFilters, resetFilters }: FiltersProps) {
    const handleSubmit = (handler: () => void) => {
        return (event: React.MouseEvent) => {
            event.preventDefault();
            handler();
        }
    }

    console.log(relatedTags)

    return <form className="filters" id="filters">
        <div className="title-section">
            <h2>Filters</h2>
            <input type="button" value="&#x2716;" />
        </div>
        <RangeFilter from={priceRange.from} to={priceRange.to} />
        {Object.keys(relatedTags).map(label => <CheckBoxFilter label={label} options={relatedTags[label].map(tag => tag.parameter) ?? []} key={label} />)}
        <div className="apply-section">
            <input type="submit" onClick={handleSubmit(updateFilters)} className="apply-button" id="apply-filters" value="Apply filters" />
            <input type="submit" onClick={handleSubmit(resetFilters)}  className="default-button" id="reset-filters" value="Return default" />
        </div>
    </form>;
}
 
export default Filters;