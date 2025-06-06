import { useRef } from 'react';
import './SelectList.css';

interface SelectListProps {
    label?: string;
    id: string;
    name: string;
    options: InputChoosable[];
    icons?: { [id: string]: string }
    selectedOption: InputChoosable;
    handler: () => void;
}
type InputChoosable = {
    label: string;
    value: string;
}

function SelectList({ label, id, name, options, icons, selectedOption, handler }: SelectListProps) {
    const toggleSelectList = () => {
        (selectListRef.current! as HTMLElement).classList.toggle("expanded");
    }

    const selectListRef = useRef(null);
    const inputRef = useRef(null);
    const selectOption = (value: string) => () => {
        toggleSelectList();
        const input: HTMLInputElement = inputRef.current!;
        input.value = value;
        handler();
    };
    
    const labelContent = label !== undefined
        ? <label htmlFor={id}>{label}</label>
        : <></>
    
    return <div className="setting-section">
        {labelContent}
        <div ref={selectListRef} className={"select-list" + (icons === undefined ? "" : " iconed-list")}>
            <span className="option" onClick={toggleSelectList}>{selectedOption.label}</span>
            {options.map((option, idx) => {
                return <span className="option" onClick={selectOption(option.value)} style={{ top: `${30 * (idx + 1)}px` }} key={idx}>
                    <p>{option.label}</p>
                    {icons !== undefined 
                    ? <img src={icons[option.label]} alt="list icon" />
                    : <></>
                    }
                </span>;
            })}
        </div>
        <input style={{ display: "none" }} ref={inputRef}
            type="text" className="option" name={name} id={id}
            defaultValue={selectedOption.value} />
    </div>;
}

export default SelectList;
  