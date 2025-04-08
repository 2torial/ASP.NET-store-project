import { useEffect, useRef } from 'react';
import './Paginator.css'

interface PaginatorProps {
    pageCount: number;
    pageIndex: number;
    handler(): void;
}

function Paginator({ pageCount, pageIndex, handler }: PaginatorProps) {
    const pageIndexInput = useRef(null);
    const pageIndexDisplay = useRef(null);

    useEffect(() => {
        (pageIndexInput.current! as HTMLInputElement).value = String(pageIndex);
        (pageIndexDisplay.current! as HTMLInputElement).value = String(pageIndex + "/" + pageCount);
    });

    const focusPageIndex = () => {
        const input = pageIndexInput.current! as HTMLInputElement;
        input.classList.remove("hidden");
        input.select();
        input.focus();
    }

    const unfocusPageIndex = () => {
        const input = pageIndexInput.current! as HTMLInputElement;
        input.classList.add("hidden");

        const onlyPositiveNumbersPattern = /^[1-9][0-9]*/;
        if (!onlyPositiveNumbersPattern.test(input.value))
            input.value = input.dataset.currentPage!;
        else handler();
    }

    const unfocusOnEnter = (event: React.KeyboardEvent) => {
        if (event.key === 'Enter')
            unfocusPageIndex();
    }

    const moveUp = (event: React.MouseEvent) => {
        event.preventDefault();
        const input = pageIndexInput.current! as HTMLInputElement;
        input.value = String(parseInt(input.value) + 1);
        unfocusPageIndex();
    }

    const moveDown = (event: React.MouseEvent) => {
        event.preventDefault();
        const input = pageIndexInput.current! as HTMLInputElement;
        input.value = String(parseInt(input.value) - 1);
        unfocusPageIndex();
    }

    return <div className="setting-section">
        <div className="paginator">
            <button onClick={moveDown} className="page-changer">&#x25C2;</button>
            <span className="page-selector">
                <input ref={pageIndexInput} type="text" className="page-index hidden"
                    onBlur={unfocusPageIndex} onKeyDown={unfocusOnEnter}
                    name="PageIndex" data-current-page={pageIndex} />
                <input ref={pageIndexDisplay} type="text" className="page-display"
                    onClick={focusPageIndex} data-current-page={pageIndex} readOnly />
            </span>
            <button onClick={moveUp} className="page-changer">&#x25B8;</button>
        </div>
    </div>;
}

export default Paginator;