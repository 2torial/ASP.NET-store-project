import { useRef } from 'react';
import './SelectList.css';

interface CombinedSelectListProps {
    id: string;
    label?: string;
    nameA: string;
    nameB: string;
    optionsA: InputChoosable[];
    optionsB: InputChoosable[];
    selectedOptionA: InputChoosable;
    selectedOptionB: InputChoosable;
    handler(): void;
}
type InputChoosable = {
    label: string;
    value: string;
}

function CombinedSelectList({ label, id, nameA, nameB, optionsA, optionsB, selectedOptionA, selectedOptionB, handler }: CombinedSelectListProps) {
    const toggleSelectList = () => {
        (selectListRef.current! as HTMLElement).classList.toggle("expanded");
    }

    const selectListRef = useRef(null);
    const inputRefA = useRef(null);
    const inputRefB = useRef(null);
    const selectOption = (valueA: string, valueB: string) => () => {
        toggleSelectList();
        const inputA: HTMLInputElement = inputRefA.current!;
        const inputB: HTMLInputElement = inputRefB.current!;
        inputA.value = valueA;
        inputB.value = valueB;
        handler();
    };
    
    const labelContent = label !== undefined
        ? <label htmlFor={id+"-A"}>{label}</label>
        : <></>;

    let combinedIdx = 0;

    return <div className="setting-section">
        {labelContent}
        <div ref={selectListRef} className={"select-list"}>
            <span className="option" onClick={toggleSelectList}>{selectedOptionA.label + ": " + selectedOptionB.label}</span>
            {optionsA.map(optionA =>
                optionsB.map(optionB =>
                    <span className="option" onClick={selectOption(optionA.value, optionB.value)} style={{ top: `${30 * (++combinedIdx)}px` }} key={combinedIdx}>
                        <p>{optionA.label + ": " + optionB.label}</p>
                    </span>)
            )}
        </div>
        <input style={{ display: "none" }} ref={inputRefA}
            type="text" className="option" name={nameA} id={id + "-A"}
            onClick={toggleSelectList} defaultValue={selectedOptionA.value} />
        <input style={{ display: "none" }} ref={inputRefB}
            type="text" className="option" name={nameB} id={id + "-B"}
            defaultValue={selectedOptionB.value} />
    </div>;
}

export default CombinedSelectList;
  