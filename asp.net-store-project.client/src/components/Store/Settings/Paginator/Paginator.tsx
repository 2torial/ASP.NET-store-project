import './Paginator.css'

interface PaginatorProps {
    pages: number;
    selectedPage: number;
}

function Paginator({selectedPage}: PaginatorProps) {
    /* Do poprawy - input nie rejestruje zmian */
    const focusPageIndex = (event: React.MouseEvent) => {
        const pageIndex = (event.target as HTMLInputElement);
        pageIndex.classList.remove("idle");
        pageIndex.readOnly = false;
        pageIndex.select();
        pageIndex.focus();
        console.log(pageIndex);
    }

    const unfocusPageIndex = (event: React.FocusEvent) => {
        const pageIndex = (event.target as HTMLInputElement);
        const onlyPositiveNumbersPattern = /^[1-9][0-9]*/;
        if (!onlyPositiveNumbersPattern.test(pageIndex.value))
            pageIndex.value = pageIndex.dataset.currentPage!;
        pageIndex.classList.add("idle");
        pageIndex.readOnly = true;
        console.log(pageIndex);
    }

    return <div className="setting-section">
        <div className="paginator">
            <span className="page-changer">&#x25C2;</span>
            <input type="text" className="page-index idle" 
                onClick={focusPageIndex} onBlur={unfocusPageIndex} 
                name="PageIndex" defaultValue={selectedPage} 
                data-current-page={selectedPage} readOnly />
            <span className="page-changer">&#x25B8;</span>
        </div>
    </div>;
}

export default Paginator;