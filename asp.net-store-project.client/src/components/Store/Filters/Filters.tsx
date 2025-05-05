import RangeFilter from './RangeFilter';
import CheckBoxFilter from './CheckBoxFilter';
import './Filters.css'
import React from 'react';

interface FiltersProps {
	priceRange: PriceRange;
    groupedTags: { [label: string]: ProductTag[] };
    applyFilters(): void,
    defaultFilters(): void,
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

function Filters({ priceRange, groupedTags, applyFilters, defaultFilters }: FiltersProps) {
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
        {Object.keys(groupedTags).map(label => <CheckBoxFilter label={label} options={groupedTags[label].sort((a, b) => a.order - b.order).map(tag => tag.parameter) ?? []} key={label} />)}
        <div className="apply-section">
            <input type="submit" onClick={handleSubmit(applyFilters)} className="apply-button" id="apply-filters" value="Apply filters" />
            <input type="submit" onClick={handleSubmit(defaultFilters)}  className="default-button" id="reset-filters" value="Return default" />
        </div>
    </form>;
}
 
export default Filters;