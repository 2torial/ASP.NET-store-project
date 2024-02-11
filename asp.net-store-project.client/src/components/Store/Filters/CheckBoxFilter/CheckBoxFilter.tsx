import './CheckBoxFilter.css';

interface CheckBoxFilterProps {
    label: string;
    options: string[];
}

function CheckBoxFilter({ label, options }: CheckBoxFilterProps) {
    const toggleSection = (event: React.MouseEvent<HTMLElement>) => {
        (event.target as HTMLElement).parentElement!.classList.toggle("expanded");
    }

    return <div className="filter-section drop-down-section">
        <label className="drop-down-label" onClick={toggleSection}>{label}</label>
        <div className="checkbox-list drop-down-content">
            {options.map((option, i) => (
                <span className="checkbox-option" key={i}>
                    <input type="checkbox" id={option} name={label} value={option} />
                    <label htmlFor={option}>{option}</label>
                </span>
            ))}
        </div>
    </div>;
}

export default CheckBoxFilter;