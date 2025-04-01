import './Paginator.css'

interface PaginatorProps {
    pages: number;
    selectedPageIndex: number;
    updateSettings(): void;
}

function Paginator({selectedPageIndex}: PaginatorProps) {
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
                name="PageIndex" defaultValue={selectedPageIndex} 
                data-current-page={selectedPageIndex} readOnly />
            <span className="page-changer">&#x25B8;</span>
        </div>
    </div>;
}

export default Paginator;