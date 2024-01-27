import { useRef } from 'react';
import './SelectList.css'

interface SelectListProps {
    label?: string;
    id: string;
    name: string;
    options: string[];
    icons?: { [id: string]: string }
    selectedOption: string;
}

function SelectList({label, id, name, options, icons, selectedOption}: SelectListProps) {
    const toggleSelectList = (event: React.MouseEvent) => {
        (event.target as HTMLElement).classList.toggle("expanded");
    }

    const selectListRef = useRef(null);
    const topOptionRef = useRef(null);
    const selectOption = (event: React.MouseEvent) => {
        const selectList: HTMLElement = selectListRef.current!;
        const topOption: HTMLInputElement = topOptionRef.current!;
        const selectedOption = (event.target as HTMLElement);
        topOption.value = selectedOption.dataset.value!;
        selectList.classList.remove("expanded");
    }
    
    const labelContent = label !== undefined
        ? <label htmlFor={id}>{label}</label>
        : <></>
    
    return <div className="setting-section">
        {labelContent}
        <div ref={selectListRef} className={"select-list" + (icons === undefined ? "" : " iconed-list")}>
            <input ref={topOptionRef} type="text" className="option" onClick={toggleSelectList} name={name} id={id} value={selectedOption} readOnly />
            {options.map((option, idx) => {
                return <span className="option" onClick={selectOption} data-value={option} style={{top: `${30*(idx+1)}px`}}>
                    <p>{option}</p>
                    {icons !== undefined 
                    ? <img src={icons[option]} alt="list icon" />
                    : <></>
                    }
                </span>;
            })}
        </div>
    </div>;
}

export default SelectList;
  